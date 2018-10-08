using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.ConstDefine;
using THEDARKKNIGHT.UI;
using UnityEngine;
using UnityEngine.UI;

namespace THEDARKKNIGHT.Example.UI
{
    /// <summary>
    /// BaseWindos
    /// It is the class which contain base function about UI windos Control
    /// All the UI componet should to inherit this class
    /// </summary>
    public class BrotherWindows : BBaseWindows
    {

        Button SendMsgToParentWindowsBtn;
        Button SendMsgToMyWindowsBtn;
        Button SendMsgToSubWindowsBtn;
        Button SendMsgToAll;
        Text MessageText;

        public override void AddListener()
        {
            SendMsgToParentWindowsBtn.onClick.AddListener(SendMsgToParentWindowsFunction);
            SendMsgToMyWindowsBtn.onClick.AddListener(SendMsgToMyWindowsFunction);
            SendMsgToSubWindowsBtn.onClick.AddListener(SendMsgToSubWindowsFunction);
            SendMsgToAll.onClick.AddListener(SendMsgToAllFunction);
        }

        private void SendMsgToAllFunction()
        {
            PostMsgToAllWindows("Hello Gays!I am BrotherWindows");
        }

        private void SendMsgToSubWindowsFunction()
        {
            PostMsgToWindows("SubWindows", "Hello SubWindows!I am BrotherWindows");
        }

        private void SendMsgToMyWindowsFunction()
        {
            PostMsgToWindows("MyWindows", "MyWindows! I am BrotherWindows");
        }

        private void SendMsgToParentWindowsFunction()
        {
            SendMsgToParentWindows("Hello ParentWindows!I am BrotherWindows");
        }

        public BrotherWindows(string name)
        {
            WindowsAlias = name;
        }


        public override void DataInit()
        {

        }

        public override void DestoryWindows()
        {

        }

        public override bool GetWindowsMsg(int windowsID, string windowsAlias, object data)
        {
            switch (windowsAlias)
            {
                case "RootWindows":
                    MessageText.text = (string)data;
                    break;
                case "BrotherWindows":
                    MessageText.text = (string)data;
                    break;
                case "MyWindows":
                    MessageText.text = (string)data;
                    break;
                case "SubWindows":
                    MessageText.text = (string)data;
                    break;
                case BROADCASTINFO:
                    MessageText.text = (string)data;
                    break;
            }
            return true;
        }

        public override void Hide()
        {

        }

        public override void RemoveListener()
        {

        }

        public override void Show()
        {

        }

        public override Transform UIInit(Transform UIRoot)
        {
            GameObject UINode = Resources.Load(BFameWorkPathDefine.BFameResourceTestUIPath + "/BrotherPanel") as GameObject;
            UINode = GameObject.Instantiate(UINode, UIRoot) as GameObject;
            MessageText = UINode.transform.Find("MessageText").GetComponent<Text>();
            SendMsgToParentWindowsBtn = UINode.transform.Find("SendMsgToParentWindows").GetComponent<Button>();
            SendMsgToMyWindowsBtn = UINode.transform.Find("SendMsgToMyWindows").GetComponent<Button>();
            SendMsgToSubWindowsBtn = UINode.transform.Find("SendMsgToSubWindows").GetComponent<Button>();
            SendMsgToAll = UINode.transform.Find("SendMsgToAll").GetComponent<Button>();
            return UINode.transform;
        }
    }
}
