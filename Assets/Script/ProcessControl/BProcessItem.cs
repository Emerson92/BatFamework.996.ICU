using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.ProcessCore
{
    public abstract class BProcessItem :ILifeCycle
    {

        public enum PROCESSSTATUS {
            None,
            Ready,
            Excuting,
            Finish
        }

        private PROCESSSTATUS Status = PROCESSSTATUS.None;

        protected LifeCycleTool tool;

        public PROCESSSTATUS ProcessStatus {
            set {
                Status = value;
                if (value == PROCESSSTATUS.Finish)
                {
                    FinishCallback();
                    StopExcute();
                }
            }
            get {
                return Status;
            }
        }

        private void FinishCallback()
        {
            if (ProcessItemFinishExcution != null)
                ProcessItemFinishExcution();
        }

        public string TaskName;

        public MonoBehaviour mono;

        public Action ProcessItemFinishExcution;

        public Action ProcessItemAssetAlready;

        public abstract void AssetCheck();

        public abstract void ProcessExcute();

        public abstract void StopExcute();

        public abstract void Update();

        public abstract void FixedUpdate();

        public abstract void OnDestory();


        public virtual void ProcessFinish() {
            Status = PROCESSSTATUS.Finish;
            FinishCallback();
        }

        public virtual void Init() {
            this.Enable();
            tool = this.GetLifeCycleTool();
            tool.SetLifeCycle(LifeCycleTool.LifeType.Update, true);
            tool.SetLifeCycle(LifeCycleTool.LifeType.FixedUpdate, true);
        }

        public void BAwake(MonoBehaviour main){ mono = main; }

        public void BStart(MonoBehaviour main){}

        public void BOnEnable(MonoBehaviour main){}

        public void BDisable(MonoBehaviour main){}

        public void BOnDestory(MonoBehaviour main){}

        public void BFixedUpdate(MonoBehaviour main){ FixedUpdate(); }

        public void BLateUpdate(MonoBehaviour main){}

        public void BOnApplicationFocus(MonoBehaviour main){}

        public void BOnApplicationPause(MonoBehaviour main){}

        public void BOnApplicationQuit(MonoBehaviour main){}

        public void BOnDestroy(MonoBehaviour main){}

        public void BUpdate(MonoBehaviour main){ Update(); }
    }
}
