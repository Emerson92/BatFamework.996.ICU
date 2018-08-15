using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT
{

    /// <summary>
    /// 程序生命周期控制器
    /// </summary>
    public class LifeCycleControl : BatSingletion<LifeCycleControl>
    {
        private LifeCycleControl() { }

        public static Dictionary<int, LifeCycleTool> ToolKeepDic = new Dictionary<int, LifeCycleTool>();

        private static List<LifeCycleTool> ItemList = new List<LifeCycleTool>();

        public static void Add(LifeCycleTool i)
        {
            ItemList.ForEach((LifeCycleTool item) =>
            {
                if (item == i)
                {
                    i = null;
                    return;
                }
            });
            if (i != null)
                ItemList.Add(i);
            Sort();
        }

        public static void Sort() {
            QuickSort(ItemList,0, ItemList.Count - 1 );
        }

        public static void QuickSort(List<LifeCycleTool> list, int left, int right) {
            if (left < right)
            {
                int i = Division(list, left, right);
                QuickSort(list, i + 1, right);
                QuickSort(list, left, i - 1);
            }
        }

        private static int Division(List<LifeCycleTool> list, int left, int right)
        {
            while (left < right)
            {
                int num = list[left].priority;
                if (num > list[left + 1].priority)
                {
                    LifeCycleTool Temp = list[left];
                    list[left] = list[left + 1];
                    list[left + 1] = Temp;
                    left++;
                }
                else
                {
                    LifeCycleTool temp = list[right];
                    list[right] = list[left + 1];
                    list[left + 1] = temp;
                    right--;
                }
            }
            return left;
        }

        public static void Remove(LifeCycleTool i)
        {
            ItemList.Remove(i);
        }

        private void Start(MonoBehaviour enter)
        {
            LoopSend((LifeCycleTool i) =>
            {
                if (i.GetLifeCycleState(LifeCycleTool.LifeType.Start))
                    i.Icycle.BStart(enter);
            });
        }

        private void Awake(MonoBehaviour enter)
        {
            LoopSend((LifeCycleTool i) =>
            {
                if (i.GetLifeCycleState(LifeCycleTool.LifeType.Awake))
                    i.Icycle.BAwake(enter);
            });
        }

        public void Update(MonoBehaviour enter)
        {
            LoopSend((LifeCycleTool i) =>
            {
                if (i.GetLifeCycleState(LifeCycleTool.LifeType.Update) && i.Frame > 0)
                    i.Icycle.BUpdate(enter);
            });
        }

        public void FixedUpdate(MonoBehaviour enter) {
            LoopSend((LifeCycleTool i) =>
            {
                if (i.Frame == 0)
                {
                    Awake(enter);
                    Start(enter);
                    i.Frame++;
                }
                if (i.GetLifeCycleState(LifeCycleTool.LifeType.FixedUpdate) && i.Frame > 0)
                    i.Icycle.BFixedUpdate(enter);
            });
        }

        public void LateUpdate(MonoBehaviour enter) {
            LoopSend((LifeCycleTool i) =>
            {
                if (i.GetLifeCycleState(LifeCycleTool.LifeType.LateUpdate) && i.Frame > 0)
                    i.Icycle.BLateUpdate(enter);
            });
        }

        public void OnEnable(MonoBehaviour enter) {
            LoopSend((LifeCycleTool i) =>
            {
                if (i.GetLifeCycleState(LifeCycleTool.LifeType.OnEnable))
                    i.Icycle.BOnEnable(enter);
            });
        }

        public void OnDisable(MonoBehaviour enter) {
            LoopSend((LifeCycleTool i) =>
            {
                if (i.GetLifeCycleState(LifeCycleTool.LifeType.OnDisable))
                    i.Icycle.BDisable(enter);
            });
        }

        public void OnDestory(MonoBehaviour enter) {
            LoopSend((LifeCycleTool i) =>
            {
                if (i.GetLifeCycleState(LifeCycleTool.LifeType.OnDestroy))
                    i.Icycle.BOnDestory(enter);
            });
        }

        public void OnApplicationFocus(MonoBehaviour enter) {
            LoopSend((LifeCycleTool i) =>
            {
                if (i.GetLifeCycleState(LifeCycleTool.LifeType.OnApplicationFocus))
                    i.Icycle.BOnApplicationFocus(enter);
            });
        }

        public void BOnApplicationPause(MonoBehaviour enter) {
            LoopSend((LifeCycleTool i) =>
            {
                if (i.GetLifeCycleState(LifeCycleTool.LifeType.OnApplicationPause))
                    i.Icycle.BOnApplicationPause(enter);
            });
        }

        public void OnApplicationQuit(MonoBehaviour enter) {
            LoopSend((LifeCycleTool i) =>
            {
                if (i.GetLifeCycleState(LifeCycleTool.LifeType.OnApplicationQuit))
                    i.Icycle.BOnApplicationQuit(enter);
            });
        }

        private void LoopSend(Action<LifeCycleTool> fuction)
        {
            ItemList.ForEach((LifeCycleTool i) =>
            {
                fuction(i);
            });
        }
    }

    public class LifeCycleTool
    {

        Dictionary<LifeType, bool> LifeStatue = new Dictionary<LifeType, bool>() {
            { LifeType.Awake,false},
            { LifeType.Start,false},
            { LifeType.Update,false},
            { LifeType.FixedUpdate,false},
            { LifeType.LateUpdate,false},
            { LifeType.OnApplicationFocus,false},
            { LifeType.OnApplicationPause,false},
            { LifeType.OnApplicationQuit,false},
            { LifeType.OnDestroy,false},
            { LifeType.OnDisable,false},
            { LifeType.OnEnable,false}
        };

        public enum LifeType
        {
            Awake,
            Start,
            Update,
            FixedUpdate,
            LateUpdate,
            OnApplicationFocus,
            OnApplicationPause,
            OnApplicationQuit,
            OnDestroy,
            OnDisable,
            OnEnable
        }

        public ILifeCycle Icycle;

        public int priority;

        public int Frame = 0;

        public LifeCycleTool SetLifeCycle(LifeType type,bool excute) {
            LifeStatue[type] = excute;
            return this;
        }

        public bool GetLifeCycleState(LifeType type) {
            return LifeStatue[type];
        }
    }
}
