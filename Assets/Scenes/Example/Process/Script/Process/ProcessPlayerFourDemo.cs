using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.ConstDefine;
using THEDARKKNIGHT.ProcessCore;
using UnityEngine;
namespace THEDARKKNIGHT.Example {

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

        }

        public override void Destory()
        {

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

