using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using THEDARKKNIGHT.ConstDefine;
using THEDARKKNIGHT.EventDefine;
using THEDARKKNIGHT.EventSystem;
using THEDARKKNIGHT.InputOperate;
using THEDARKKNIGHT.InputOperate.DataStruct;
using THEDARKKNIGHT.ProcessCore;
using UnityEngine;
namespace THEDARKKNIGHT.Example
{
    public class ProcessTreePlayerTwoDemo : BProcessItem
    {
        public GameObject PlayerTwo { get; private set; }
        public bool StartToExcute { get; private set; }

        public Vector3 TargetScale = new Vector3(0.25f,2,0.25f);

        GameObject PlayCylinder;

        GameObject PlaySphere;

        private float tick = 0;

        public ProcessTreePlayerTwoDemo(string name) {
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
                StartToExcute = true;
            }
            else if (packet.Info.CastGameObject == PlaySphere)
            {
                PlaySphere.AddComponent<Rigidbody>();
                mono.StartCoroutine(DelayToExcuteFinish("PlayerThreeUnit"));
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
            if (StartToExcute)
            {
                tick += Time.deltaTime;
                PlayCylinder.transform.localScale = Vector3.Lerp(PlayCylinder.transform.localScale, TargetScale, tick / 5);
                if (PlayCylinder.transform.localScale == TargetScale)
                {
                    StartToExcute = false;
                    tick = 0;
                    ProcessFinish("PlayerFourUnit");
                }
            }

        }

        public override void Destory()
        {
            BEventManager.Instance().RemoveListener(BatEventDefine.LEFTPRESSEVENT, PlayerTwoLeftPressCallback);
            GameObject.Destroy(PlayerTwo);
        }
    }

}
