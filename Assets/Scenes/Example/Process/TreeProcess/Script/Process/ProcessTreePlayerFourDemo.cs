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
    public class ProcessTreePlayerFourDemo : BProcessItem
    {
        public GameObject PlayerFour { get; private set; }

        public Vector3 TargetScale = new Vector3(2,2,2);
        public bool StartToExcute = false;
        private float Timer = 0;

        public ProcessTreePlayerFourDemo(string name)
        {
            this.TaskName = name;
        }

        public override void AssetInit(object data)
        {
            GameObject playerFour = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/PlayerFour") as GameObject;
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
                StartToExcute = true;
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
            if (StartToExcute)
            {
                Timer += Time.deltaTime;
                PlayerFour.transform.localScale = Vector3.Lerp(PlayerFour.transform.localScale, TargetScale, Timer / 5);
                if (PlayerFour.transform.localScale == TargetScale)
                {
                    StartToExcute = false;
                    Timer = 0;
                    ProcessFinish("PlayerSixOneUnit");
                }
            }
        }
    }

}

