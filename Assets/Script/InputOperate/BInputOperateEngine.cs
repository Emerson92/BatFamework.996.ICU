using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.InputOperate
{
    public class BInputOperateEngine :BatSingletion<BInputOperateEngine> ,ILifeCycle
    {

        private IInputParser InputParser;

        private BInputOperateEngine() {
            this.Enable();
        }

        public void BAwake(MonoBehaviour main)
        {
            LifeCycleTool tool = this.GetLifeCycleTool();
            tool.SetLifeCycle(LifeCycleTool.LifeType.Start,true)
                .SetLifeCycle(LifeCycleTool.LifeType.Update,true);
        }

        public void BDisable(MonoBehaviour main)
        {
        }

        public void BFixedUpdate(MonoBehaviour main)
        {
        }

        public void BLateUpdate(MonoBehaviour main)
        {
        }

        public void BOnApplicationFocus(MonoBehaviour main)
        {
        }

        public void BOnApplicationPause(MonoBehaviour main)
        {
        }

        public void BOnApplicationQuit(MonoBehaviour main)
        {
        }

        public void BOnDestory(MonoBehaviour main)
        {
        }

        public void BOnDestroy(MonoBehaviour main)
        {
        }

        public void BOnEnable(MonoBehaviour main)
        {
        }

        public void BStart(MonoBehaviour main)
        {
        }

        public void BUpdate(MonoBehaviour main)
        {
        }
    }
}

