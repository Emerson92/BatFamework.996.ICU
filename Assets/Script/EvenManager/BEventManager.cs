using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.BatCore;
using UnityEngine;
namespace THEDARKKNIGHT.EventSystem {
    public class BEventManager : BatSingletion<BEventManager>
    {

        Dictionary<string, List<EventParam>> EventManagerDic = new Dictionary<string, List<EventParam>>();

        private BEventManager() { }

        /// <summary>
        /// 添加监听事件
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="dispatchEvent"></param>
        /// <param name="priority"></param>
        public void AddListener(string methodName,Func<object,object> dispatchEvent,int priority = 0) {
            EventParam param = new EventParam(dispatchEvent, priority);
            if (!EventManagerDic.ContainsKey(methodName))
            {
                List<EventParam> receviers = new List<EventParam>();
                receviers.Add(param);
                EventManagerDic.Add(methodName, receviers);
            }
            else {
                List<EventParam> CallbackList = EventManagerDic[methodName];
                CallbackList.Add(param);
            }
            SortEventOrder(EventManagerDic[methodName]);
        }

        internal void AddListener(string lEFTPRESSEVENT, object leftPressCallback)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 更据事件优先级排序
        /// </summary>
        /// <param name="receviers"></param>
        private void SortEventOrder(List<EventParam> receviers)
        {
            QuickSort(receviers,0, receviers.Count-1);
        }

        public static void QuickSort(List<EventParam> list, int left, int right)
        {
            if (left < right)
            {
                int i = Division(list, left, right);
                QuickSort(list, i + 1, right);
                QuickSort(list, left, i - 1);
            }
        }

        private static int Division(List<EventParam> list, int left, int right)
        {
            while (left < right)
            {
                int num = list[left].Priority;
                if (num > list[left + 1].Priority)
                {
                    EventParam Temp = list[left];
                    list[left] = list[left + 1];
                    list[left + 1] = Temp;
                    left++;
                }
                else
                {
                    EventParam temp = list[right];
                    list[right] = list[left + 1];
                    list[left + 1] = temp;
                    right--;
                }
            }
            return left;
        }

        /// <summary>
        /// 派发事件
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="resultCallback"></param>
        public void DispatchEvent(string methodName,object data, Action<object> resultCallback = null) {
            if (EventManagerDic.ContainsKey(methodName)) {
                List <EventParam> receviers = EventManagerDic[methodName];
                receviers.ForEach((EventParam param) => {
                    if (param.DispatchEvent != null) {
                        object result = param.DispatchEvent(data);
                        if (resultCallback != null)
                            resultCallback(result);
                    }
                });
            }
        }

        public struct EventParam {

            public Func<object,object> DispatchEvent;

            public int Priority;

            public EventParam(Func<object, object> dispatchEvent,int priority) {
                this.DispatchEvent = dispatchEvent;
                this.Priority = priority;
            }
        }
    }
}
