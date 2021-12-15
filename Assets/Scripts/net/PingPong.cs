using System;
using UnityEngine;
using Util;

namespace Network
{
    public class PingPong : EventManager
    {
        public static string LONG_NO_PONG = "longnopong";
        private bool _pingpong;
        private float waittime;
        private string pingdata;
        private Action<string> sendMsg;
        private float pintime;
        private float receiveTime;
        private float disconnectTime;
        private float sendTime;
        private bool sendLong = false;
        private float runTime;
        public PingPong(Action<string> fun, int sendcd, int disTime)
        {
            disconnectTime = disTime;
            pintime = sendcd;
            sendMsg = fun;
            new Timer(1, update, -1);
        }


        private void update()
        {
            if (_pingpong)
            {
                waittime -= 1;
                if (waittime < 0)
                {
                    waittime = pintime;
                    sendTime = System.DateTime.Now.Second;
                    sendMsg(pingdata);
                }
            }
            if (!sendLong)
            {
                runTime -= 1;
                if (runTime < 0)
                {
                    Invoke(LONG_NO_PONG,null);
                    sendLong = true;
                }
            }
        }
        public void receivePong()
        {
            runTime = disconnectTime;
            receiveTime = System.DateTime.Now.Second;
        }
        public float ping
        {
            get { return receiveTime - sendTime; }
        }

        public void startHeart(string seesion)
        {
            runTime = disconnectTime;
            pingdata = seesion;
            _pingpong = true;
            waittime = 0;
            receiveTime = System.DateTime.Now.Second;
            sendTime = receiveTime;
            sendLong = false;
        }
        public void stopHeart()
        {
            _pingpong = false;
            sendLong = true;
            receiveTime = System.DateTime.Now.Second;
        }
    }

}