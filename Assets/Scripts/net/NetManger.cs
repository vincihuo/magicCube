using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Network
{
    public delegate void ReqHandle();
    public class WebSocketWrapper: EventManager
    {
        private ushort requestId = 0;
        private const int ReceiveChunkSize = 1024;
        private const int SendChunkSize = 1024;

        private readonly ClientWebSocket _ws;
        private readonly Uri _uri;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly CancellationToken _cancellationToken;

        private Action<WebSocketWrapper> _onConnected;
        private Action<byte[], WebSocketWrapper> _onMessage;
        private Action<WebSocketWrapper> _onDisconnected;

        private PingPong pingpong;

        protected WebSocketWrapper(string uri)
        {
            _ws = new ClientWebSocket();
            _ws.Options.KeepAliveInterval = TimeSpan.FromSeconds(20);
            _uri = new Uri(uri);
            pingpong = new PingPong(SendMessage,10,30);
            pingpong.Register(PingPong.LONG_NO_PONG, disconnect);
            _cancellationToken = _cancellationTokenSource.Token;
        }
        private void disconnect(byte[] mm) 
        {
            _ws.Abort();
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="uri">The URI of the WebSocket server.</param>
        /// <returns></returns>
        public static WebSocketWrapper Create(string uri)
        {
            return new WebSocketWrapper(uri);
        }

        /// <summary>
        /// Connects to the WebSocket server.
        /// </summary>
        /// <returns></returns>
        public WebSocketWrapper Connect()
        {
            ConnectAsync();
            return this;
        }

        /// <summary>
        /// Set the Action to call when the connection has been established.
        /// </summary>
        /// <param name="onConnect">The Action to call.</param>
        /// <returns></returns>
        public WebSocketWrapper OnConnect(Action<WebSocketWrapper> onConnect)
        {
            _onConnected = onConnect;
            return this;
        }

        /// <summary>
        /// Set the Action to call when the connection has been terminated.
        /// </summary>
        /// <param name="onDisconnect">The Action to call</param>
        /// <returns></returns>
        public WebSocketWrapper OnDisconnect(Action<WebSocketWrapper> onDisconnect)
        {
            _onDisconnected = onDisconnect;
            return this;
        }

        /// <summary>
        /// Set the Action to call when a messages has been received.
        /// </summary>
        /// <param name="onMessage">The Action to call.</param>
        /// <returns></returns>
        public WebSocketWrapper OnMessage(Action<byte[], WebSocketWrapper> onMessage)
        {
            _onMessage = onMessage;
            return this;
        }

        /// <summary>
        /// Send a message to the WebSocket server.
        /// </summary>
        /// <param name="message">The message to send</param>
        public void SendMessage(string message)
        {
            SendMessageAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text);
        }
        public void Register(object mainId, object subId, EventMgr eventMgr)
        {
            Register(mainId + "_" + subId, eventMgr);
        }
        public void UnRegister(object mainId, object subId, EventMgr eventMgr)
        {
            UnRegister(mainId + "_" + subId, eventMgr);
        }

        public void SendMessage(object mainId,object subId,IMessage message,bool islock=false, ReqHandle handle=null)
        {
            Byte[] buff = message.ToByteArray();
            Header header = new Header();
            header.len = (ushort)(Header.HeaderLen + buff.Length);
            header.mainId = (byte)mainId;
            header.subId = (byte)subId;
            header.realSize = (ushort)buff.Length;
            header.requestId = getRequestId();
            SendMessage(header.package(buff));
        }
        private ushort getRequestId() 
        {
            if (requestId > 32767)
            {
                requestId = 0;
            }
            return requestId++;
        }

        public void SendMessage(byte[] messageBuffer)
        {
            SendMessageAsync(messageBuffer);
        }

        private async void SendMessageAsync(byte[] messageBuffer, WebSocketMessageType type= WebSocketMessageType.Binary)
        {
            var messagesCount = (int)Math.Ceiling((double)messageBuffer.Length / SendChunkSize);

            for (var i = 0; i < messagesCount; i++)
            {
                var offset = (SendChunkSize * i);
                var count = SendChunkSize;
                var lastMessage = ((i + 1) == messagesCount);

                if ((count * (i + 1)) > messageBuffer.Length)
                {
                    count = messageBuffer.Length - offset;
                }

                await _ws.SendAsync(new ArraySegment<byte>(messageBuffer, offset, count),type, lastMessage, _cancellationToken);
            }
        }

        private async void ConnectAsync()
        {
            await _ws.ConnectAsync(_uri, _cancellationToken);
            CallOnConnected();
            StartListen();
        }

        private async void StartListen()
        {
            var buffer = new byte[ReceiveChunkSize];

            try
            {
                while (_ws.State == WebSocketState.Open)
                {
                    var stringResult = new StringBuilder();
                    List<byte> bs = new List<byte>();
                    WebSocketReceiveResult result;
                    do
                    {
                        result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationToken);


                        if (result.MessageType == WebSocketMessageType.Text) 
                        {
                            var str = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            stringResult.Append(str);
                            Debug.Log(stringResult.ToString());
                            pingpong.receivePong();
                        }
                        else if (result.MessageType == WebSocketMessageType.Binary)
                        {
                            bs.AddRange(buffer.Take(result.Count));
                        }

                        else if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await
                                _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                            CallOnDisconnected();
                        }
                    } while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Binary) 
                    {
                        CallOnMessage(bs.ToArray());
                    }
                }
            }
            catch (Exception)
            {
                CallOnDisconnected();
            }
            finally
            {
                _ws.Dispose();
            }
        }

        private void CallOnMessage(Byte[] buffer)
        {
            NetStream stream = new NetStream(buffer);

            Header header = new Header();
            if (header.unpackage(stream)) 
            {
                Debug.Log("crc ²»Ò»ÖÂ");
            }
            Byte[] proto = stream.ReadBytes(header.realSize);
            Invoke(header.mainId+"_"+header.subId, proto);

            if (_onMessage != null)
                RunInTask(() => _onMessage(buffer, this));
        }

        private void CallOnDisconnected()
        {
            pingpong.stopHeart();
            if (_onDisconnected != null)
                RunInTask(() => _onDisconnected(this));
        }

        private void CallOnConnected()
        {
            pingpong.startHeart("ping");
            if (_onConnected != null)
                RunInTask(() => _onConnected(this));
        }

        private static void RunInTask(Action action)
        {
            Task.Factory.StartNew(action);
        }
    }
}