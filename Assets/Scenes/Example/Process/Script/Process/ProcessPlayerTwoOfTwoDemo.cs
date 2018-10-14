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
    public class ProcessPlayerTwoOfTwoDemo : BProcessItem
    {
        public GameObject PlayerTwo { get; private set; }

        private Vector3 TargetScale;
        private float Timer = 0;
        private bool StartToExcute = false;

        public ProcessPlayerTwoOfTwoDemo(string name) {
            this.TaskName = name;
        }

        public override void AssetInit(object data)
        {
            GameObject playerTwo_2 = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/playerTwo_2") as GameObject;
            PlayerTwo = GameObject.Instantiate(playerTwo_2);
            ReadyToExcute();
        }

        public override void DataInit(object data)
        {
            BEventManager.Instance().AddListener(BatEventDefine.LEFTPRESSEVENT, LeftPressCallback);
        }

        private object LeftPressCallback(object data)
        {
            InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;

            if (packet.Info.CastGameObject == PlayerTwo) {
                TargetScale = PlayerTwo.transform.localScale * 0.5f;
                StartToExcute = true;
            }
            return null;
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
            if(StartToExcute){
                Timer += Time.deltaTime;
                PlayerTwo.transform.localScale = Vector3.Lerp(PlayerTwo.transform.localScale, TargetScale, Timer/ 5);
                if(PlayerTwo.transform.localScale == TargetScale){
                    StartToExcute = false;
                    Timer = 0;
                    ProcessFinish();
                }
            }
           
        }

        public override void OnDestory()
        {
            GameObject.Destroy(PlayerTwo);
        }
    }

}
