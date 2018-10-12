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
    public class ProcessPlayerThreeOfTwoDemo : BProcessItem
    {
        public GameObject PlayerThree { get; private set; }

        public ProcessPlayerThreeOfTwoDemo(string name)
        {
            this.TaskName = name;
        }


        public override void AssetCheck()
        {
            GameObject PlayerTwo_1 = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/PlayerThree_1") as GameObject;
            PlayerThree = GameObject.Instantiate(PlayerTwo_1);
            BEventManager.Instance().AddListener(BatEventDefine.LEFTPRESSEVENT, LeftPressCallback);
        }

        private object LeftPressCallback(object data)
        {
            InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;
            if (packet.Info.CastGameObject == PlayerThree)
            {
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
