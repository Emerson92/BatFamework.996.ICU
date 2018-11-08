﻿using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.ConstDefine;
using THEDARKKNIGHT.InputOperate;
using THEDARKKNIGHT.ProcessCore;
using THEDARKKNIGHT.ProcessCore.Lua;
using UnityEngine;
namespace THEDARKKNIGHT.Example {
    public class LuaProcessDemo : MonoBehaviour
    {
        ProcessDemo ProcessControl;

        public GameObject PlayerAreaOne { get; private set; }
        public GameObject PlayerAreaTwo { get; private set; }
        public GameObject PlayerAreaThree { get; private set; }

        // Use this for initialization
        void Start()
        {
            BInputOperateEngine.Instance();
            ProcessControl = new ProcessDemo();
            ProcessControl.SetProcessUnitStartCallback(ProcessUnitStart);
            ProcessControl.SetProcessUnitFinishCallback(ProcessUnitFinish);


            LuaBProcessItem PlayerOne = new LuaBProcessItem("LuaProcessPlayerOneDemo", "PlayerOne");

            BProcessUnit<BProcessItem> PlayerOneUnit = new BProcessUnit<BProcessItem>(PlayerOne);
            PlayerOneUnit.SetUnitTagName("PlayerOneUnit");

            LuaBProcessItem PlayerTwo_One = new LuaBProcessItem("LuaProcessPlayerTwoOfOneDemo", "PlayerTwo_One");
            LuaBProcessItem PlayerTwo_Two = new LuaBProcessItem("LuaProcessPlayerTwoOfTwoDemo", "PlayerTwo_Two");
            LuaBProcessItem PlayerTwo_Three = new LuaBProcessItem("LuaProcessPlayerTwoOfThreeDemo", "PlayerTwo_Three");
            BProcessUnit<BProcessItem> PlayerTwoUnit = new BProcessUnit<BProcessItem>(PlayerTwo_One, PlayerTwo_Two, PlayerTwo_Three);
            PlayerTwoUnit.SetUnitTagName("PlayerTwoUnit");

            LuaBProcessItem PlayerThree_One = new LuaBProcessItem("LuaProcessPlayerThreeOfOneDemo", "PlayerThree_One");
            BProcessUnit<BProcessItem> PlayerThreeUnit = new BProcessUnit<BProcessItem>(PlayerThree_One);
            PlayerThreeUnit.SetUnitTagName("PlayerThreeUnit");

            ProcessControl.AddProcessUnit(PlayerOneUnit);
            ProcessControl.AddProcessUnit(PlayerTwoUnit);
            ProcessControl.AddProcessUnit(PlayerThreeUnit);
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
                    if ((string)data == "B")
                    {
                        ProcessControl.StartProcess();
                        return false;
                    }
                    break;
                case "PlayerFourUnit":
                    {
                        ProcessControl.StartProcess();
                        return false;
                    }
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
                    //case "PlayerFourUnit":
                    //    if (!PlayAreaFour)
                    //    {
                    //        GameObject PlayerArea4 = Resources.Load(BFameWorkPathDefine.BFameResourceTestProcessPath + "/PlayAreaFour") as GameObject;
                    //        PlayAreaFour = GameObject.Instantiate(PlayerArea4);
                    //    }
                    //    CameraCtrl.Instance().LerpFocusCenter(PlayAreaFour.transform.position);
                    //    break;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

