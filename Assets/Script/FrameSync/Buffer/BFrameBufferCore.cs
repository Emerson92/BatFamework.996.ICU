using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.BBuffer {

    public abstract class BFrameBufferCore<T> where T : class{

        protected List<BFrame<T>> bufferQuene = new List<BFrame<T>>(16);

        private uint cacheNum;

        protected BFrame<T>[] cacheBuffer;

        public uint CacheNum {
            set {
                this.cacheNum = value;
                cacheBuffer = new BFrame<T>[this.cacheNum];
            }
            get {
                return cacheNum;
            }
        }

        public BFrameBufferCore(uint cacheNum = 1) {
            this.CacheNum = (cacheNum == 0 ? 1 : cacheNum);
        }

        public virtual void EnQuene(BFrame<T> data) {
            ////TODO  add the data to quene
            bufferQuene.Add(data);
        }

        public virtual BFrame<T>[] DeQuenes(uint frameIndex, bool force = false) {

            return cacheBuffer;
        }

        public virtual BFrame<T>? DeQuene(uint frameIndex, bool force = false) {
            BFrame<T> topFrame = bufferQuene[0];
            bufferQuene.RemoveAt(0);
            return topFrame;
        }

        /// <summary>
        /// force to refresh the buffer data
        /// </summary>
        /// <param name="force"></param>
        public virtual void RefreshBuffer(bool force = false) {

        }

        /// <summary>
        /// Dynamic expend Cache
        /// </summary>
        /// <param name="volume"></param>
        public void DynamicExpendCache(uint volume)
        {
            CacheNum = volume;
        }

        /// <summary>
        /// Dispose 
        /// </summary>
        public virtual void Dispose() {
            bufferQuene.Clear();
        } 
    }

    /// <summary>
    /// the struct contain every frame data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct BFrame<T> where T : class
    {

        public uint FrameNum;

        public T Cmd;
    
    }
}