using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT {
    public static class ExtendMethod{
        
        /// <summary>
        /// 启用生命周期
        /// </summary>
        /// <param name="i"></param>
        public static LifeCycleTool Enable(this ILifeCycle i) {
            LifeCycleTool tool = new LifeCycleTool()
            {
                priority = 0,
                Icycle = i,
            }.SetLifeCycle(LifeCycleTool.LifeType.Awake, true);
            LifeCycleControl.ToolKeepDic.Add(i.GetHashCode(), tool);
            LifeCycleControl.Add(tool);
            return tool;
        }

        /// <summary>
        /// Gets the life cycle tool.
        /// </summary>
        /// <returns>The LifeCycleTool</returns>
        /// <param name="i"></param>
        public static LifeCycleTool GetLifeCycleTool(this ILifeCycle i){
            return LifeCycleControl.ToolKeepDic.ContainsKey(i.GetHashCode()) == true ? LifeCycleControl.ToolKeepDic[i.GetHashCode()] : null;
        }

        /// <summary>
        /// 关闭生命周期
        /// </summary>
        /// <param name="i"></param>
        public static void Disable(this ILifeCycle i) {
            LifeCycleTool tool = null;
            if (LifeCycleControl.ToolKeepDic.ContainsKey(i.GetHashCode()))
                tool = LifeCycleControl.ToolKeepDic[i.GetHashCode()];
            if(tool != null) LifeCycleControl.Remove(tool);
        }
    }
}
