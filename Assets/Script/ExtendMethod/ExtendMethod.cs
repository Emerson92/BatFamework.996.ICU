using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT {
    public static class ExtendMethod{
        
        /// <summary>
        /// 启用生命周期
        /// </summary>
        /// <param name="i"></param>
        public static void Enable(this ILifeCycle i) {
            LifeCycleControl.Add(i);
        }

        /// <summary>
        /// 关闭生命周期
        /// </summary>
        /// <param name="i"></param>
        public static void Disable(this ILifeCycle i) {
            LifeCycleControl.Remove(i);
        }
    }
}
