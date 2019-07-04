using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.SyncSystem.FrameSync.BStruct;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.BBuffer {

    public class BLocalFrameBuffer<T> : BFrameBufferCore<T> where T :class
    {

        public BLocalFrameBuffer(uint cacheNum) : base(cacheNum)
        {

        }

        public override void EnQuene(BFrame<T> data)
        {
            bufferQuene.Add(data);
        }

        public override BFrame<T>[] DeQuenes(uint frameIndex, bool force = false)
        {
            return PushBuffer();
        }

        public override void RefreshBuffer(bool force = false)
        {
            
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        private BFrame<T>[] PushBuffer()
        {
            for (int i = 0; i < CacheNum; i++)
            {
                cacheBuffer[i] = bufferQuene[i];
                bufferQuene.RemoveAt(i);
            }
            return cacheBuffer;
        }
    }

}

