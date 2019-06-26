using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.Buffer {

    public class BFrameBufferCore<T> where T : class{

        private List<BFrame<T>> bufferQuene = new List<BFrame<T>>(16);

        private uint cacheNum;

        private BFrame<T>[] cacheBuffer;

        private uint CacheNum {
            set {
                this.cacheNum = value;
                cacheBuffer = new BFrame<T>[this.cacheNum];
            }
            get {
                return cacheNum;
            }
        }

        private Action<BFrame<T>[]> DeQueneCallback;

        public BFrameBufferCore(uint cacheNum = 1) {
            this.CacheNum = (cacheNum == 0 ? 1 : cacheNum);
        }

        public void SetDeQueneCallback(Action<BFrame<T>[]> callback) {

            if (callback != null)
                DeQueneCallback = callback;
        }

        public void EnQuene(BFrame<T> data) {
            ////TODO  add the data to quene
            AddAndSortBufferQuene(data);
        }

        /// <summary>
        /// force to refresh the buffer data
        /// </summary>
        /// <param name="force"></param>
        public void RefreshBuffer(bool force = false) {
            if (CheckIsNeedPushBuffer())
            {
                PushBuffer();
            }
            else {
                if (force)
                    PushBuffer();
            }
        }

        private void PushBuffer()
        {
            for (int i = 0; i < cacheNum; i++)
            {
                cacheBuffer[i] = bufferQuene[i];
                bufferQuene.RemoveAt(i);
            }
            if (DeQueneCallback != null)
                DeQueneCallback(cacheBuffer);
        }

        private void AddAndSortBufferQuene(BFrame<T> frame)
        {
            bool isNewFrame = true;
            for (int i = 0 ;i < bufferQuene.Count; i++) {
                ////if we have the same frame,so we replace it in the quene
                if (bufferQuene[i].FrameNum == frame.FrameNum) {
                    bufferQuene[i] = frame;
                    isNewFrame = false;
                }
            }
            if (!isNewFrame) {
                /////Add new frame and sort order
                bufferQuene.Add(frame);
                bufferQuene.Sort((a,b)=> {
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
        private bool CheckIsNeedPushBuffer()
        {
            for (int i = 0; i < CacheNum-1; i++) {
                if (bufferQuene[i].FrameNum + 1 == bufferQuene[i + 1].FrameNum)
                    continue;
                else
                    return false;
            }
            return true;
        }
    }

    /// <summary>
    /// the struct contain every frame data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct BFrame<T> where T : class
    {

        public uint FrameNum;

        public T Data;
    
    }
}