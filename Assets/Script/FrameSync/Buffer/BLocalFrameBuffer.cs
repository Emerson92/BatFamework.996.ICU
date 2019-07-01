using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.SyncSystem.FrameSync.Struct;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.BBuffer {

    public class BLocalFrameBuffer : BFrameBufferCore<BFrameCommend>
    {

        public BLocalFrameBuffer(uint cacheNum) : base(cacheNum)
        {

        }

        public override void EnQuene(BFrame<BFrameCommend> data)
        {
            bufferQuene.Add(data);
        }

        public override BFrame<BFrameCommend>[] DeQuenes(uint frameIndex, bool force = false)
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

        private BFrame<BFrameCommend>[] PushBuffer()
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

