using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.ProcessCore.DataStruct;
using THEDARKKNIGHT.ProcessCore.Interface;
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
        public BranchProcessMgr<T, K> BranchMgr = new BranchProcessMgr<T, K>();

        private T CurrentNode;

        private IProcessForerunner<T, K> ProcessLink;

        private Action<string, object> ProcessUnitStartCallback;

        private Func<string, object, bool> ProcessUnitFinishCallback;

        public BProcessCore()
        {
            ProcessLink = new BProcessLink<T, K>();
        }

        public void SetProcessUnitStartCallback(Action<string, object> call)
        {
            this.ProcessUnitStartCallback = call;
        }

        public void SetProcessUnitFinishCallback(Func<string, object, bool> call)
        {
            this.ProcessUnitFinishCallback = call;
        }

        /// <summary>
        /// Inject the branch process into main process
        /// </summary>
        /// <param name="Current"></param>
        /// <param name="branchName"></param> 
        public void InjectBranchProcess(string Current, string branchName)
        {
            IProcessForerunner<T, K> branchList = BranchMgr.FindBranchProcess(Current, branchName);
            if (branchList != null)
            {
                T firstNode = branchList.GetFirstNode();
                T CurrentNode = ProcessLink.GetCurrentProcessItem();
                T tempNode = branchList.GetCurrentProcessItem();
                ProcessLink.InsertNodeToLink(CurrentNode, tempNode);
                while ( branchList.GetCurrentProcessItem() != null) {
                    T tempNode = branchList.GetCurrentProcessItem();
                    branchList.NextProcessItem();
                    ProcessLink.InsertNodeToLink(CurrentNode, tempNode);
                }
            }
        }

        /// <summary>
        /// Reset the ProcessLinkList
        /// </summary>
        public void RebackLinkList()
        {
            ProcessLink.RebackData();
        }

        public BProcessUnit<K> GetProcessUnitAtIndex(int index)
        {
            return ProcessLink.GetProcessItemAtIndex(index);
        }

        public void AddProcessUnit(T unit)
        {
            ProcessLink.SetProcessItem(unit);
        }

        public void StartProcess(object data = null, T node = null)
        {

            if (node == null)
            {
                T firstNode = ProcessLink.GetFirstNode();
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

        private void ProcessUnitFinish(object data)
        {
            bool moveNext = true;
            CurrentNode.ProcessUnitReadyToGO -= ProcessUnitPrepareComplete;
            CurrentNode.ProcessUnitFinishExcution -= ProcessUnitFinish;
            if (ProcessUnitFinishCallback != null)
                moveNext = ProcessUnitFinishCallback(CurrentNode.UnitTagName, data);
            if (moveNext)
            {
                Debug.Log("ProcessUnitFinish");
                ProcessLink.NextProcessItem();
                T currentNode = ProcessLink.GetCurrentProcessItem();
                if (currentNode != null) {
                    StartProcess(data, currentNode);
                }
            }
        }
    }

    /// <summary>
    /// it is the Process branch Class which designs to help the main Process class to add branch process arrccoding to user choose result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public class BranchProcessMgr<T, K> where T : BProcessUnit<K> where K : BProcessItem
    {

        private Dictionary<string, Dictionary<string, IProcessForerunner<T, K>>> BranchProcessDic = new Dictionary<string, Dictionary<string, IProcessForerunner<T, K>>>();

        public IProcessForerunner<T, K> CreateBranchLink(List<T> nodeList)
        {
            IProcessForerunner<T, K> subLinkedList = new BProcessLink<T, K>();
            nodeList.ForEach((T t) =>
            {
                subLinkedList.SetProcessItem(t);
            });
            return subLinkedList;
        }

        public void SetBranchProcessDic(string currentNodeName, Dictionary<string, IProcessForerunner<T, K>> dic)
        {
            BranchProcessDic.Add(currentNodeName, dic);
        }

        public IProcessForerunner<T, K> FindBranchProcess(string currentNodeName, string branchName)
        {
            Dictionary<string, IProcessForerunner<T, K>> dic;
            if (BranchProcessDic.TryGetValue(currentNodeName, out dic))
            {
                IProcessForerunner<T, K> branchList;
                if (dic.TryGetValue(branchName, out branchList))
                {
                    return branchList;
                }
                else
                    return null;
            }
            else
                return null;
        }
    }
}
