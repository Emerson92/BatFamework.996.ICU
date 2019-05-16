using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.ProcessCore;
using UnityEngine;
using XNode;
namespace THEDARKKNIGHT.ProcessCore.Graph {

    public class ProcessItem : XNode.Node
    {
        //[Input]
        //public ProcessItem[] Test;

        [Input]
        public ProcessItem[] EnterProcess;

        [SerializeField, SetProperty("RedefineNodeProproty")]
        public string BranchID = null;
        
        [SerializeField]
        public string[] SubBranchID;

        private string BranchParent;

        [Output]
        public ProcessItem[] OutPortProcess;

        public ClassType[] ProcessItems;

        public object RedefineNodeProproty {
            set {
                UpdateBranchParent();
            }
        }

        public new void OnEnable()
        {
            base.OnEnable();
            EnterProcess = new ProcessItem[1];
            OutPortProcess = new ProcessItem[1];
        }


        /// <summary>
        /// Update all the Node branch Parent
        /// </summary>
        private void UpdateBranchParent()
        {
            ////TODO
            FindBranchParent();////Find Parent Branch 
            CheckBackwardChild();////Check Child Statues
        }

        private void FindBranchParent()
        {
            for (int i = 0 ; i < EnterProcess.Length ; i++) {
                if (EnterProcess[i] == null) continue; 
                if (string.IsNullOrEmpty(EnterProcess[i].BranchID))
                {
                    this.BranchParent = EnterProcess[i].name;
                }
                else {
                    if(EnterProcess[i].BranchID == this.BranchID)
                    this.BranchParent = EnterProcess[i].BranchParent;
                }
            }
        }

        private void CheckBackwardChild()
        {
            for (int i = 0; i < OutPortProcess.Length;i++ )
            {
                if (OutPortProcess[i] == null) continue;
                    CheckBackwardNode(OutPortProcess[i], this);
            }
        }

        private void CheckBackwardNode(ProcessItem node, ProcessItem lastNode)
        {
            if (node == null || lastNode == null) return;
            if (node.BranchID == lastNode.BranchID && !string.IsNullOrEmpty(node.BranchID)) {
                node.BranchParent = lastNode.BranchParent;
                foreach (ProcessItem item in node.OutPortProcess) {
                    CheckBackwardNode(item, node);
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

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            base.OnCreateConnection(from ,to);
            if (from.node == this)
            {
                if (OutPortProcess != null) {
                    for (int i = 0; i < OutPortProcess.Length; i++)
                    {
                        if (OutPortProcess[i] == null)
                        {
                            OutPortProcess[i] = to.node as ProcessItem;
                            break;
                        }
                        else if (i == OutPortProcess.Length - 1)
                        {
                            foreach (NodePort p in OutPortProcess[i].Outputs)
                            {
                                from.Disconnect(p);
                            }
                            OutPortProcess[i] = to.node as ProcessItem;
                            break;
                        }
                    }
                }
            }
            else {
                if (EnterProcess != null) {
                    for (int i = 0; i < EnterProcess.Length; i++)
                    {
                        if (EnterProcess[i] == null)
                        {
                            EnterProcess[i] = from.node as ProcessItem;
                            break;
                        }
                        else if (i == EnterProcess.Length - 1)
                        {
                            foreach (NodePort p in EnterProcess[i].Outputs)
                            {
                                p.Disconnect(to);
                            }
                            EnterProcess[i] = from.node as ProcessItem;
                            break;
                        }
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
            base.OnRemoveConnection(output, input);
            if (output.node == this)
            {
                if (OutPortProcess != null)
                {
                    for (int i = 0; i < OutPortProcess.Length; i++)
                    {
                        if (OutPortProcess[i] == input.node)
                        {
                            OutPortProcess[i] = null;
                            break;
                        }
                    }
                }
            }
            else {
                if (EnterProcess != null) {
                    for (int i = 0; i < EnterProcess.Length; i++)
                    {
                        if (EnterProcess[i] == output.node)
                        {
                            EnterProcess[i] = null;
                            break;
                        }
                    }
                }
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
