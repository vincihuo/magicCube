
using Google.Protobuf;
using System;
using System.IO;

namespace Network
{
    public class MsgTool
    {
        public static byte[] packageMsg(IMessage msg)
        {
            MemoryStream stream = new MemoryStream();
            CodedOutputStream writer = new CodedOutputStream(stream);
            Header header = new Header();

            writer.WriteMessage(msg);
            writer.Flush();
            return stream.ToArray();
        }

        public static byte[] Serialize<T>(T obj) where T : IMessage
        {
            byte[] data = obj.ToByteArray();
            return data;
        }

        public static T Deserialize<T>(byte[] data) where T : class, IMessage, new()
        {
            T obj = new T();
            IMessage message = obj.Descriptor.Parser.ParseFrom(data);
            return message as T;
        }
        public static short SwapEndina(short v)
        {
            return (short)(((v & 0xff) << 8) | (((v >> 8) & 0xff)));
        }
        public static ushort SwapEndina(ushort v)
        {
            return (ushort)(((v & 0xff)) << 8 | ((v >> 8) & 0xff));
        }
        public static int SwapEndina(int v)
        {
            return (int)(((SwapEndina((short)v) & 0xffff) << 0x10) |
                         (SwapEndina((short)(v >> 0x10)) & 0xffff));
        }
        public static uint SwapEndina(uint v)
        {
            return (uint)(((SwapEndina((ushort)v) & 0xffff) << 0x10) |
                  (SwapEndina((ushort)(v >> 0x10)) & 0xffff));
        }

        public static long SwapEndina(long n)
        {
            return (long)(((SwapEndina((int)n) & 0xffffffffL) << 0x20) |
                           (SwapEndina((int)(n >> 0x20)) & 0xffffffffL));
        }
        public static ulong SwapEndina(ulong n)
        {
            return (ulong)(((SwapEndina((uint)n) & 0xffffffffL) << 0x20) |
                            (SwapEndina((uint)(n >> 0x20)) & 0xffffffffL));
        }
        public static ushort GetCrc(NetStream stream ,long len)
        {
            long bufflen = len / 2;
            var sum = 0;
            for (int i = 0; i < bufflen; ++i)
            {
                sum = stream.ReadUshort();
            }
            if (len % 2 != 0)
            {
                sum += stream.ReadByte();
            }
            return (ushort)(sum % 65536);
        }
    }
}
