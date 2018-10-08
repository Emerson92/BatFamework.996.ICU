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
    public class ParentWindows : BBaseWindows
    {

        Button SendToChildsBtn;
        Button SendToBrother;
        Button SendToMyWindows;
        Button RecoveryBtn;
        Text MessageText;
        GameObject UINode;

        public ParentWindows(string name){
            WindowsAlias = name;
        }

        public override void AddListener()
        {
            SendToChildsBtn.onClick.AddListener(SendToChildsFunction);
            SendToBrother.onClick.AddListener(SendToBrotherFunction);
            SendToMyWindows.onClick.AddListener(SendToMyWindowsFunction);
            RecoveryBtn.onClick.AddListener(RecoveryFunction);
        }

        private void RecoveryFunction()
        {
            ShowAllSubWindows();
        }

        private void SendToMyWindowsFunction()
        {
            PostMsgToWindows("MyWindows", "Hello MyWindows!I am Parent Windows");
        }

        private void SendToBrotherFunction()
        {
            PostMsgToWindows("BrotherWindows", "Hello BrotherWindows!I am Parent Windows");
        }

        private void SendToChildsFunction()
        {
            SendMsgToAllSubWindows("Hello Childs!I am Parent Windows");
        }

        public override void DestoryWindows()
        {

        }

        public override bool GetWindowsMsg(int windowsID, string windowsAlias, object data)
        {
            switch (windowsAlias) {
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

        public override void RegisterErrorCallback(string subwindowsName)
        {
            Debug.Log("RegisterErrorCallback"+subwindowsName);
        }

        public override void DataInit()
        {
            BrotherWindows broWindows = new BrotherWindows("BrotherWindows");
            MyWindows myWindows = new MyWindows("MyWindows");
            RegisterWindows(broWindows.WindowsAlias, broWindows);
            RegisterWindows(myWindows.WindowsAlias, myWindows);
        }

        public override void RemoveListener()
        {

        }

        public override void Show()
        {
            UINode.SetActive(true);
            WindowsStatus = true;
        }
        public override void Awake(MonoBehaviour main)
        {
            Debug.Log("Excute the Awake Function!!");

        }

        public override Transform UIInit(Transform UIRoot)
        {
            UINode = Resources.Load(BFameWorkPathDefine.BFameResourceTestUIPath + "/UIRoot") as GameObject;
            UINode = GameObject.Instantiate(UINode) as GameObject;
            SendToChildsBtn = UINode.transform.Find("MainPanel/SendToChilds").GetComponent<Button>();
            SendToBrother   = UINode.transform.Find("MainPanel/SendToBrother").GetComponent<Button>();
            SendToMyWindows = UINode.transform.Find("MainPanel/SendToMyWindows").GetComponent<Button>();
            RecoveryBtn = UINode.transform.Find("MainPanel/Recovery All Windows").GetComponent<Button>();
            MessageText = UINode.transform.Find("MainPanel/Text").GetComponent<Text>();
            return UINode.transform.Find("MainPanel");
        }
    }

}
