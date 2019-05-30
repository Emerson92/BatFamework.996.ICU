using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
namespace THEDARKKNIGHT.ProcessCore.Graph {

    [CreateAssetMenu]
    public class ProcessGraph : XNode.NodeGraph
    {
        public static bool IsLuaScript;
    }

    [Serializable]
    public class ProcessPort { }
}
