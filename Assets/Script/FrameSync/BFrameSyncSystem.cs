using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.Interface;
using THEDARKKNIGHT.SyncSystem.FrameSync.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync {

    public class BFrameSyncSystem : BFrameSyncCore,ILifeCycle
    {

        public static List<ISyncComponent> SyncObjectGroup = new List<ISyncComponent>();


        public override void FrameLockLogic(int frameConut)
        {
            for (int i = 0;i < SyncObjectGroup.Count;i++) {
                SyncObjectGroup[i].UpdateLogic(frameConut);
            }
        }

        public override void UpdateRender(float interpolationValue)
        {
            for (int i = 0; i < SyncObjectGroup.Count; i++) {
                SyncObjectGroup[i].Update(interpolationValue);
            }
        }

        void ILifeCycle.BAwake(MonoBehaviour main){
            this.Enable().SetLifeCycle(LifeCycleTool.LifeType.Update,true);
        }
        void ILifeCycle.BDisable(MonoBehaviour main){}
        void ILifeCycle.BFixedUpdate(MonoBehaviour main){}
        void ILifeCycle.BLateUpdate(MonoBehaviour main){}
        void ILifeCycle.BOnApplicationFocus(MonoBehaviour main){}
        void ILifeCycle.BOnApplicationPause(MonoBehaviour main){}
        void ILifeCycle.BOnApplicationQuit(MonoBehaviour main){}
        void ILifeCycle.BOnDestroy(MonoBehaviour main){}
        void ILifeCycle.BOnEnable(MonoBehaviour main){}
        void ILifeCycle.BStart(MonoBehaviour main){}
        void ILifeCycle.BUpdate(MonoBehaviour main){
            UpdateLogic();
        }
    }

}

