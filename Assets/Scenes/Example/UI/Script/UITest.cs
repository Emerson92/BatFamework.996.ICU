using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.Example.UI
{
    public class UITest : MonoBehaviour
    {
        ParentWindows RootWindows;

        // Use this for initialization
        void Start()
        {
            Debug.Log("UITest");
            RootWindows = new ParentWindows("RootWindows");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
