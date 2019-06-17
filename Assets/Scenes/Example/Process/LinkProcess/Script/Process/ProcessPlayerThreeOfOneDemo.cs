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
    public class ProcessPlayerThreeOfOneDemo : BProcessItem
    {

        public GameObject PlayerCalabash { get; private set; }

        private Transform Calabash;

        public GameObject PlayerAreaTwo { get; private set; }


        private Vector3 targetRotation = new Vector3 (0,0,0);
        private Vector3 targetScale = new Vector3(10,10,10);

        private bool IsTransform;
        private float Tick;

        public ProcessPlayerThreeOfOneDemo(string name) {
            this.TaskName = name;
        }

        public override void AssetInit(object data)
        {
            GameObject playerThree_1 = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/PlayerThree_1") as GameObject;
            PlayerCalabash = GameObject.Instantiate(playerThree_1);
            Calabash =  PlayerCalabash.transform.Find("Calabash");
            ReadyToExcute();
        }

        public override void DataInit(object data)
        {
            BEventManager.Instance().AddListener(BatEventDefine.LEFTPRESSEVENT, LeftPressCallback);
        }

        private object LeftPressCallback(object data)
        {
            InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;

            if (packet.Info.CastGameObject == PlayerCalabash)
            {
                IsTransform = true;
            }
            return null;
        }

        IEnumerator DelayToExcuteFinish()
        {
            yield return new WaitForSecondsRealtime(3f);
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
            if (IsTransform) {
                Tick +=Time.deltaTime;
                IsTransform = true;
                Calabash.rotation =Quaternion.Euler(Vector3.Lerp(Calabash.rotation.eulerAngles, targetRotation, Tick/5));
                Calabash.localScale = Vector3.Lerp(Calabash.localScale, targetScale, Tick / 5);
                if (Calabash.rotation == Quaternion.Euler(targetRotation) && Calabash.localScale == targetScale) {
                    IsTransform = false;
                    Calabash.gameObject.AddComponent<Rigidbody>();
                    mono.StartCoroutine(DelayToExcuteFinish());
                }
            }
        }

        public override void Destory()
        {
            BEventManager.Instance().RemoveListener(BatEventDefine.LEFTPRESSEVENT, LeftPressCallback);
            GameObject.Destroy(PlayerCalabash);
        }
    }

}
