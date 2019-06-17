using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.Log;
using THEDARKKNIGHT.ProcessCore.DataStruct;
using THEDARKKNIGHT.ProcessCore.Graph.Json;
using THEDARKKNIGHT.ProcessCore.Interface;
using THEDARKKNIGHT.ProcessCore.Lua;
using UnityEngine;
namespace THEDARKKNIGHT.ProcessCore {

    /// <summary>
    /// Line Process Link
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public class BProcessLinkCore<T, K> : BProcessCore<T, K> where T : BProcessUnit<K> where K : BProcessItem
    {
        public BranchProcessMgr<T, K> BranchMgr = new BranchProcessMgr<T, K>();

        private IProcessForerunner<T, K> ProcessLink;


        public BProcessLinkCore()
        {
            ProcessLink = new BProcessLink<T, K>();
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
                ProcessLink.InsertLinkAtNode(CurrentNode, branchList);
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

        /// <summary>
        /// Parse Json Data to proceess unit
        /// </summary>
        /// <param name="data"></param>
        public void AddProcessUnitByJson(string data)
        {
            ProcessJson target = JsonUtility.FromJson<ProcessJson>(data);
            for (int i = 0; i < target.ProcessList.Count; i++)
            {
                T unit = new BProcessUnit<K>() as T;
                unit.SetUnitTagName(target.ProcessList[i].Name);
                if (target.ProcessList[i].IsLuaScript)
                {
                    target.ProcessList[i].SubLuaProcessList.ForEach((SubLuaProcess p) => {
                        try
                        {
                            K item = new LuaBProcessItem(p.UrlPath, p.Nickname) as K;
                            unit.AddItem(item);
                        }
                        catch (Exception ex)
                        {
                            BLog.Instance().Error(ex.Message);
                        }
                    });
                }
                else
                {
                    target.ProcessList[i].SubProcessList.ForEach((SubProcess p) => {
                        try
                        {
                            Type t = Type.GetType(p.Namespace + "." + p.ClassName);
                            K item = Activator.CreateInstance(t, p.Nickname) as BProcessItem as K;
                            unit.AddItem(item);
                        }
                        catch (Exception ex)
                        {
                            BLog.Instance().Error(ex.Message);
                        }
                    });
                }
                if (string.IsNullOrEmpty(target.ProcessList[i].BranchID))
                {
                    AddProcessUnit(unit);
                }
                else
                {
                    BranchMgr.AddBranchProcess(target.ProcessList[i].BranchParentName, target.ProcessList[i].BranchID, unit);
                }
            }
        }

        public override T GetFirstNode()
        {
            T firstNode = ProcessLink.GetFirstNode();
            ProcessLink.ResetIndicatorToStart();
            return firstNode;
        }

        public override void MoveToNext(string branch)
        {
            ProcessLink.NextProcessItem();
        }

        public override T GetCurrentNode()
        {
            return ProcessLink.GetCurrentProcessItem();
        }

        public override void FinishExcuteCallback(T currentNode, object data, string branch)
        {
            if (!string.IsNullOrEmpty(branch))
            {
                ////TODO Merge Branch process into main process
                ProcessLink.InsertLinkAtNode(CurrentNode, BranchMgr.FindBranchProcess(CurrentNode.UnitTagName, branch));
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
            if (!BranchProcessDic.ContainsKey(currentNodeName))
            {
                BranchProcessDic.Add(currentNodeName, dic);
            }
        }

        /// <summary>
        /// add the new branch Process into Dicnationary
        /// </summary>
        /// <param name="currentNodeName"></param>
        /// <param name="branchName"></param>
        /// <param name="branchUnit"></param>
        public void AddBranchProcess(string currentNodeName, string branchName, T branchUnit = null)
        {
            if (BranchProcessDic.ContainsKey(currentNodeName))
            {
                if (BranchProcessDic[currentNodeName].ContainsKey(branchName))
                {
                    if (branchUnit != null)
                        BranchProcessDic[currentNodeName][branchName].SetProcessItem(branchUnit);
                }
                else
                {
                    IProcessForerunner<T, K> processLink = new BProcessLink<T, K>();
                    if (branchUnit != null)
                        processLink.SetProcessItem(branchUnit);
                    BranchProcessDic[currentNodeName].Add(branchName, processLink);
                }

            }
            else
            {
                Dictionary<string, IProcessForerunner<T, K>> branchDic = new Dictionary<string, IProcessForerunner<T, K>>();
                IProcessForerunner<T, K> processLink = new BProcessLink<T, K>();
                processLink.SetProcessItem(branchUnit);
                branchDic.Add(branchName, processLink);
                BranchProcessDic.Add(currentNodeName, branchDic);
            }
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
