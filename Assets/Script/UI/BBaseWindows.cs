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
        // Maybe we can use the hashTable to replace the Dictionary;
        protected Dictionary<string,BBaseWindows> SubWindows = new Dictionary<string, BBaseWindows>();

        LifeCycleTool tool;

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

        public BBaseWindows GetSubWindows(string windowsName){
            BBaseWindows subwindows = null;
            SubWindows.TryGetValue(windowsName,out subwindows);
            return subwindows;
        }

        public void BAwake(MonoBehaviour main)
        {
            Init();
            AddListener();
        }

        public void RegisterWindows(string subwindowsName,BBaseWindows windows) {
            try{
                SubWindows.Add(subwindowsName, windows);
            }catch(Exception e){
                RegisterErrorCallback(subwindowsName);
            }
         
        }

        public virtual void RegisterErrorCallback(string subwindowsName){}

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
            SubWindows.Clear();
        }

        public void BUpdate(MonoBehaviour main){}

        //TODO The if the subWindows need to Tell the parent wnidows,how to do that??

        //TODO Create the Windows Base Information


    }

}

