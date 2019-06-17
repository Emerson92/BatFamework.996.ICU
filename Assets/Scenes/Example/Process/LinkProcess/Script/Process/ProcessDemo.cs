using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.ProcessCore;
using UnityEngine;
namespace THEDARKKNIGHT.Example
{
    public class ProcessDemo : BProcessLinkCore<BProcessUnit<BProcessItem>, BProcessItem> {

        public ProcessDemo() {
            Debug.Log("Create new ProcessDemo!");
        }
    }
}
