using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.ConstDefine;
using THEDARKKNIGHT.InputOperate;
using THEDARKKNIGHT.ProcessCore;
using THEDARKKNIGHT.ProcessCore.Graph.Json;
using UnityEngine;
namespace THEDARKKNIGHT.Example {
    public class TreeProcessDemoTest : MonoBehaviour
    {
        private ProcessTreeDemo ProcessControl;
#if UNITY_EDITOR
        public TextAsset text;
#else
        private TextAsset text;
#endif
        GameObject PlayerAreaTwo { get; set; }
        GameObject PlayerAreaOne { get; set; }
        GameObject PlayerAreaThree { get; set; }
        GameObject PlayAreaFour { get; set; }
        GameObject PlayAreaFive { get; set; }
        GameObject PlayAreaSix { get; set; }

        // Use this for initialization
        void Start()
        {
            BInputOperateEngine.Instance();
            ProcessControl = new ProcessTreeDemo();
            ProcessControl.AddProcessUnitByJson(text.text);
            ProcessControl.SetProcessUnitStartCallback(ProcessUnitStart);
            ProcessControl.SetProcessUnitFinishCallback(ProcessUnitFinish);
            ProcessControl.StartProcess();
        }

        private bool ProcessUnitFinish(string name, object data)
        {
            switch (name)
            {
                case "PlayerOneUnit":
                    break;
                case "PlayerTwoUnit":
                    break;
                case "PlayerThreeUnit":

                    break;
            }
            return true;
        }

