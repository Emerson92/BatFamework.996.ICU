using System;
using System.Collections;
using System.Collections.Generic;
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

        private LinkedList<T> ProcessList = new LinkedList<T>();

        private LinkedListNode<T> CurrentNode { set; get; }

        private int CurrentIndex = 0;

        private Action<string, object> ProcessUnitStartCallback;

        private Func<string, object,bool> ProcessUnitFinishCallback;

        public void SetProcessUnitStartCallback(Action<string, object> call) {
            this.ProcessUnitStartCallback = call;
        }
        public void SetProcessUnitFinishCallback(Func<string, object,bool> call)
        {
            this.ProcessUnitFinishCallback = call;
        }

        public BProcessUnit<K> GetProcessUnitAtIndex(int index) {
            LinkedListNode<T> tempNode = ProcessList.First;
            for (int i = 0; i < index ; i++)
            {
                tempNode = tempNode.Next;
            }
            return tempNode.Value;
        }

        public void AddProcessUnit(T unit) {
            ProcessList.AddLast(unit);
        }

        public void AddProcessUnitAfter(T unitNode,T newUnit) {
            LinkedListNode<T> node= ProcessList.Find(unitNode);
            ProcessList.AddAfter(node, newUnit);
        }

        public void AddProcessUnitBefore(T unitNode, T newUnit) {
            LinkedListNode<T> node = ProcessList.Find(unitNode);
            ProcessList.AddBefore(node, newUnit);
        }

        public void AddProcessLinkedListAfter(T unitNode, LinkedList<T> newList) {
            LinkedListNode<T> node = ProcessList.Find(unitNode);
            ProcessList.AddBefore(node, newList.First);
        }

        public void AddProcessLinkOnCount(int num, LinkedList<T> newUnit) {
            if (CurrentNode == null)
                return;
            LinkedListNode<T> tempNode = CurrentNode;
            for (int i = 0; i < num; i++)
            {
                tempNode = tempNode.Next;
            }
            ProcessList.AddAfter(tempNode, newUnit.First);
        }

        public void AddProcessUnitOnCount(int num, T newUnit) {
            if (CurrentNode == null)
                return;
            LinkedListNode<T> tempNode = CurrentNode;
            for (int i = 0; i < num; i++) {
                tempNode = tempNode.Next;
            }
            ProcessList.AddAfter(tempNode, newUnit);
        }

        public void AddProcessUnitOnIndex(int index, T newUnit) {
            LinkedListNode<T> tempNode = ProcessList.First;
            for (int i = 0; i < index; i++)
            {
                tempNode = tempNode.Next;
            }
            ProcessList.AddAfter(tempNode, newUnit);
        }

        public void AddProcessUnitOnIndex(int index, LinkedListNode<T> newUnit)
        {
            LinkedListNode<T> tempNode = ProcessList.First;
            for (int i = 0; i < index; i++){
                tempNode = tempNode.Next;
            }
            ProcessList.AddAfter(tempNode, newUnit);
       }

        public void AddProcessLinkOnIndex(int index, LinkedList<T> newUnit)
        {
            LinkedListNode<T> tempNode = ProcessList.First;
            for (int i = 0; i < index; i++)
            {
                tempNode = tempNode.Next;
            }
            ProcessList.AddAfter(tempNode, newUnit.First);
        }

        public void RemoveProcessUnitOnCount(int num) {
            if (CurrentNode == null)
                return;
            LinkedListNode<T> tempNode = CurrentNode;
            for (int i = 0; i < num; i++)
            {
                tempNode = tempNode.Next;
            }
            ProcessList.Remove(tempNode);
        }

        public void RemoveProcessUnitOnIndex(int index) {
            LinkedListNode<T> tempNode = ProcessList.First;
            for (int i = 0; i < index; i++)
            {
                Debug.Log(tempNode.Next.Value.UnitTagName);
                tempNode = tempNode.Next;
            }
            if (tempNode != null)
                ProcessList.Remove(tempNode);
        }

        public void RemoveAllProcessUnitOnIndex(int index) {
            for (int i = 0 ; i < ProcessList.Count - index +1; i++)
            {
                ProcessList.Remove(ProcessList.Last);
            }
        }

        public void RemoveAllProcessUnitOnCount(int num) {
            for (int i = 0; i < ProcessList.Count - (CurrentIndex+num) + 1; i++)
            {
                ProcessList.Remove(ProcessList.Last);
            }
        }

        public void StartProcess(object data = null ,LinkedListNode<T> node = null) {
            
            if (node == null)
            {
                LinkedListNode<T> firstNode = ProcessList.First;
                CurrentNode = firstNode;
                CurrentIndex = 0;
            }
            else
                CurrentNode = node;
            T processUnit = CurrentNode.Value;
            processUnit.DataInit(data);
            if (ProcessUnitStartCallback != null)
                ProcessUnitStartCallback(processUnit.UnitTagName, data);
            processUnit.ProcessUnitReadyToGO = ProcessUnitPrepareComplete;
            processUnit.ProcessUnitFinishExcution = ProcessUnitFinish;
        }

        private void ProcessUnitPrepareComplete()
        {
            CurrentNode.Value.ProcessExcute();
        }

        private void ProcessUnitFinish(object data)
        {
            bool moveNext = true;
            CurrentNode.Value.ProcessUnitReadyToGO -= ProcessUnitPrepareComplete;
            CurrentNode.Value.ProcessUnitFinishExcution -= ProcessUnitFinish;
            if (ProcessUnitFinishCallback != null)
                moveNext = ProcessUnitFinishCallback(CurrentNode.Value.UnitTagName, data);
            Debug.Log("ProcessUnitFinish");
            if (CurrentNode.Next != null && moveNext) {
                StartProcess(data, CurrentNode.Next);
                CurrentIndex++;
            }
        }
    }
}
