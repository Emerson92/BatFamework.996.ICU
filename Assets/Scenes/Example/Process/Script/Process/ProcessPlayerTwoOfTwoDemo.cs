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


        public ProcessPlayerTwoOfTwoDemo(string name) {
            this.TaskName = name;
        }

        public override void AssetCheck()
        {
            GameObject playerTwo_2 = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/playerTwo_2") as GameObject;
            PlayerTwo = GameObject.Instantiate(playerTwo_2);
            BEventManager.Instance().AddListener(BatEventDefine.LEFTPRESSEVENT, LeftPressCallback);
        }

        private object LeftPressCallback(object data)
        {
            InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;

            if (packet.Info.CastGameObject == PlayerTwo) {
                PlayerTwo.transform.localScale *= 0.5f;
                mono.StartCoroutine(DelayToExcuteFinish());
            }
            return null;
        }

        IEnumerator  DelayToExcuteFinish()
        {
            yield return new WaitForSecondsRealtime(8f);
            ProcessFinish();
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

        public override void OnDestory()
        {
            GameObject.Destroy(PlayerTwo);
        }
    }

}
