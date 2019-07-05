using System.Collections.Generic;
using THEDARKKNIGHT.EventDefine;
using THEDARKKNIGHT.EventSystem;
using THEDARKKNIGHT.Interface;
using THEDARKKNIGHT.SyncSystem.FrameSync.Interface;
using UnityEngine;
using THEDARKKNIGHT.SyncSystem.FrameSync.Snapshot;
using THEDARKKNIGHT.SyncSystem.FrameSync.BStruct.NetworkProtocol;
using THEDARKKNIGHT.SyncSystem.FrameSync.Utility;

namespace THEDARKKNIGHT.SyncSystem.FrameSync
{

    public class BFrameSyncSystem : BFrameSyncCore,ILifeCycle
    {

        public static List<ISyncComponent> SyncObjectGroup = new List<ISyncComponent>();

        public TimeMachine TimeMachineInstance;

        public BFrameSyncSystem() {
            TimeMachineInstance = new TimeMachine();
            Init();
        }

        private void Init()
        {
            BEventManager.Instance().AddListener(BatEventDefine.UPDATEBYNETFRAME, NetworkLogicUpdate);
        }

        public override void FrameLockLogic(int frameConut)
        {
            LocalLogicUpdate(frameConut);////for local logic code
            TimeMachineInstance.TakeSnapshot();/////every frame save the component
        }

        private object NetworkLogicUpdate(object data)
        {
           
            BNFrameCommdend Ncmd = (BNFrameCommdend)data;
            uint frameIndex = (uint)Ncmd.NFrameNum;
            TimeMachineInstance.GetFrameSnapshot(frameIndex);///////Dispatch the snapshot to each component;
            for (int i = 0; i < SyncObjectGroup.Count; i++)
            {
                for (int j = 0; j < Ncmd.OperateCmd.Count;j++) {
                    if (SyncObjectGroup[i].GetComponentType() == Ncmd.OperateCmd[j].OperateType) {
                        if (SyncObjectGroup[i].UpdateByNet(frameIndex, Ncmd.OperateCmd[j].cmd)) {
                            TimeMachineInstance.RollbackTo(frameIndex);//////Rollback the Frame beacause one local frame can not match the frame from server
                            return null;
                        }
                    }
                }
            }

            ///////Network frame is no error ,so comfire this frame
            TimeMachineInstance.ConfiremedFrame((uint)Ncmd.NFrameNum);
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
            BNFrameCommdend frameCmd = new BNFrameCommdend();
            frameCmd.NFrameNum = frameConut;
            frameCmd.OperateCmd = new List<BNOperateCommend>();
            ///////TODO each Component's commend is collected ,and send it to Server 
            for (int i = 0; i < SyncObjectGroup.Count; i++)
            {
                BNOperateCommend operatedCmd = SyncObjectGroup[i].UpdateLogic(frameConut);
                if (operatedCmd != null)
                    frameCmd.OperateCmd.Add(operatedCmd);
            }
            SendCommendToServer(frameCmd);
        }

        /// <summary>
        /// Send it Msg to Server
        /// </summary>
        /// <param name="frameCmd"></param>
        private void SendCommendToServer(BNFrameCommdend frameCmd)
        {
            /////TODO Send it Msg to Server
            byte[] cmdBytes= BFrameSyncUtility.NSeralizableClassTobytes(frameCmd);
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

