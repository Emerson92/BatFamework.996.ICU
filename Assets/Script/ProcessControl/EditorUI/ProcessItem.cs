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

        [SerializeField,SetProperty("RedefineNodeProproty")]
        public string BranchID = null;

        [SerializeField, SetProperty("RedefineNodeProproty")]
        public string[] SubBranchID;

        public string BranchParent;

        [Output]
        public ProcessItem OutPortProcess;

        public ClassType[] ProcessItems;

        public object RedefineNodeProproty {
            set {
                UpdateBranchParent();
            }
        }

        /// <summary>
        /// Update all the Node branch Parent
        /// </summary>
        private void UpdateBranchParent()
        {
            ////TODO
            
        }


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
            Debug.Log(this.name+ "  OnCreateConnection "+ " from :"+ from.node.name + " to :" + to.node.name);
            base.OnCreateConnection(from ,to);
            if (from.node == this)
            {
                OutPortProcess = to.node as ProcessItem;
            }
            else {

                EnterProcess = from.node as ProcessItem;
                CheckBranchParent(from);
            }
        }

        private void CheckBranchParent(NodePort from)
        {
            if (((ProcessItem)(from.node)).BranchID != null)
            {
                BranchID = ((ProcessItem)(from.node)).BranchID;
                BranchParent = ((ProcessItem)(from.node)).BranchParent;
            }
            else 
            {
                BranchParent = ((ProcessItem)(from.node)).name;
            }
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