        private void ProcessUnitStart(string name, object data)
        {

            switch (name)
            {
                case "PlayerOneUnit":
                    if (!PlayerAreaOne)
                    {
                        GameObject PlayerArea1 = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/PlayAreaOne") as GameObject;
                        PlayerAreaOne = GameObject.Instantiate(PlayerArea1);
                    }
                    CameraCtrl.Instance().LerpFocusCenter(PlayerAreaOne.transform.position);
                    break;
                case "PlayerTwoUnit":
                    if (!PlayerAreaTwo)
                    {
                        GameObject PlayerArea2 = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/PlayAreaTwo") as GameObject;
                        PlayerAreaTwo = GameObject.Instantiate(PlayerArea2);
                    }
                    CameraCtrl.Instance().SetObserverRadius(8f);
                    CameraCtrl.Instance().LerpFocusCenter(PlayerAreaTwo.transform.position);
                    break;
                case "PlayerThreeUnit":
                    if (!PlayerAreaThree)
                    {
                        GameObject PlayerArea3 = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/PlayAreaThree") as GameObject;
                        PlayerAreaThree = GameObject.Instantiate(PlayerArea3);
                    }
                    CameraCtrl.Instance().LerpFocusCenter(PlayerAreaThree.transform.position);
                    break;
                case "PlayerFourUnit":
                    if (!PlayAreaFour)
                    {
                        GameObject PlayerArea4 = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/PlayAreaFour") as GameObject;
                        PlayAreaFour = GameObject.Instantiate(PlayerArea4);
                    }
                    CameraCtrl.Instance().LerpFocusCenter(PlayAreaFour.transform.position);
                    break;
                case "PlayerFiveUnit":
                    if (!PlayAreaFive)
                    {
                        GameObject PlayerArea5 = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/PlayAreaFive") as GameObject;
                        PlayAreaFive = GameObject.Instantiate(PlayerArea5);
                    }
                    CameraCtrl.Instance().LerpFocusCenter(PlayAreaFive.transform.position);
                    break;
                case "PlayerSixOneUnit":
                case "PlayerSixTwoUnit":
                    if (!PlayAreaSix)
                    {
                        GameObject PlayerArea6 = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/PlayAreaSix") as GameObject;
                        PlayAreaSix = GameObject.Instantiate(PlayerArea6);
                    }
                    CameraCtrl.Instance().LerpFocusCenter(PlayAreaSix.transform.position);
                    break;

            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CreateProcessTreeJson() {
            //root first node
            ProcesTreeJson rootJson = new ProcesTreeJson();
            rootJson.Name = "PlayerOneUnit";
            SubProcess rootProcess = new SubProcess();
            rootProcess.Nickname  = "PlayerOne";
            rootProcess.Namespace = "THEDARKKNIGHT.Example";
            rootProcess.ClassName = "ProcessPlayerOneDemo";
            rootJson.SubProcessList = new List<SubProcess>();
            rootJson.SubProcessList.Add(rootProcess);

            //Second node
            ProcesTreeJson secondJson = new ProcesTreeJson();
            secondJson.Name = "PlayerTwoUnit";
            SubProcess secondProcess = new SubProcess();
            secondProcess.Nickname = "PlayerTwo";
            secondProcess.Namespace = "THEDARKKNIGHT.Example";
            secondProcess.ClassName = "ProcessPlayerTwoDemo";
            secondJson.SubProcessList = new List<SubProcess>();
            secondJson.SubProcessList.Add(secondProcess);

            //Thrid node
            ProcesTreeJson thridJson = new ProcesTreeJson();
            thridJson.Name = "PlayerThreeUnit";
            SubProcess thridProcessOne = new SubProcess();
            thridProcessOne.Nickname = "PlayerThreeOfOne";
            thridProcessOne.Namespace = "THEDARKKNIGHT.Example";
            thridProcessOne.ClassName = "ProcessPlayerThreeOfTwoDemo";
            SubProcess thridProcessTwo = new SubProcess();
            thridProcessTwo.Nickname = "PlayerThreeOfTwo";
            thridProcessTwo.Namespace = "THEDARKKNIGHT.Example";
            thridProcessTwo.ClassName = "ProcessPlayerThreeOfTwoDemo";
            thridJson.SubProcessList = new List<SubProcess>();
            thridJson.SubProcessList.Add(thridProcessOne);
            thridJson.SubProcessList.Add(thridProcessTwo);

            //Four node
            ProcesTreeJson fourJson = new ProcesTreeJson();
            fourJson.Name = "PlayerFourUnit";
            SubProcess fourProcess  = new SubProcess();
            fourProcess.Nickname    = "PlayerFour";
            fourProcess.Namespace   = "THEDARKKNIGHT.Example";
            fourProcess.ClassName   = "ProcessPlayerFourDemo";
            fourJson.SubProcessList = new List<SubProcess>();
            fourJson.SubProcessList.Add(fourProcess);

            //Five node
            ProcesTreeJson fiveJson = new ProcesTreeJson();
            fiveJson.Name = "PlayerFiveUnit";
            SubProcess fiveProcess = new SubProcess();
            fiveProcess.Nickname = "PlayerFive";
            fiveProcess.Namespace = "THEDARKKNIGHT.Example";
            fiveProcess.ClassName = "ProcessPlayerFiveDemo";
            fiveJson.SubProcessList = new List<SubProcess>();
            fiveJson.SubProcessList.Add(fiveProcess);

            //Six node
            ProcesTreeJson six_OneJson = new ProcesTreeJson();
            six_OneJson.Name = "PlayerSixOneUnit";
            SubProcess six_oneProcess = new SubProcess();
            six_oneProcess.Nickname = "PlayerSix";
            six_oneProcess.Namespace = "THEDARKKNIGHT.Example";
            six_oneProcess.ClassName = "ProcessPlayerSixDemo";
            six_OneJson.SubProcessList = new List<SubProcess>();
            six_OneJson.SubProcessList.Add(six_oneProcess);

            //Six_two node
            ProcesTreeJson six_twoJson = new ProcesTreeJson();
            six_twoJson.Name = "PlayerSixTwoUnit";
            SubProcess six_twoProcess = new SubProcess();
            six_twoProcess.Nickname = "PlayerSix";
            six_twoProcess.Namespace = "THEDARKKNIGHT.Example";
            six_twoProcess.ClassName = "ProcessPlayerSixDemo";
            six_twoJson.SubProcessList = new List<SubProcess>();
            six_twoJson.SubProcessList.Add(six_twoProcess);


            rootJson.SubTrees   = new List<ProcesTreeJson>();
            secondJson.SubTrees = new List<ProcesTreeJson>();
            thridJson.SubTrees  = new List<ProcesTreeJson>();
            fourJson.SubTrees   = new List<ProcesTreeJson>();
            fiveJson.SubTrees   = new List<ProcesTreeJson>();

            rootJson.SubTrees.Add(secondJson);
            secondJson.SubTrees.Add(thridJson);
            secondJson.SubTrees.Add(fourJson);
            thridJson.SubTrees.Add(fiveJson);
            fourJson.SubTrees.Add(six_OneJson);
            fiveJson.SubTrees.Add(six_twoJson);

            string data = JsonUtility.ToJson(rootJson);

            Debug.Log(data);
        }
    }

}

