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
        public override void AssetInit(object data)
        {
            GameObject PlayerThree_1 = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/PlayerThree_1") as GameObject;
            PlayerThree = GameObject.Instantiate(PlayerThree_1);
            ReadyToExcute();
        }

        public override void DataInit(object data)
        {
            BEventManager.Instance().AddListener(BatEventDefine.LEFTPRESSEVENT, LeftPressCallback);
        }

        private object LeftPressCallback(object data)
        {
            InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;
            if (packet.Info.CastGameObject == PlayerThree) {
                ForceToFinishProcess();
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
