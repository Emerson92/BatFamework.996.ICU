using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.SyncSystem.FrameSync.ExtendMethod;
using THEDARKKNIGHT.SyncSystem.FrameSync.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync {

    public abstract class BSyncItemCore : ISyncComponent
    {

        public BSyncItemCore() {
            this.EnableSync();
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
