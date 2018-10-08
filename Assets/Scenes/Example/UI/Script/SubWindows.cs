using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.ConstDefine;
using THEDARKKNIGHT.UI;
using UnityEngine;
using UnityEngine.UI;

namespace THEDARKKNIGHT.Example.UI
{
    public class SubWindows : BBaseWindows
    {
        Button SendToParentWindowsBtn;
        Button SendToMyWindowsBtn;
        Button SendToAllBtn;
        Button SendToBrotherWindowsBtn;
        Button CloseBtn;
        Text MessageText;
        GameObject UINode;
        public SubWindows(string name)
        {
            WindowsAlias = name;
        }


        public override void AddListener()
        {
            SendToParentWindowsBtn.onClick.AddListener(SendToParentWindowsFunction);
            SendToMyWindowsBtn.onClick.AddListener(SendToMyWindowsFunction);
            SendToAllBtn.onClick.AddListener(SendToAllFunction);
            SendToBrotherWindowsBtn.onClick.AddListener(SendToBrotherWindowsFunction);
            CloseBtn.onClick.AddListener(CloseFunction);
        }

        private void CloseFunction()
        {
            Hide();
        }

        private void SendToBrotherWindowsFunction()
        {
            PostMsgToWindows("BrotherWindows", "Hello BrotherWindows!I am SubWindows");
        }

        private void SendToAllFunction()
        {
            PostMsgToAllWindows("Hello Gays!I am SubWindows");
        }

        private void SendToMyWindowsFunction()
        {
            PostMsgToWindows("MyWindows", "Hello MyWindows!I am SubWindows");
        }

        private void SendToParentWindowsFunction()
        {
            SendMsgToParentWindows("Hello ParentWindows!I am SubWindows");
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
            UINode = Resources.Load(BFameWorkPathDefine.BFameResourceTestUIPath + "/SubPanel") as GameObject;
            UINode = GameObject.Instantiate(UINode, UIRoot) as GameObject;
            SendToParentWindowsBtn = UINode.transform.Find("SendToParentWindows").GetComponent<Button>();
            SendToMyWindowsBtn = UINode.transform.Find("SendToMyWindows").GetComponent<Button>();
            SendToAllBtn = UINode.transform.Find("SendToAll").GetComponent<Button>();
            SendToBrotherWindowsBtn = UINode.transform.Find("SendToBrotherWindows").GetComponent<Button>();
            CloseBtn = UINode.transform.Find("Close").GetComponent<Button>();
            MessageText = UINode.transform.Find("Message").GetComponent<Text>();
            return UINode.transform;
        }
    }
}
