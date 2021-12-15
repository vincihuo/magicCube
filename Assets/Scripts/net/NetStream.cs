using System.IO;
using System.Net;
using System;

namespace Network
{
    public class NetStream
    {
        public static readonly bool LittleEndian = true;
        private MemoryStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        public NetStream(byte[] buffer = null)
        {
            if (buffer == null)
            {
                stream = new MemoryStream();
            }
            else
            {
                stream = new MemoryStream(buffer);
            }
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
        }
        public byte[] GetBuffer()
        {
            return stream.ToArray();
        }
        public void WriteByte(byte v)
        {
            writer.Write(v);
        }
        public void WriteShort(short v)
        {
            writer.Write(BitConverter.GetBytes(v));
        }
        public void WriteUshort(ushort v)
        {
            writer.Write(BitConverter.GetBytes(v));
        }

        public void WriteInt(int v)
        {
            writer.Write(BitConverter.GetBytes(v));
        }
        public void WriteUint(uint v)
        {
            writer.Write(BitConverter.GetBytes(v));
        }

        public void WriteInt64(long v)
        {
            writer.Write(BitConverter.GetBytes(v));
        }

        public void WriteBytes(byte[] v)
        {
            writer.Write(v);
        }

        public byte ReadByte()
        {
            return reader.ReadByte();
        }
        public short ReadShort()
        {
            return reader.ReadInt16();
        }

        public ushort ReadUshort()
        {
            return  BitConverter.ToUInt16(reader.ReadBytes(2),0);
        }
        public int ReadInt()
        {
            return reader.ReadInt32();
        }

        public uint ReadUint() 
        {
            return BitConverter.ToUInt32(reader.ReadBytes(4), 0);
        }
        
        public long ReadInt64() 
        {
            return reader.ReadInt64();
        }

        public ulong ReadUint64()
        {
            return BitConverter.ToUInt64(reader.ReadBytes(8), 0);
        }


        public byte[] ReadBytes(int len)
        {
            return reader.ReadBytes(len);
        }
        public long GetPos()
        {
            return stream.Position;
        }
        public void SetPos(long p)
        {
            stream.Position = p;
        }

        public long getSize() 
        {
            return stream.Length;
        }
        public void flush() 
        {
            writer.Flush();
        }
    }
}