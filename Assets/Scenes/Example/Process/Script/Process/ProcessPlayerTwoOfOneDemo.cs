using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.ConstDefine;
using THEDARKKNIGHT.EventDefine;
using THEDARKKNIGHT.EventSystem;
using THEDARKKNIGHT.InputOperate;
using THEDARKKNIGHT.InputOperate.DataStruct;
using THEDARKKNIGHT.ProcessCore;
using UnityEngine;
namespace THEDARKKNIGHT.Example
{
    public class ProcessPlayerTwoOfOneDemo : BProcessItem
    {
        public GameObject PlayerOne { get; private set; }

        public ProcessPlayerTwoOfOneDemo(string name) {
            this.TaskName = name;
        }

        public override void AssetInit(object data)
        {
            GameObject PlayerTwo_1 = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/PlayerTwo_1") as GameObject;
            PlayerOne = GameObject.Instantiate(PlayerTwo_1);
            ReadyToExcute();
        }

        public override void DataInit(object data)
        {
            BEventManager.Instance().AddListener(BatEventDefine.LEFTPRESSEVENT, LeftPressCallback);
        }

        private object LeftPressCallback(object data)
        {
            InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;

            if (packet.Info.CastGameObject == PlayerOne) {
                PlayerOne.GetComponent<Rotate>().IsNeedRotate = true;
                mono.StartCoroutine(DelayToExcuteFinish());
            } 
            return null;
        }

        IEnumerator DelayToExcuteFinish() {
            yield return new WaitForSecondsRealtime(8f);
            ProcessFinish();
        }

        public override void FixedUpdate()
        {

        }

        public override void ProcessExcute()
        {

        }

        public override void StopExcute()
        {

        }

        public override void Update()
        {

        }

        public override void OnDestory()
        {
            GameObject.Destroy(PlayerOne);
        }
    }

}
