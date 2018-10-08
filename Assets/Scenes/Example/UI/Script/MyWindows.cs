using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.ConstDefine;
using THEDARKKNIGHT.UI;
using UnityEngine;
using UnityEngine.UI;

namespace THEDARKKNIGHT.Example.UI
{
    public class MyWindows : BBaseWindows
    {

        Button SendMsgToParentBtn;
        Button SendMsgToSubBtn;
        Button SendMsgToBrotherBtn;
        Button CloseWindowsBtn;
        Button OpenSubWindowsBtn;
        GameObject UINode;
        Text MessageText;

        public MyWindows(string name)
        {
            WindowsAlias = name;
        }

        public override void AddListener()
        {
            SendMsgToParentBtn.onClick.AddListener(SendMsgToParentFunction);
            SendMsgToSubBtn.onClick.AddListener(SendMsgToSubFunction);
            SendMsgToBrotherBtn.onClick.AddListener(SendMsgToBrotherFunction);
            CloseWindowsBtn.onClick.AddListener(CloseWindowsFunction);
            OpenSubWindowsBtn.onClick.AddListener(OpenSubWindowsFunction);
        }

        private void CloseWindowsFunction()
        {
            Hide();
        }

        private void OpenSubWindowsFunction()
        {
            ShowAllSubWindows();
        }

        private void SendMsgToBrotherFunction()
        {
            PostMsgToWindows("BrotherWindows", "Hello BrotherWindows!I am MyWindows");
        }

        private void SendMsgToSubFunction()
        {
            SendMsgToAllSubWindows("Hello SubWindows!I am MyWindows");
        }

        private void SendMsgToParentFunction()
        {
            SendMsgToParentWindows("Hello ParentWindows!I am MyWindows");
        }

        public override void DataInit()
        {
            SubWindows windows = new SubWindows("SubWindows");
            RegisterWindows(windows.WindowsAlias, windows);
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
            UINode.SetActive(false);
            WindowsStatus = false;
        }

        public override void RemoveListener()
        {
        }

        public override void Show()
        {
            UINode.SetActive(true);
            WindowsStatus = true;
        }

        public override Transform UIInit(Transform UIRoot)
        {
            UINode = Resources.Load(BFameWorkPathDefine.BFameResourceTestUIPath + "/MyPanel") as GameObject;
            UINode = GameObject.Instantiate(UINode, UIRoot) as GameObject;
            SendMsgToParentBtn = UINode.transform.Find("SendMsgToParent").GetComponent<Button>();
            SendMsgToSubBtn = UINode.transform.Find("SendMsgToSub").GetComponent<Button>();
            SendMsgToBrotherBtn = UINode.transform.Find("SendMsgToBrother").GetComponent<Button>();
            CloseWindowsBtn = UINode.transform.Find("Close").GetComponent<Button>();
            OpenSubWindowsBtn = UINode.transform.Find("OpenSubWindows").GetComponent<Button>();
            MessageText = UINode.transform.Find("MessageText").GetComponent<Text>();
            return UINode.transform;
        }
    }
}
