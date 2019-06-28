using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.SyncSystem.FrameSync.Struct;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.Buffer
{

    public class BNetworkFrameBuffer : BFrameBufferCore<BFrameCommend>
    {

        public BNetworkFrameBuffer(uint cacheNum = 1) : base(cacheNum)
        {

        }

        public override BFrame<BFrameCommend>[] DeQuene(int frameIndex, bool force = false)
        {
            if (CheckIsNeedPushBuffer(frameIndex))
            {
                return PushBuffer();
            }
            else
            {
                if (force)
                  return  PushBuffer();
            }
            return null;
        }

        public override void EnQuene(BFrame<BFrameCommend> data)
        {
            AddAndSortBufferQuene(data);
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

        private void AddAndSortBufferQuene(BFrame<BFrameCommend> frame)
        {
            bool isNewFrame = true;
            for (int i = 0; i < bufferQuene.Count; i++)
            {
                ////if we have the same frame,so we replace it in the quene
                if (bufferQuene[i].FrameNum == frame.FrameNum)
                {
                    bufferQuene[i] = frame;
                    isNewFrame = false;
                }
            }
            if (!isNewFrame)
            {
                /////Add new frame and sort order
                bufferQuene.Add(frame);
                bufferQuene.Sort((a, b) =>
                {
                    if (a.FrameNum == b.FrameNum)
                        return 0;
                    else if (a.FrameNum > b.FrameNum)
                        return 1;
                    else
                        return -1;
                });
            }
        }

        /// <summary>
        /// Check the frame index keep right order 
        /// </summary>
        /// <returns>true/false</returns>
        private bool CheckIsNeedPushBuffer(int frameIndex)
        {
            for (int i = 0; i < CacheNum - 1; i++)
            {
                if (frameIndex == bufferQuene[0].FrameNum)////Check the current networkframe is equal the buffer top element's frameNum
                {
                    if (bufferQuene[i].FrameNum + 1 == bufferQuene[i + 1].FrameNum)
                        continue;
                    else
                        return false;
                }
                else {
                    return false;
                }
            }
            return true;
        }
    }

}
