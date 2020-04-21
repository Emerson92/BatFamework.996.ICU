using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.Network.TcpSocket
{
    public class ByteArray
    {

        //默认大小
        public const int DEFAULT_SIZE = 1024;

        //初始化大小
        int initSize = 0;

        //缓存区
        public byte[] Bytes;

        //读写位置
        public int ReadIdx = 0;

        //写入位置
        public int WriteIdx = 0;

        //容量
        private int capacity = 0;

        //剩余空间
        public int Remain { get { return capacity - WriteIdx; } }

        //数据有效长度
        public int Length { get { return WriteIdx - ReadIdx; } }

        public ByteArray(int size = DEFAULT_SIZE)
        {
            Bytes = new byte[size];
            capacity = size;
            initSize = size;
            ReadIdx = 0;
            WriteIdx = 0;
        }

        public ByteArray(byte[] defaultBytes)
        {
            Bytes = defaultBytes;
            capacity = defaultBytes.Length;
            ReadIdx = 0;
            WriteIdx = defaultBytes.Length;
        }

        /// <summary>
        /// 重新规划缓冲区大小
        /// </summary>
        /// <param name="size"></param>
        public void Resize(int size)
        {
            if (size < Length) return;
            if (size < initSize) return;
            int n = 1;
            while (n < size) n *= 2;
            capacity = n;
            byte[] newBytes = new byte[capacity];
            Array.Copy(Bytes, ReadIdx, newBytes, 0, WriteIdx - ReadIdx);
            Bytes = newBytes;
            WriteIdx = Length;///重新记录没有读取过的数据
            ReadIdx = 0;
        }

        public void CheckAndMoveBytes()
        {
            if (Length < 8) MoveBytes();
        }

        public void MoveBytes()
        {
            if (Length > 0) Array.Copy(Bytes, ReadIdx, Bytes, 0, Length);
            WriteIdx = Length;
            ReadIdx = 0;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            WriteIdx = 0;
            ReadIdx = 0;
        }

        //写入
        public int Write(byte[] bs, int offset, int count)
        {
            if (Remain < count)
            {
                Resize(count + Length);
            }
            Array.Copy(bs, offset, Bytes, WriteIdx, count);
            WriteIdx += count;
            return count;
        }

        //读取
        public int Read(byte[] bs, int offset, int count)
        {
            if (Length <= 0) return 0;
            count = Math.Min(count, Length);
            Array.Copy(Bytes, ReadIdx, bs, offset, count);
            ReadIdx += count;
            CheckAndMoveBytes();
            return count;
        }

        public Int16 ReadInt16()
        {
            if (Length <= 2) return 0;
            Int16 ret;
            if (TcpSocketClientMgr.TransportLittleEndian)
            {
                ret = (Int16)(Bytes[ReadIdx + 1] << 8 | Bytes[ReadIdx]);
            }
            else
            {
                ret = (Int16)(Bytes[ReadIdx + 1] | Bytes[ReadIdx] << 8);
            }
            //ReadIdx += 2;
            //heckAndMoveBytes();
            return ret;
        }

        public Int32 ReadInt32()
        {
            if (Length <= 4) return 0;
            Int32 ret;
            if (TcpSocketClientMgr.TransportLittleEndian)
            {
                ret = (Int32)(Bytes[ReadIdx + 3] << 24 | Bytes[ReadIdx + 2] << 16 | Bytes[ReadIdx + 1] << 8 | Bytes[ReadIdx]);
            }
            else
            {
                ret = (Int32)(Bytes[ReadIdx] << 24 | Bytes[ReadIdx + 1] << 16 | Bytes[ReadIdx + 2] << 8 | Bytes[ReadIdx + 3]);
            }
            //ReadIdx += 4;
            //CheckAndMoveBytes();
            return ret;
        }

        //输出缓存区数据
        public string Debug()
        {
            return BitConverter.ToString(Bytes, ReadIdx, Length);
        }

        public override string ToString()
        {
            return string.Format("readIdx({0}) writeIdx({1}) bytes({2})", ReadIdx, WriteIdx, Bytes.Length);
        }
    }
}
