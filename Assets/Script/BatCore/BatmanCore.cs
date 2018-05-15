using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.EventSystem;
using THEDARKKNIGHT.TcpSocket;
using UnityEngine;
namespace THEDARKKNIGHT
{

    /// <summary>
    /// 程序主入口
    /// </summary>
    public class BatmanCore : BatMonoSingletion<BatmanCore>
    {
        Hello hello;
        TcpSocketClientMgr tcp;
        public void Awake()
        {
            CodeWatcher.Instance().Init();
            LifeCycleControl.Instance().Awake(this);
            TestCode();
        }

        private void TestCode()
        {
            EventManager.Instance().AddListener("Test", Test1, "0", 0);
            EventManager.Instance().AddListener("Test", Test1, "2", 2);
            EventManager.Instance().AddListener("Test", Test1, "1", 1);
            EventManager.Instance().AddListener("Test", Test1, "8", 8);
            EventManager.Instance().AddListener("Test", Test1, "3", 3);
            EventManager.Instance().AddListener("Test", Test1, "4", 4);
            EventManager.Instance().AddListener("Test", Test1, "10", 10);
            EventManager.Instance().AddListener("Test", Test1, "5", 5);
            EventManager.Instance().AddListener("Test", Test1, "7", 7);
            EventManager.Instance().AddListener("Test", Test1, "9", 9);
            EventManager.Instance().DispatchEvent("Test",(object num) => {
                Debug.Log("Callback:"+ num);
            });
        }

        private object Test1(object num)
        {
            Debug.Log("Test1:"+ num);
            return num;
        }

        public void FixedUpdate()
        {
            LifeCycleControl.Instance().FixedUpdate(this);
        }

        public void LateUpdate()
        {
            LifeCycleControl.Instance().LateUpdate(this);
        }

        public void OnApplicationFocus(bool focus)
        {
            LifeCycleControl.Instance().OnApplicationFocus(this);
        }

        public void OnApplicationPause(bool pause)
        {
            LifeCycleControl.Instance().BOnApplicationPause(this);
        }

        public void OnApplicationQuit()
        {
            LifeCycleControl.Instance().OnApplicationQuit(this);
        }

        public override void OnDestroy()
        {
            LifeCycleControl.Instance().OnDestory(this);
        }

        public void OnDisable()
        {
            LifeCycleControl.Instance().OnDisable(this);
        }

        public void OnEnable()
        {
            LifeCycleControl.Instance().OnEnable(this);
        }

        // Use this for initialization
        void Start()
        {
            LifeCycleControl.Instance().Start(this);
        }

        // Update is called once per frame
        void Update()
        {
            LifeCycleControl.Instance().Update(this);
        }
    }
}
