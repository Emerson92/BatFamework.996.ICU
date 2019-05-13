using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.ProcessCore;
using UnityEngine;
using XNode;
namespace THEDARKKNIGHT.ProcessCore.Graph {
   
    public class ProcessItem : XNode.Node
    {
        [Input]
        public ProcessPort EnterProcess;

        [Output]
        public ProcessPort outPortProcess;

        public ClassType[] ProcessItems;

        public ProcessPort OutPortProcess
        {
            get
            {
                return outPortProcess;
            }

            set
            {
                outPortProcess = value;
            }
        }

        // Use this for initialization
        protected override void Init()
        {
            base.Init();

        }

        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port)
        {
            return null; // Replace this
        }
    }

    [Serializable]
    public class ClassType {

        public string className;

        public string Namespace;

        public BProcessItem item;
    }
}
