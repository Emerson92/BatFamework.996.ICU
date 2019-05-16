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
        public ProcessItem[] Test;

        [Input]
        public ProcessItem EnterProcess;

        [SerializeField, SetProperty("RedefineNodeProproty")]
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
            FindBranchParent();////Find Parent Branch 
            //CheckBackwardChild();////Check Child Statues
        }

        private void FindBranchParent()
        {
            foreach (NodePort p in Inputs)
            {
                Debug.Log("p :"+ p.node.name);
                //if(p.node != this)
                   // CheckForwardNode(p,this);
            }
        }

        private void CheckForwardNode(NodePort nodePort, ProcessItem lastNode)
        {
            if (string.IsNullOrEmpty(((ProcessItem)nodePort.node).BranchID) || ((ProcessItem)nodePort.node).BranchID != this.BranchID)
            {
                this.BranchParent = ((ProcessItem)nodePort.node).name;
            }
            else
            {
                foreach (NodePort p in ((ProcessItem)nodePort.node).Inputs)
                {
                    if (p.node != lastNode)
                        CheckForwardNode(p, ((ProcessItem)nodePort.node));
                }
            }
        }

        private void CheckBackwardChild()
        {
            foreach (NodePort p in Outputs)
            {
                CheckBackwardNode(p,this);
            }
        }

        private void CheckBackwardNode(NodePort nodePort, ProcessItem lastNode)
        {
            if (((ProcessItem)nodePort.node).BranchID != lastNode.BranchID)
            {
                ((ProcessItem)nodePort.node).BranchParent = lastNode.BranchParent;
            }
            else {
                foreach (NodePort p in ((ProcessItem)nodePort.node).Outputs)
                {
                    CheckBackwardNode(p, ((ProcessItem)nodePort.node));
                }
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
            //Debug.Log(this.name+ "  OnCreateConnection "+ " from :"+ from.node.name + " to :" + to.node.name);
            base.OnCreateConnection(from ,to);
            if (from.node == this)
            {
                OutPortProcess = to.node as ProcessItem;
            }
            else {
                EnterProcess = from.node as ProcessItem;
                for (int i = 0 ; i < Test.Length ; i++ ) {
                    if (Test[i] == null)
                    {
                        Test[i] = from.node as ProcessItem;
                        break;
                    }
                    else if (i == Test.Length - 1) {
                        ///端口已经连接的接口
                        Test[i] = from.node as ProcessItem;
                        break;
                    }
                }
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

        public override void OnRemoveConnection(NodePort output, NodePort input)
        {
            //Debug.Log(this.name + "  OnRemoveConnection " + " from :" + from.node.name + " to :" + to.node.name);
            base.OnRemoveConnection(output, input);
            if (output.node == this)
            {
                OutPortProcess = null;
            }
            else {
                EnterProcess = null;
                for (int i = 0; i < Test.Length; i++)
                {
                    if (Test[i] == output.node)
                    {
                        Test[i] = null;
                        break;
                    }
                }
            }
            //switch (port.fieldName) {
            //    case "Test":
            //        for (int i = 0; i < Test.Length;i++) {
            //            if (Test[i] == port.node) {
            //                Test[i] = null;
            //                break;
            //            }
            //        }
            //        break;
            //    case "EnterProcess":
            //        EnterProcess = null;
            //        break;
            //    case "OutPortProcess" :
            //        OutPortProcess = null;
            //        break;
            //}
        }
    }



    [Serializable]
    public class ClassType {

        public string className;

        public string Namespace;

        public BProcessItem item;
    }
}
