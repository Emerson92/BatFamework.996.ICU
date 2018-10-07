using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.ConstDefine;
using THEDARKKNIGHT.UI;
using UnityEngine;
namespace THEDARKKNIGHT.Example.UI
{
    /// <summary>
    /// BaseWindos
    /// It is the class which contain base function about UI windos Control
    /// All the UI componet should to inherit this class
    /// </summary>
    public class ParentWindows : BBaseWindows
    {
        public ParentWindows(string name){
            WindowsAlias = name;
        }

        public override void AddListener()
        {

        }

        public override void DestoryWindows()
        {

        }

        public override bool GetWindowsMsg(int windowsID, string windowsAlias, object data)
        {
            return true;
        }

        public override void Hide()
        {

        }

        public override void Init(MonoBehaviour main)
        {
            GameObject UIRoot = Resources.Load(BFameWorkPathDefine.BFameResourceTestUIPath+"/UIRoot") as GameObject;
            GameObject.Instantiate(UIRoot);
        }

        public override void RemoveListener()
        {

        }

        public override void Show()
        {

        }
        public override void Awake(MonoBehaviour main)
        {
            Debug.Log("Excute the Awake Function!!");

        }
    }

}
