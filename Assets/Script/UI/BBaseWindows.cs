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
        /// <summary>
        /// The parent windos.
        /// </summary>
        protected BBaseWindows ParentWindows;

        protected string WindowsAlias { set; get; }

        /// <summary>
        /// Gets or sets the windows identifier.
        /// </summary>
        /// <value>The windows identifier.</value>
        /// 
        protected readonly int WindowsID;

        // Maybe we can use the hashTable to replace the Dictionary;
        protected Dictionary<string, BBaseWindows> SubWindows = new Dictionary<string, BBaseWindows>();

        LifeCycleTool tool;

        public bool WindowsStatus { set; get; }

        public abstract void Init(MonoBehaviour main);

        public abstract void AddListener();

        public abstract void RemoveListener();

        public abstract void DestoryWindows();

        public abstract void Show();

        public abstract void Hide();

        public virtual void Awake(MonoBehaviour main) { }

        public virtual void Start(MonoBehaviour main) { }

        public virtual void OnEnable(MonoBehaviour main) { }

        public virtual void Disable(MonoBehaviour main) { }

        public virtual void OnDestory(MonoBehaviour main) { }

        public virtual void FixedUpdate(MonoBehaviour main) { }

        public virtual void LateUpdate(MonoBehaviour main) { }

        public virtual void OnApplicationFocus(MonoBehaviour main) { }

        public virtual void OnApplicationPause(MonoBehaviour main) { }

        public virtual void OnApplicationQuit(MonoBehaviour main) { }

        public virtual void OnDestroy(MonoBehaviour main){}

        public virtual void Update(MonoBehaviour main) { }

        /// <summary>
        /// Gets the parent windows message.
        /// </summary>
        /// <returns>
        /// <c>true</c> this Msg will pass to SubWindows 
        /// <c>false</c> this msg will not pass to the SubWindows
        /// </returns>
        /// <param name="windowsID">Windows identifier.</param>
        /// <param name="data">Data.</param>
        public abstract bool GetWindowsMsg(int windowsID, string windowsAlias, object data);

        public BBaseWindows()
        {
            WindowsID = GetHashCode();
            this.Enable();
            tool = this.GetLifeCycleTool();
            SetLifeCycleType(LifeCycleTool.LifeType.OnDestroy, true);
        }

        public void SetLifeCycleType(LifeCycleTool.LifeType type, bool statue)
        {
            tool.SetLifeCycle(type, statue);
        }

        public BBaseWindows GetSubWindows(string windowsName)
        {
            BBaseWindows subwindows = null;
            SubWindows.TryGetValue(windowsName, out subwindows);
            return subwindows;
        }

        public void BAwake(MonoBehaviour main)
        {
            Awake(main);
            Init(main);
        }
        /// <summary>
        /// Registers the SubWindows,It is a very Important way.
        /// </summary>
        /// <param name="subwindowsName">Subwindows name.</param>
        /// <param name="windows">Windows.</param>
        public void RegisterWindows(string subwindowsName, BBaseWindows windows)
        {
            try
            {
                windows.ParentWindows = this;
                SubWindows.Add(subwindowsName, windows);
            }
            catch
            {
                RegisterErrorCallback("this Windows " + subwindowsName + " already register!!");
            }

        }

        public virtual void RegisterErrorCallback(string subwindowsName) { }

        public void BStart(MonoBehaviour main) { Start(main); }

        public void BOnEnable(MonoBehaviour main) { OnEnable(main); }

        public void BDisable(MonoBehaviour main) { Disable(main); }

        public void BOnDestory(MonoBehaviour main) { OnDestory( main); }

        public void BFixedUpdate(MonoBehaviour main) { FixedUpdate(main); }

        public void BLateUpdate(MonoBehaviour main) { LateUpdate(main); }

        public void BOnApplicationFocus(MonoBehaviour main) { OnApplicationFocus(main); }

        public void BOnApplicationPause(MonoBehaviour main) { OnApplicationPause(main); }

        public void BOnApplicationQuit(MonoBehaviour main) { OnApplicationQuit(main); }

        public void BOnDestroy(MonoBehaviour main)
        {
            OnDestroy(main);
            DestoryWindows();
            SubWindows.Clear();
        }

        public void BUpdate(MonoBehaviour main) { Update(main); }

        protected void DispatchMsg(int windowsID, string windowsAlias, object data)
        {
            if (GetWindowsMsg(windowsID, windowsAlias, data))
            {
                SendMsgToAllSubWindows(data);
            }
        }

        /// <summary>
        /// Sends the message to all sub windows.
        /// </summary>
        /// <param name="data">Data.</param>
        protected void SendMsgToAllSubWindows(object data)
        {
            foreach (KeyValuePair<string, BBaseWindows> item in SubWindows)
            {
                item.Value.DispatchMsg(WindowsID, WindowsAlias, data);
            }
        }

        /// <summary>
        /// feedback the message to parent windows.
        /// </summary>
        /// <param name="data">Data.</param>
        protected void SendMsgToParentWindows(object data)
        {
            if (ParentWindows != null)
            {
                ParentWindows.GetWindowsMsg(WindowsID, WindowsAlias, data);
            }
        }
        /// <summary>
        /// Broadcast the message to windows which has the same AliaName.
        /// it will cost much time to broadcast msg, i advice you that do not use this mehtod
        /// frequently
        /// </summary>
        /// <param name="windowsAlias">Windows alias.</param>
        /// <param name="data">Data.</param>
        protected void PostMsgToWindows(string windowsAlias, object data)
        {
            FindWindowsDownToSend(WindowsID, windowsAlias, data);
            FindWindowUpToSend(WindowsID, WindowsID, windowsAlias, data);
        }

        public void FindWindowUpToSend(int passWindowsID,int targetSourceID,string windowsAlias, object data)
        {
            if (ParentWindows != null)
            {
                if (ParentWindows.WindowsAlias == windowsAlias)
                {
                    ParentWindows.GetWindowsMsg(targetSourceID, windowsAlias, data);
                }
                foreach (KeyValuePair<string, BBaseWindows> item in ParentWindows.SubWindows)
                {
                    if (item.Value.WindowsID != passWindowsID)
                    {
                        if (item.Value.WindowsAlias == windowsAlias)
                            item.Value.GetWindowsMsg(targetSourceID, windowsAlias, data);
                        item.Value.FindWindowsDownToSend(targetSourceID, windowsAlias, data);
                    }
                }
                if (ParentWindows.ParentWindows != null)
                    ParentWindows.ParentWindows.FindWindowUpToSend(ParentWindows.WindowsID, targetSourceID, windowsAlias, data);
            }
        }

        public void FindWindowsDownToSend(int windowsID,string windowsAlias, object data)
        {
            foreach (KeyValuePair<string, BBaseWindows> item in SubWindows)
            {
                if (item.Value.WindowsAlias == windowsAlias)
                {
                    item.Value.GetWindowsMsg(windowsID, windowsAlias, data);
                }
                item.Value.FindWindowsDownToSend(windowsID, windowsAlias, data);
            }

        }
    }

}

