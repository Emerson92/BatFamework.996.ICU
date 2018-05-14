using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT {
    public static class ExtendMethod{
        
        /// <summary>
        /// 启用生命周期
        /// </summary>
        /// <param name="i"></param>
        public static LifeCycleTool Enable(this ILifeCycle i, LifeCycleTool tool) {
            LifeCycleControl.Add(tool);
            return tool;
        }

        /// <summary>
        /// 关闭生命周期
        /// </summary>
        /// <param name="i"></param>
        public static void Disable(this ILifeCycle i, LifeCycleTool tool) {
            LifeCycleControl.Remove(tool);
        }
    }
}
