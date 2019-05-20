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
    public class ProcessPlayerFiveDemo : BProcessItem
    {
        public GameObject PlayerFive { get; private set; }

        public ProcessPlayerFiveDemo(string name)
        {
            this.TaskName = name;
        }


        public override void AssetInit(object data)
        {
            GameObject playerFive = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/PlayerFive") as GameObject;
            PlayerFive = GameObject.Instantiate(playerFive);
            ReadyToExcute();
        }

        public override void DataInit(object data)
        {
            BEventManager.Instance().AddListener(BatEventDefine.LEFTPRESSEVENT, LeftPressCallback);
        }

        private object LeftPressCallback(object data)
        {
            InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;
            if (packet.Info.CastGameObject == PlayerFive)
            {
                ProcessData = "B";
                ForceToFinishProcess();
            }
            return null;
        }

        public override void FixedUpdate()
        {
        }

        public override void Destory()
        {

            BEventManager.Instance().RemoveListener(BatEventDefine.LEFTPRESSEVENT, LeftPressCallback);
            GameObject.Destroy(PlayerFive);
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
