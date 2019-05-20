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
    public class ProcessPlayerTwoDemo : BProcessItem
    {
        public GameObject PlayerTwo { get; private set; }
        GameObject PlayCylinder;

        GameObject PlaySphere;

        public ProcessPlayerTwoDemo(string name) {
            this.TaskName = name;
        }

        public override void AssetInit(object data)
        {
            GameObject playerTwo = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/PlayerTwo") as GameObject;
            PlayerTwo = GameObject.Instantiate(playerTwo);
            ReadyToExcute();
        }

        public override void DataInit(object data)
        {
            PlayCylinder = PlayerTwo.transform.Find("playerTwo_2").gameObject;
            PlaySphere = PlayerTwo.transform.Find("playerTwo_3").gameObject;
            BEventManager.Instance().AddListener(BatEventDefine.LEFTPRESSEVENT, PlayerTwoLeftPressCallback);
        }

        private object PlayerTwoLeftPressCallback(object data)
        {
            InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;

            if (packet.Info.CastGameObject == PlayCylinder)
            {
                PlayCylinder.GetComponent<Rotate>().IsNeedRotate = true;
                mono.StartCoroutine(DelayToExcuteFinish("1"));
            }
            else if (packet.Info.CastGameObject == PlaySphere)
            {
                PlaySphere.AddComponent<Rigidbody>();
                mono.StartCoroutine(DelayToExcuteFinish("2"));
            }
            return null;
        }


        IEnumerator DelayToExcuteFinish(string branch)
        {
            yield return new WaitForSecondsRealtime(5f);
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
            //if(StartToExcute){
            //    Timer += Time.deltaTime;
            //    PlayerTwo.transform.localScale = Vector3.Lerp(PlayerTwo.transform.localScale, TargetScale, Timer/ 5);
            //    if(PlayerTwo.transform.localScale == TargetScale){
            //        StartToExcute = false;
            //        Timer = 0;
            //        ProcessFinish();
            //    }
            //}
           
        }

        public override void Destory()
        {
            BEventManager.Instance().RemoveListener(BatEventDefine.LEFTPRESSEVENT, PlayerTwoLeftPressCallback);
            GameObject.Destroy(PlayerTwo);
        }
    }

}
