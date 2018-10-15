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
    public class ProcessPlayerTwoOfThreeDemo : BProcessItem
    {

        public GameObject PlayerThree { get; private set; }
        public GameObject PlayerAreaTwo { get; private set; }


        public ProcessPlayerTwoOfThreeDemo(string name) {
            this.TaskName = name;
        }

        public override void AssetInit(object data)
        {
            GameObject playerTwo_3 = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/playerTwo_3") as GameObject;
            PlayerThree = GameObject.Instantiate(playerTwo_3);
            ReadyToExcute();
        }

        public override void DataInit(object data)
        {
            BEventManager.Instance().AddListener(BatEventDefine.LEFTPRESSEVENT, LeftPressCallback);
        }

        private object LeftPressCallback(object data)
        {
            InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;

            if (packet.Info.CastGameObject == PlayerThree)
            {
                PlayerThree.AddComponent<Rigidbody>();
                ProcessFinish();
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

        }

        public override void Destory()
        {
            BEventManager.Instance().RemoveListener(BatEventDefine.LEFTPRESSEVENT, LeftPressCallback);
            GameObject.Destroy(PlayerThree);
        }
    }

}
