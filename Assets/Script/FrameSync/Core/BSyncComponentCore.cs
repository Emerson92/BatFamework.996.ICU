using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.SyncSystem.FrameSync.BBuffer;
using THEDARKKNIGHT.SyncSystem.FrameSync.ExtendMethod;
using THEDARKKNIGHT.SyncSystem.FrameSync.Interface;
using THEDARKKNIGHT.SyncSystem.FrameSync.BStruct;
using THEDARKKNIGHT.SyncSystem.FrameSync.Utility;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync {

    public abstract class BSyncComponentCore : ISyncComponent,IRoallbackable, IFrameConfirm
    {
        public enum SYNCTYPE {
            NULL,
            TRANSFORM,
            POSITION,
            ROTATION,
            SCALE,
            ANIMATION,
            ANIMATOR,
            OTHER
        }

        public enum COMPONENTLIFECYCLE {
            LIVE,
            HIDDEN,
            DEATH
        }

        public COMPONENTLIFECYCLE Statue;

        /// <summary>
        /// Current Component's Type 
        /// </summary>
        private SYNCTYPE componentType = SYNCTYPE.NULL;

        private uint ComponentID;

        protected BNetworkFrameBuffer NetworkFrameBuffer = new BNetworkFrameBuffer(1);

        protected BLocalFrameBuffer LocalFrameBuffer = new BLocalFrameBuffer(1);

        public BSyncComponentCore(uint componentID,SYNCTYPE type) {
            this.EnableSync();
            this.componentType = type;
            this.ComponentID = componentID;
        }

        public virtual SYNCTYPE GetComponentType()
        {
            return componentType;
        }

        public virtual void Update(float interpolationValue){ }

        public virtual BNOperateCommend UpdateLogic(int frameIndex)
        {
            ////TODO do some Logic thing
            return CreateCmdLogic(frameIndex);
        }

        private BNOperateCommend CreateCmdLogic(int frameIndex)
        {
            ////TODO PS: Warning ,there has a trap, you need to pay a attention
            BFrame<BFrameCommend>? frames = LocalFrameBuffer.DeQuene((uint)frameIndex);
            BNOperateCommend cmd = new BNOperateCommend();
            cmd.ComponentID = ComponentID;/////Wait to create new ID;
            cmd.OperateType = componentType;
            cmd.cmd         = frames?.Cmd; ////if you get cache much commdend ,theose code make you feel trouble;
            return cmd;
        }

        protected BFrameCommend GetNetworkCmd(uint frameIndex) {
            BFrame<BFrameCommend>?  frames = NetworkFrameBuffer.DeQuene(frameIndex);
            return frames?.Cmd;
        }

        public virtual bool UpdateByNet(uint NframeCount, BFrameCommend data)
        {
            ////TODO do some Update by net
            BFrame<BFrameCommend> Scmd = new BFrame<BFrameCommend>()
            {
                FrameNum = NframeCount,
                Cmd = data
            };
            NetworkFrameBuffer.EnQuene(Scmd);
            return false;
        }

        public virtual void SetComponentType(SYNCTYPE type) {componentType = type;}

        public virtual void TakeSnapshot(SnapshotWriter writer){}

        public virtual void RollbackTo(SnapshotReader reader){}

        public virtual void Dispose() { this.DisableSync();Statue = COMPONENTLIFECYCLE.DEATH;}

        public virtual bool FrameConfirm<T>(T data) where T : class
        {
            return false;
        }

        //public virtual bool FrameConfirm(BFrameCommend data) {return false;}

    }
}