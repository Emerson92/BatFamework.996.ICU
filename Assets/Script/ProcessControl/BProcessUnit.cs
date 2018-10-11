using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace THEDARKKNIGHT.ProcessCore {
    public class BProcessUnit<T> where T : BProcessItem
    {

        /// <summary>
        /// it is a list which contain some Parallel process items;
        /// </summary>
        private List<T> ProcessItem = new List<T>();

        public Action ProcessUnitFinishExcution;

        public Action ProcessUnitReadyToGO;

        public virtual void AssetCheck()
        {
            for (int i = 0; i < ProcessItem.Count; i++)
            {
                ProcessItem[i].Init();
                ProcessItem[i].AssetCheck();
                ProcessItem[i].ProcessItemFinishExcution += ProcessFinish;
                ProcessItem[i].ProcessItemAssetAlready += ProcessReady;
            }
        }

        private void ProcessReady()
        {
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

        public virtual void ProcessFinish()
        {
            for (int i = 0;i< ProcessItem.Count;i++) {
                if (ProcessItem[i].ProcessStatus != BProcessItem.PROCESSSTATUS.Finish)
                    return;
            }
            AllTaskFinish();
        }

        private void AllTaskFinish()
        {
            if (ProcessUnitFinishExcution != null)
                ProcessUnitFinishExcution();
            for (int i = 0; i < ProcessItem.Count; i++) {
                ProcessItem[i].ProcessItemFinishExcution -= ProcessFinish;
                ProcessItem[i].ProcessItemAssetAlready -= ProcessReady;
            }
                
        }
    }


}


