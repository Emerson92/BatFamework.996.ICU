﻿using System;
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


        public override void AssetInit(object data)
        {
            GameObject PlayerThree_2 = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/PlayerThree_2") as GameObject;
            PlayerThree = GameObject.Instantiate(PlayerThree_2);
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