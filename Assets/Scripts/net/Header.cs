
using Google.Protobuf;
using System;
using System.IO;
namespace Network
{
    public class Header
    {
        public const int HeaderLen= 18;
        public ushort len;
        public ushort crc;
        public ushort ver=0x001;
        public ushort sign=0xaabb;
        public byte mainId;
        public byte subId;
        public byte encryptType=0;
        public byte other=0;
        public ushort realSize;
        public ushort requestId;
        public bool unpackage(NetStream stream)
        {
            if (stream.getSize() < HeaderLen)
            {
                return false;
            }
            len = stream.ReadUshort();
            crc = stream.ReadUshort();
            long pos = stream.GetPos();
            long bufflen = stream.getSize() - 4;
            if (MsgTool.GetCrc(stream, bufflen) != crc)
            {
                return false;
            }
            stream.SetPos(pos);
            ver = stream.ReadUshort();
            sign = stream.ReadUshort();
            encryptType = stream.ReadByte();
            other = stream.ReadByte();
            mainId = stream.ReadByte();
            subId = stream.ReadByte();
            requestId = stream.ReadUshort();
            realSize = stream.ReadUshort();
            return true;
        }
        public Byte[] package(byte[] buff)
        {
            NetStream stream = new NetStream();
            stream.WriteUshort(ver);
            stream.WriteUshort(sign);
            stream.WriteByte(encryptType);
            stream.WriteByte(other);
            stream.WriteByte(mainId);
            stream.WriteByte(subId);
            stream.WriteUint(requestId);
            stream.WriteUshort(realSize);
            stream.WriteBytes(buff);
            long blen = stream.getSize();
            stream.SetPos(0);
            crc = MsgTool.GetCrc(stream, blen);

            NetStream tstream = new NetStream();
            tstream.WriteUshort(len);
            tstream.WriteUshort(crc);
            tstream.WriteBytes(stream.GetBuffer());
            return tstream.GetBuffer();
        }

    }
}

