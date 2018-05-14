using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.EventSystem {
    public class EventManager :BatSingletion<EventManager>{

        Dictionary<string, List<EventParam>> EventManagerDic = new Dictionary<string, List<EventParam>>();

        private EventManager() { }

        /// <summary>
        /// 添加监听事件
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="dispatchEvent"></param>
        /// <param name="priority"></param>
        public void AddListener(string methodName,Func<object,object> dispatchEvent,object data ,int priority = 0) {
            EventParam param = new EventParam(dispatchEvent, data, priority);
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

        /// <summary>
        /// 更据事件优先级排序
        /// </summary>
        /// <param name="receviers"></param>
        private void SortEventOrder(List<EventParam> receviers)
        {
            
        }

        /// <summary>
        /// 派发事件
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="resultCallback"></param>
        public void DispatchEvent(string methodName,Action<object> resultCallback = null) {
            if (EventManagerDic.ContainsKey(methodName)) {
                List <EventParam> receviers = EventManagerDic[methodName];
                receviers.ForEach((EventParam param) => {
                    if (param.DispatchEvent != null) {
                        object result = param.DispatchEvent(param.Data);
                        if (resultCallback != null)
                            resultCallback(result);
                    }
                });
            }
        }

        public struct EventParam {

            public Func<object,object> DispatchEvent;

            public object Data;

            public int Priority;

            public EventParam(Func<object, object> dispatchEvent,object data ,int priority) {
                this.DispatchEvent = dispatchEvent;
                this.Data = data;
                this.Priority = priority;
            }
        }
    }
}
