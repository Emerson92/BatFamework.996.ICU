using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.EventDefine;
using THEDARKKNIGHT.EventSystem;
using THEDARKKNIGHT.Interface;
using THEDARKKNIGHT.SyncSystem.FrameSync.Buffer;
using THEDARKKNIGHT.SyncSystem.FrameSync.Interface;
using THEDARKKNIGHT.SyncSystem.FrameSync.Struct;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync {

    public class BFrameSyncSystem : BFrameSyncCore,ILifeCycle
    {

        public static List<ISyncComponent> SyncObjectGroup = new List<ISyncComponent>();

        //private BFrameBufferCore<BFrameCommend> networkFrameBuffer = new BFrameBufferCore<BFrameCommend>(1);

        public BFrameSyncSystem() {
            Init();
        }

        private void Init()
        {
            BEventManager.Instance().AddListener(BatEventDefine.UPDATEBYNETFRAME, NetworkLogicUpdate);
        }

        public override void FrameLockLogic(int frameConut)
        {
            LocalLogicUpdate(frameConut);////for local logic code 
        }

        private object NetworkLogicUpdate(object data)
        {
            BNFrameCommdend Ncmd = (BNFrameCommdend)data;
            for (int i = 0; i < SyncObjectGroup.Count; i++)
            {
                for (int j = 0; j < Ncmd.OperateCmd.Count;j++) {
                    if (SyncObjectGroup[i].GetComponentType() == Ncmd.OperateCmd[j].OperateType) {
                        SyncObjectGroup[i].UpdateByNet(Ncmd.NFrameNum, Ncmd.OperateCmd[j].cmd);
                    }
                }
            }
            return null;
        }

        public override void UpdateRender(float interpolationValue)
        {
            for (int i = 0; i < SyncObjectGroup.Count; i++) {
                SyncObjectGroup[i].Update(interpolationValue);
            }
        }

        /// <summary>
        /// Locals the logic update.
        /// </summary>
        /// <param name="frameConut">Frame conut.</param>
        void LocalLogicUpdate(int frameConut)
        {
            for (int i = 0; i < SyncObjectGroup.Count; i++)
            {
                SyncObjectGroup[i].UpdateLogic(frameConut);
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

