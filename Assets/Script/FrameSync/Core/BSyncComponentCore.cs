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
            ACTION,
            SOLIDER,
            SPHERE,
            PEOPLE,
            ANIMATION,
            CAT,
            OTHER
        }

        /// <summary>
        /// Current Component's Type 
        /// </summary>
        private SYNCTYPE componentType = SYNCTYPE.NULL;

        public BSyncComponentCore() {
            this.EnableSync();
        }

        protected BFrameBufferCore<BFrameCommend> networkFrameBuffer = new BFrameBufferCore<BFrameCommend>(1);

        public virtual SYNCTYPE GetComponentType()
        {
            return componentType;
        }

        public virtual void Update(float interpolationValue)
        { 
            ////TODO do some of Render
        }

        public virtual void UpdateLogic(int frameCount)
        {
           ////TODO do some Logic thing
        }

        public virtual void UpdateByNet(uint NframeCount, BFrameCommend data)
        {
            ////TODO do some Update by net
            BFrame<BFrameCommend> Severcmd = new BFrame<BFrameCommend>()
            {
                FrameNum = NframeCount,
                Cmd = data
            };
            networkFrameBuffer.EnQuene(Severcmd);
        }

        public virtual void SetComponentType(SYNCTYPE type) {
            componentType = type;
        }

    }


}
