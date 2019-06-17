using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.Log;
using THEDARKKNIGHT.ProcessCore;
using THEDARKKNIGHT.ProcessCore.DataStruct.Tree;
using THEDARKKNIGHT.ProcessCore.Graph.Json;
using THEDARKKNIGHT.ProcessCore.Lua;
using UnityEngine;
namespace THEDARKKNIGHT.ProcessCore
{

    public class BProcessTreeCore<T, K> : BProcessCore<T, K> where T : BProcessUnit<K> where K : BProcessItem
    {

        private BProcessTree<T,K> processTree;


        public BProcessTreeCore() {
            processTree = new BProcessTree<T, K>();
        }

        /// <summary>
        /// Add data to Process Tree
        /// </summary>
        public void AddProcessUnitByJson(string data)
        {
            ProcesTreeJson target = JsonUtility.FromJson<ProcesTreeJson>(data);
            LoopAddTheNode(target, processTree.RootTree);
        }

        private void LoopAddTheNode(ProcesTreeJson target, BProcessUnit<K> parent)
        {
            T unit = new BProcessUnit<K>() as T;
            unit.SetUnitTagName(target.Name);
            if (target.IsLuaScript)
            {
                target.SubLuaProcessList.ForEach((SubLuaProcess p) =>
                {
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
                target.SubProcessList.ForEach((SubProcess p) =>
                {
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
            processTree.AddNewTree(parent, unit);
            for (int i = 0; i < target.SubTrees.Count; i++)
            {
                LoopAddTheNode(target.SubTrees[i], unit);
            }
        }

        public void AddProcess(BProcessUnit<K> parent, BProcessUnit<K> value) {
            processTree.AddNewTree(parent, value);
        }

        public void AddProcess(string parentName, BProcessUnit<K> value)
        {
            processTree.AddNewTree(parentName, value);
        }

        /// <summary>
        /// when the node excute finish will excute this function
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="data"></param>
        /// <param name="branch"></param>
        public override void FinishExcuteCallback(T currentNode, object data, string branch)
        {
            
        }

        /// <summary>
        /// Get Current Node
        /// </summary>
        /// <returns></returns>
        public override T GetCurrentNode()
        {
            return (T)processTree.GetCurrentUnit();
        }

        /// <summary>
        /// Get the First Node
        /// </summary>
        /// <returns></returns>
        public override T GetFirstNode()
        {
            return (T)processTree.RootTree;
        }

        /// <summary>
        /// move to the next node
        /// </summary>
        /// <param name="branch"></param>
        public override void MoveToNext(string branch)
        {
            if(!string.IsNullOrEmpty(branch) )
                processTree.MoveToNext(branch);
        }
    }

}
