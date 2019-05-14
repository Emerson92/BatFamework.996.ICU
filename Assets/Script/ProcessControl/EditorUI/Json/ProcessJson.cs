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

        public string name;

        public List<SubProcess> SubProcessList;

        //public Vector2 position;
    }

    [Serializable]
    public class SubProcess {

        public string Namespace;

        public string ClassName;

    }
}
