using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.SyncSystem.FrameSync.Buffer;
using THEDARKKNIGHT.SyncSystem.FrameSync.ExtendMethod;
using THEDARKKNIGHT.SyncSystem.FrameSync.Interface;
using THEDARKKNIGHT.SyncSystem.FrameSync.Struct;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync {

    public abstract class BSyncComponentCore : ISyncComponent
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

        public virtual void Update(float interpolationValue)
        { 
            ////TODO do some of Render
        }

        public virtual BNOperateCommend UpdateLogic(int frameCount)
        {
            ////TODO do some Logic thing
            return CreateCmdLogic(frameCount);
        }

        private BNOperateCommend CreateCmdLogic(int frameCount)
        {
            ////TODO PS: Warning ,there has a trap, you need to pay a attention
            BFrame<BFrameCommend>[] frames = LocalFrameBuffer.DeQuene(frameCount);
            BNOperateCommend cmd = new BNOperateCommend();
            cmd.ComponentID = ComponentID;/////Wait to create new ID;
            cmd.OperateType = componentType;
            cmd.cmd = frames[0].Cmd; ////if you get cache much commdend ,theose code make you feel trouble;
            return cmd;
        }

        public virtual void UpdateByNet(uint NframeCount, BFrameCommend data)
        {
            ////TODO do some Update by net
            BFrame<BFrameCommend> Scmd = new BFrame<BFrameCommend>()
            {
                FrameNum = NframeCount,
                Cmd = data
            };
            NetworkFrameBuffer.EnQuene(Scmd);
        }

        public virtual void SetComponentType(SYNCTYPE type) {
            componentType = type;
        }

    }


}
