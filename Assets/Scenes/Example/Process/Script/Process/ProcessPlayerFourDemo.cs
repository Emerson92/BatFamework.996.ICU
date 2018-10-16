using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.ConstDefine;
using THEDARKKNIGHT.EventDefine;
using THEDARKKNIGHT.EventSystem;
using THEDARKKNIGHT.InputOperate.DataStruct;
using THEDARKKNIGHT.ProcessCore;
using UnityEngine;
namespace THEDARKKNIGHT.Example {
    [Serializable]
    public class ProcessPlayerFourDemo : BProcessItem
    {
        public GameObject PlayerFour { get; private set; }

        public ProcessPlayerFourDemo(string name)
        {
            this.TaskName = name;
        }

        public override void AssetInit(object data)
        {
            GameObject playerFour = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/Lollipop") as GameObject;
            PlayerFour = GameObject.Instantiate(playerFour);
            ReadyToExcute();
        }

        public override void DataInit(object data)
        {
            BEventManager.Instance().AddListener(BatEventDefine.LEFTPRESSEVENT, LeftPressCallback);
        }

        private object LeftPressCallback(object data)
        {
            InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;
            if (packet.Info.CastGameObject == PlayerFour)
            {
                ProcessFinish();
            }
            return null;
        }

        public override void Destory()
        {
            BEventManager.Instance().RemoveListener(BatEventDefine.LEFTPRESSEVENT, LeftPressCallback);
            GameObject.Destroy(PlayerFour);
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
    }

}

