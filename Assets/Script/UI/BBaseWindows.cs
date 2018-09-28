using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.UI {
    /// <summary>
    ///                           BaseWindos 
    ///   It is the class which contain base function about UI windos Control
    ///   All the UI componet should to inherit this class
    /// </summary>
    public abstract class BBaseWindows : ILifeCycle
    {

        protected List<BBaseWindows> WindowsList = new List<BBaseWindows>();

        private LifeCycleTool tool;

        public bool WindowsStatus { set; get; }

        public abstract void Init();

        public abstract void AddListener();

        public abstract void RemoveListener();

        public abstract void DestoryWindows();

        public abstract void Show();

        public abstract void Hide();

        private BBaseWindows() {
            this.Enable();
            tool = this.GetLifeCycleTool();
            SetLifeCycleType(LifeCycleTool.LifeType.OnDestroy,true);
        }

        public void SetLifeCycleType(LifeCycleTool.LifeType type,bool statue) {
            tool.SetLifeCycle(type, statue);
        }

        public void BAwake(MonoBehaviour main)
        {
            Init();
            AddListener();
        }

        public void RegisterWindows(BBaseWindows windows) {
            WindowsList.Add(windows);
        }

        public void BStart(MonoBehaviour main){}

        public void BOnEnable(MonoBehaviour main){}

        public void BDisable(MonoBehaviour main){}

        public void BOnDestory(MonoBehaviour main){}

        public void BFixedUpdate(MonoBehaviour main){}

        public void BLateUpdate(MonoBehaviour main){}

        public void BOnApplicationFocus(MonoBehaviour main){}

        public void BOnApplicationPause(MonoBehaviour main){}

        public void BOnApplicationQuit(MonoBehaviour main){}

        public void BOnDestroy(MonoBehaviour main)
        {
            DestoryWindows();
            WindowsList.Clear();
        }

        public void BUpdate(MonoBehaviour main){}
    }

}

