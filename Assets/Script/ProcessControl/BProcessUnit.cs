using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace THEDARKKNIGHT.ProcessCore {
    public class BProcessUnit<T> where T : BProcessItem
    {

        private string unitTagName;

        public string UnitTagName {
            get {
                return unitTagName;
            }
        }

        /// <summary>
        /// it is a list which contain some Parallel process items;
        /// </summary>
        private List<T> ProcessItem = new List<T>();

        public Action<object,string> ProcessUnitFinishExcution;

        public Action ProcessUnitReadyToGO;

        public BProcessUnit(params T[] items) {
            for (int i = 0; i < items.Length; i++) {
                ProcessItem.Add(items[i]);
            }
        }

        public void SetUnitTagName(string name) {
            this.unitTagName = name;
        }

        public void AddItem(T item) {
            ProcessItem.Add(item);
        }

        public void RemoveAt(int index) {
            ProcessItem.RemoveAt(index);
        }

        public virtual void DataInit(object data)
        {
            for (int i = 0; i < ProcessItem.Count; i++)
            {
                ProcessItem[i].Init();
                ProcessItem[i].ProcessItemFinishExcution += ProcessFinish;
                ProcessItem[i].ProcessItemAssetAlready += ProcessReady;
                ProcessItem[i].ProcessStart(data);
            }
        }

        private void ProcessReady(BProcessItem item,object data)
        {
            item.DataInit(data);
            for (int i = 0; i < ProcessItem.Count; i++)
            {
                if (ProcessItem[i].ProcessStatus != BProcessItem.PROCESSSTATUS.Ready)
                    return;
            }
            if (ProcessUnitReadyToGO != null)
                ProcessUnitReadyToGO();
        }

        public virtual void ProcessExcute()
        {
            for (int i = 0; i < ProcessItem.Count; i++)
            {
                ProcessItem[i].ProcessExcute();
            }
        }

        public virtual void ProcessFinish(string branchName,object data,bool IsForceToEnd)
        {
            if(IsForceToEnd){
                AllTaskFinish(branchName,data);
                return;
            }
            for (int i = 0;i< ProcessItem.Count;i++) {
                if (ProcessItem[i].ProcessStatus != BProcessItem.PROCESSSTATUS.Finish)
                    return;
            }
            AllTaskFinish(branchName,data);
        }

        private void AllTaskFinish(string branchName,object data)
        {

            for (int i = 0; i < ProcessItem.Count; i++)
            {
                ProcessItem[i].ProcessDestory();
                ProcessItem[i].ProcessItemFinishExcution -= ProcessFinish;
                ProcessItem[i].ProcessItemAssetAlready -= ProcessReady;
            }
            if (ProcessUnitFinishExcution != null)
                ProcessUnitFinishExcution(data, branchName); 
        }
    }


}


