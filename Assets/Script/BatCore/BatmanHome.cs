using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT
{

    /// <summary>
    /// 程序主入口
    /// </summary>
    public class BatmanHome : BatMonoSingletion<BatmanHome>
    {
        Hello hello;
        private void Awake()
        {
            hello = new Hello();
            LifeCycleControl.Instance().Awake(this);
        }

        private void FixedUpdate()
        {

        }

        private void LateUpdate()
        {

        }

        private void OnApplicationFocus(bool focus)
        {

        }

        private void OnApplicationPause(bool pause)
        {

        }

        private void OnApplicationQuit()
        {

        }

        private void OnDestroy()
        {

        }

        private void OnDisable()
        {

        }

        private void OnEnable()
        {

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
            if (Input.GetKeyDown(KeyCode.O)) {
                hello.OpenAble();
            }
        }
    }
}
