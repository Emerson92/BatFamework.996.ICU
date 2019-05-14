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
        public ProcessItem EnterProcess;

        [Input]
        public int Branch= 0;


        [Output]
        public ProcessItem OutPortProcess;

        public ClassType[] ProcessItems;

        // Use this for initialization
        protected override void Init()
        {
            base.Init();
        }

        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port)
        {
            return OutPortProcess;
        }

        private void RefreshStatue(NodePort port)
        {
            if (port.ConnectionCount > 0)
            {
                for (int i = 0; i < port.ConnectionCount; i++)
                {
                    NodePort nextPort = port.GetConnection(i);
                    if (nextPort != port)
                    {
                        if (nextPort.IsConnected)
                            OutPortProcess = nextPort.node as ProcessItem;
                        else
                            OutPortProcess = null;
                    }
                }
            }
            else
            {
                EnterProcess = null;
                OutPortProcess = null;
            }
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            base.OnCreateConnection(from ,to);
            if (from.node == this)
                OutPortProcess = to.node as ProcessItem;
            else
                EnterProcess = to.node as ProcessItem;
        }

        public override void OnRemoveConnection(NodePort port)
        {
            base.OnRemoveConnection(port);
            switch (port.fieldName) {
                case "EnterProcess":
                    EnterProcess = null;
                    break;
                case "OutPortProcess" :
                    OutPortProcess = null;
                    break;
            }
        }
    }



    [Serializable]
    public class ClassType {

        public string className;

        public string Namespace;

        public BProcessItem item;
    }
}
