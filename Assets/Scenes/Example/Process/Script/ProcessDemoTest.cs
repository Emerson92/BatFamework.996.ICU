using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.ConstDefine;
using THEDARKKNIGHT.InputOperate;
using THEDARKKNIGHT.ProcessCore;
using UnityEngine;
namespace THEDARKKNIGHT.Example
{
    public class ProcessDemoTest : MonoBehaviour
    {

        ProcessDemo ProcessControl;

        GameObject PlayerAreaTwo { get;  set; }
        GameObject PlayerAreaOne { get;  set; }
        GameObject PlayerAreaThree { get; set; }
        GameObject PlayAreaFour { get; set; }

        // Use this for initialization
        void Start()
        {
            BInputOperateEngine.Instance();
            ProcessControl = new ProcessDemo();
            ProcessControl.SetProcessUnitStartCallback(ProcessUnitStart);
            ProcessControl.SetProcessUnitFinishCallback(ProcessUnitFinish);
            ProcessPlayerOneDemo PlayerOne = new ProcessPlayerOneDemo("PlayerOne");

            BProcessUnit<BProcessItem> PlayerOneUnit = new BProcessUnit<BProcessItem>(PlayerOne);
            PlayerOneUnit.SetUnitTagName("PlayerOneUnit");

            ProcessPlayerTwoOfOneDemo   PlayerTwoOfOne = new ProcessPlayerTwoOfOneDemo("PlayerTwoOfOne");
            ProcessPlayerTwoOfTwoDemo   PlayerTwoOfTwo   = new ProcessPlayerTwoOfTwoDemo("PlayerTwoOfTwo");
            ProcessPlayerTwoOfThreeDemo PlayerTwoOfThree = new ProcessPlayerTwoOfThreeDemo("PlayerTwoOfThree");

            BProcessUnit<BProcessItem>  PlayerTwoUnit   = new BProcessUnit<BProcessItem>(PlayerTwoOfOne, PlayerTwoOfTwo, PlayerTwoOfThree);
            PlayerTwoUnit.SetUnitTagName("PlayerTwoUnit");

            ProcessPlayerThreeOfOneDemo ProcessPlayerThreeOfOne = new ProcessPlayerThreeOfOneDemo("PlayerThreeOfOne");
            ProcessPlayerThreeOfTwoDemo ProcessPlayerThreeOfTwo = new ProcessPlayerThreeOfTwoDemo("PlayerThreeOfTwo");

            BProcessUnit<BProcessItem>  PlayerThreeUnit = new BProcessUnit<BProcessItem>(ProcessPlayerThreeOfOne, ProcessPlayerThreeOfTwo);
            PlayerThreeUnit.SetUnitTagName("PlayerThreeUnit");

            ProcessPlayerFourDemo ProcessPlayerFour = new ProcessPlayerFourDemo("PlayerFour");
            BProcessUnit<BProcessItem> PlayerFourUnit = new BProcessUnit<BProcessItem>(ProcessPlayerFour);
            PlayerFourUnit.SetUnitTagName("PlayerFourUnit");

            ProcessControl.AddProcessUnit(PlayerOneUnit);
            ProcessControl.AddProcessUnit(PlayerTwoUnit);
            ProcessControl.AddProcessUnit(PlayerThreeUnit);
            ProcessControl.AddProcessUnit(PlayerFourUnit);
            ProcessControl.StartProcess();
        }

        private void ProcessUnitStart(string name, object data)
        {
            Debug.Log("ProcessUnitStart :"+ name);
            switch (name) {
                case "PlayerOneUnit":
                    if (!PlayerAreaOne){
                        GameObject PlayerArea1 = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/PlayAreaOne") as GameObject;
                        PlayerAreaOne = GameObject.Instantiate(PlayerArea1);
                    }
                    CameraCtrl.Instance().LerpFocusCenter(PlayerAreaOne.transform.position);
                    break;
                case "PlayerTwoUnit":
                    if (!PlayerAreaTwo){
                        GameObject PlayerArea2 = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/PlayAreaTwo") as GameObject;
                        PlayerAreaTwo = GameObject.Instantiate(PlayerArea2);
                    }
                    CameraCtrl.Instance().SetObserverRadius(8f);
                    CameraCtrl.Instance().LerpFocusCenter(PlayerAreaTwo.transform.position);
                    break;
                case "PlayerThreeUnit":
                    if(!PlayerAreaThree){
                        GameObject PlayerArea3 = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/PlayAreaThree") as GameObject;
                        PlayerAreaThree = GameObject.Instantiate(PlayerArea3);
                    }
                    CameraCtrl.Instance().LerpFocusCenter(PlayerAreaThree.transform.position);
                    break;
                case "PlayerFourUnit":
                    if(!PlayAreaFour){
                        GameObject PlayerArea4 = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/PlayAreaFour") as GameObject;
                        PlayAreaFour = GameObject.Instantiate(PlayerArea4);
                    }
                    CameraCtrl.Instance().LerpFocusCenter(PlayAreaFour.transform.position);
                    break;
            }
        }

        private void ProcessUnitFinish(string name, object data)
        {
            switch (name)
            {
                case "PlayerOneUnit":
                    break;
                case "PlayerTwoUnit":
                    break;
                case "PlayerThreeUnit":
                    if ((string)data == "B")
                    {
                        ProcessControl.RemoveProcessUnitOnIndex(3);
                        ProcessControl.AddProcessUnitOnIndex(2, ProcessControl.GetProcessUnitAtIndex(0));
                    }
                    break;
                case "PlayerFourUnit":
                    break;
            }
        }


        // Update is called once per frame
        void Update()
        {

        }
    }

}

