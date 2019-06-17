using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.ProcessCore.Graph.Json
{
    [Serializable]
    public class ProcesTreeJson
    {
        public string Name;

        public bool IsLuaScript;

        public List<SubProcess> SubProcessList;

        public List<SubLuaProcess> SubLuaProcessList;

        public List<ProcesTreeJson> SubTrees;
    }
}

