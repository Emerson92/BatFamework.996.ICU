using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.SyncSystem.FrameSync.Buffer;
using THEDARKKNIGHT.SyncSystem.FrameSync.ExtendMethod;
using THEDARKKNIGHT.SyncSystem.FrameSync.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync {

    public abstract class BSyncComponentCore : ISyncComponent
    {

        public BSyncComponentCore() {
            this.EnableSync();
        }

        public virtual void networkLogicUpdate<T>(BFrame<T>[] data) where T : class
        {
            /////TODO network logic update
        }

        public virtual void Update(float interpolationValue)
        { 
            ////TODO do some of Render
            
        }

        public virtual void UpdateLogic(int frameCount)
        {
           ////TODO do some Logic thing
           
        }
    }


}
