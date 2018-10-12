using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.ConstDefine;
using THEDARKKNIGHT.EventDefine;
using THEDARKKNIGHT.EventSystem;
using THEDARKKNIGHT.InputOperate.DataStruct;
using THEDARKKNIGHT.ProcessCore;
using UnityEngine;
namespace THEDARKKNIGHT.Example
{
    public class ProcessPlayerThreeOfOneDemo : BProcessItem
    {
        public GameObject PlayerThree { get; private set; }


        public ProcessPlayerThreeOfOneDemo(string name)
        {
            this.TaskName = name;
        }

        // Use this for initialization
        public override void AssetCheck()
        {
            GameObject PlayerTwo_2 = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/PlayerThree_2") as GameObject;
            PlayerThree = GameObject.Instantiate(PlayerTwo_2);
            BEventManager.Instance().AddListener(BatEventDefine.LEFTPRESSEVENT, LeftPressCallback);
        }

        private object LeftPressCallback(object data)
        {
            InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;
            if (packet.Info.CastGameObject == PlayerThree) {
                ProcessFinish();
            }
            return null;
        }

        public override void FixedUpdate()
        {
        }

        public override void OnDestory()
        {
            GameObject.Destroy(PlayerThree);
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
    }
}
