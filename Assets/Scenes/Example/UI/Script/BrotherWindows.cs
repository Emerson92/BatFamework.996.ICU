using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.UI;
using UnityEngine;
namespace THEDARKKNIGHT.Example.UI
{
    /// <summary>
    /// BaseWindos
    /// It is the class which contain base function about UI windos Control
    /// All the UI componet should to inherit this class
    /// </summary>
    public class BrotherWindows : BBaseWindows
    {
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

        public override void Init(MonoBehaviour mai)
        {

        }

        public override void RemoveListener()
        {

        }

        public override void Show()
        {

        }
    }
}
