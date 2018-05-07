using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT
{

    /// <summary>
    /// 程序主入口
    /// </summary>
    public class BatmanCore : BatMonoSingletion<BatmanCore>
    {
        Hello hello;
        public void Awake()
        {
            CodeWatcher.Instance().Init();
            LifeCycleControl.Instance().Awake(this);
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
