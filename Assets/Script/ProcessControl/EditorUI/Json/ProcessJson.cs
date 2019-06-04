using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.ProcessCore.Graph.Json {

    [Serializable]
    public class ProcessJson 
    {
        public List<ProcessUnit> ProcessList;
    }

    [Serializable]
    public class ProcessUnit {

        public string Name;

        public Vector2 Pos;

        public bool IsLuaScript;

        public List<SubProcess> SubProcessList;

        public List<SubLuaProcess> SubLuaProcessList;

        public string BranchID;

        public string BranchParentName;

        public string[] SubBranchID;

        public string[] ParBranchID;
    }

    [Serializable]
    public class SubProcess {

        public string Nickname;

        public string Namespace;

        public string ClassName;

    }

    [Serializable]
    public class SubLuaProcess {

        public string Nickname;

        public string UrlPath;
    }
}
