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
    public class ProcessPlayerThreeOfTwoDemo : BProcessItem
    {
        public GameObject PlayThree_2 { get; private set; }

        GameObject PlayCylinder;

        GameObject PlaySphere;

        public ProcessPlayerThreeOfTwoDemo(string name) {
            this.TaskName = name;
        }

        public override void AssetInit(object data)
        {
            GameObject playerThree_2 = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/PlayerThree_2") as GameObject;
            PlayThree_2 = GameObject.Instantiate(playerThree_2);
            ReadyToExcute();
        }

        public override void DataInit(object data)
        {
            BEventManager.Instance().AddListener(BatEventDefine.LEFTPRESSEVENT, LeftPressCallback);
        }

        private object LeftPressCallback(object data)
        {
            InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;

            if (packet.Info.CastGameObject == PlayThree_2) {
                PlayThree_2.GetComponent<Rotate>().IsNeedRotate = true;
                mono.StartCoroutine(DelayToExcuteFinish(null));
            }
            return null;
        }

        IEnumerator DelayToExcuteFinish(string branch)
        {
            yield return new WaitForSecondsRealtime(8f);
            ProcessFinish(branch);
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
            GameObject.Destroy(PlayCylinder);
            GameObject.Destroy(PlaySphere);
        }
    }

}
