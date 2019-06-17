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
    public class ProcessTreePlayerSixDemo : BProcessItem
    {
        public GameObject PlayerSix { get; private set; }

        public ProcessTreePlayerSixDemo(string name)
        {
            this.TaskName = name;
        }

        // Use this for initialization
        public override void AssetInit(object data)
        {
            GameObject playSix = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/PlaySix") as GameObject;
            PlayerSix = GameObject.Instantiate(playSix);
            ReadyToExcute();
        }

        public override void DataInit(object data)
        {
            BEventManager.Instance().AddListener(BatEventDefine.LEFTPRESSEVENT, LeftPressCallback);
        }

        private object LeftPressCallback(object data)
        {
            InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;
            if (packet.Info.CastGameObject == PlayerSix)
            {
                ProcessFinish();
            }
            return null;
        }

        public override void FixedUpdate()
        {

        }

        public override void Destory()
        {
            BEventManager.Instance().RemoveListener(BatEventDefine.LEFTPRESSEVENT, LeftPressCallback);
            GameObject.Destroy(PlayerSix);
            PlayerSix = null;
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
