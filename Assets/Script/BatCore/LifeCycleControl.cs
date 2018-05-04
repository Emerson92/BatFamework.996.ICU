using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT
{
    public class LifeCycleControl : BatSingletion<LifeCycleControl>
    {
        private LifeCycleControl() { }

        private static List<ILifeCycle> ItemList = new List<ILifeCycle>();

        public static void Add(ILifeCycle i) {
            ItemList.ForEach((ILifeCycle item) =>
            {
                if (item == i) {
                    i = null;
                    return;
                }
            });
            if (i != null)
               ItemList.Add(i);
        }

        public static void Remove(ILifeCycle i) {
             ItemList.Remove(i);
        }

        public void Start(MonoBehaviour enter)
        {
            LoopSend((ILifeCycle i) => {
                i.BStart(enter);
            });
        }

        public void Awake(MonoBehaviour enter)
        {
            LoopSend((ILifeCycle i) => {
                i.BAwake(enter);
            });
        }

        public void Update(MonoBehaviour enter)
        {
            LoopSend((ILifeCycle i) => {
                i.BUpdate(enter);
            });
        }

        private void LoopSend(Action<ILifeCycle> fuction)
        {
            ItemList.ForEach((ILifeCycle i) =>
            {
                fuction(i);
            });
        }
    }
}
