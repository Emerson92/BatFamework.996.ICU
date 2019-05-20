using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.ProcessCore
{
    [Serializable]
    public abstract class BProcessItem : ILifeCycle
    {

        public enum PROCESSSTATUS {
            None,
            Start,
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

        private IEnumerator FinishCallback(string branchName = null,bool forceQuit = false)
        {
            yield return new WaitForEndOfFrame();
            if (ProcessItemFinishExcution != null)
                ProcessItemFinishExcution(branchName,ProcessData, forceQuit);
        }

        protected void ForceToFinishProcess(string branchName = null){
            mono.StartCoroutine(FinishCallback(branchName,true));
        }

        public object ProcessData { set; get; }

        public string TaskName;

        public MonoBehaviour mono { private set;get;}


        public Action<string,object,bool> ProcessItemFinishExcution;

        public Action<BProcessItem,object> ProcessItemAssetAlready;

        public abstract void AssetInit(object data);

        public abstract void DataInit(object data);

        public abstract void ProcessExcute();

        public abstract void StopExcute();

        public abstract void Update();

        public abstract void FixedUpdate();

        public abstract void Destory();

        public virtual void OnDestroy() { }

        public virtual void OnApplicationQuit() { }

        public virtual void ProcessFinish(string branch = null) {
            Debug.Log(this.TaskName + " ProcessFinish ");
            Status = PROCESSSTATUS.Finish;
            mono.StartCoroutine(FinishCallback(branch));
        }

        public virtual void Init() {
            this.Enable();
            tool = this.GetLifeCycleTool();
            tool.SetLifeCycle(LifeCycleTool.LifeType.Update, true);
            tool.SetLifeCycle(LifeCycleTool.LifeType.FixedUpdate, true);
            tool.SetLifeCycle(LifeCycleTool.LifeType.OnDestroy, true);
            tool.SetLifeCycle(LifeCycleTool.LifeType.OnApplicationQuit,true);
        }

        public void ProcessStart(object data)
        {
            ProcessStatus = PROCESSSTATUS.Start;
            this.ProcessData = data;
            AssetInit(ProcessData);
        }

        public void ReadyToExcute(){
            ProcessStatus = PROCESSSTATUS.Ready;
            if (ProcessItemAssetAlready !=null)
                ProcessItemAssetAlready(this, ProcessData);
            ProcessItemAssetAlready = null;
        }

        public void ProcessDestory() {
            Debug.Log("ProcessDestory :"+ TaskName);
            this.Disable();
            Destory();
            ProcessData = null;
        }

        public void BAwake(MonoBehaviour main){ mono = main; }

        public void BStart(MonoBehaviour main){}

        public void BOnEnable(MonoBehaviour main){}

        public void BDisable(MonoBehaviour main){}

        public void BOnDestory(MonoBehaviour main){ OnDestroy(); }

        public void BFixedUpdate(MonoBehaviour main){ FixedUpdate(); }

        public void BLateUpdate(MonoBehaviour main){}

        public void BOnApplicationFocus(MonoBehaviour main){}

        public void BOnApplicationPause(MonoBehaviour main){}

        public void BOnApplicationQuit(MonoBehaviour main){ OnApplicationQuit(); }

        public void BOnDestroy(MonoBehaviour main){}

        public void BUpdate(MonoBehaviour main){ Update(); }
    }
}
