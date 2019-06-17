using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.Log;
using THEDARKKNIGHT.ProcessCore.DataStruct;
using THEDARKKNIGHT.ProcessCore.Graph.Json;
using THEDARKKNIGHT.ProcessCore.Interface;
using THEDARKKNIGHT.ProcessCore.Lua;
using UnityEngine;
namespace THEDARKKNIGHT.ProcessCore
{
    /// <summary>
    ///  This class mainly to manage all the Process,it has two components BProcessUnit and BProcessItem
    ///  BProcessUnit is a unit to manage the task or step,it can contain many BProcessItem components.
    ///  BProcessItem is a task or step.
    /// </summary>
    /// <typeparam name="T">it is subclass extend BProcessUit class</typeparam>
    /// <typeparam name="K">it is subclass extend BProcessItem class</typeparam>
    public abstract class BProcessCore<T, K> where T : BProcessUnit<K> where K : BProcessItem
    {

        protected T CurrentNode;

        private Action<string, object> ProcessUnitStartCallback;

        private Func<string, object, bool> ProcessUnitFinishCallback;

        public BProcessCore()
        {
            
        }

        public void SetProcessUnitStartCallback(Action<string, object> call)
        {
            this.ProcessUnitStartCallback = call;
        }

        public void SetProcessUnitFinishCallback(Func<string, object, bool> call)
        {
            this.ProcessUnitFinishCallback = call;
        }


        public void StartProcess(object data = null, T node = null)
        {

            if (node == null)
            {
                T firstNode = GetFirstNode();
                CurrentNode = firstNode;
            }
            else
                CurrentNode = node;
            T processUnit = CurrentNode;
            processUnit.ProcessUnitReadyToGO = ProcessUnitPrepareComplete;
            processUnit.ProcessUnitFinishExcution = ProcessUnitFinish;
            processUnit.DataInit(data);
            if (ProcessUnitStartCallback != null)
                ProcessUnitStartCallback(processUnit.UnitTagName, data);
        }

        private void ProcessUnitPrepareComplete()
        {
            CurrentNode.ProcessExcute();
        }

        private void ProcessUnitFinish(object data, string branch)
        {
            bool moveNext = true;
            CurrentNode.ProcessUnitReadyToGO -= ProcessUnitPrepareComplete;
            CurrentNode.ProcessUnitFinishExcution -= ProcessUnitFinish;
            if (ProcessUnitFinishCallback != null)
                moveNext = ProcessUnitFinishCallback(CurrentNode.UnitTagName, data);
            FinishExcuteCallback(CurrentNode, data, branch);
            if (moveNext)
            {
                Debug.Log("ProcessUnitFinish");
                MoveToNext(branch);
                T currentNode = GetCurrentNode();
                if (currentNode != null) {
                    StartProcess(data, currentNode);
                }
            }
        }

        public abstract T GetFirstNode();

        public abstract void MoveToNext(string branch);

        public abstract T GetCurrentNode();

        public abstract void FinishExcuteCallback(T currentNode,object data , string branch);
    }
}
