using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.Interface;
using UnityEngine;
namespace THEDARKKNIGHT {
    public static class ExtendMethod{
        
        /// <summary>
        /// Set up Life Cycle 
        /// </summary>
        /// <param name="i"></param>
        public static LifeCycleTool Enable(this ILifeCycle i) {
            LifeCycleTool tool = new LifeCycleTool()
            {
                priority = 0,
                Icycle = i,
            }.SetLifeCycle(LifeCycleTool.LifeType.Awake, true);
            int index = i.GetHashCode();
            Debug.Log(" Enable " + index);
            if (LifeCycleControl.ToolKeepDic.ContainsKey(index)) LifeCycleControl.ToolKeepDic.Remove(index);
            LifeCycleControl.ToolKeepDic.Add(index, tool);
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
        /// Close the life cycle tool
        /// </summary>
        /// <param name="i"></param>
        public static void Disable(this ILifeCycle i) {
            LifeCycleTool tool = null;
            int index = i.GetHashCode();
            Debug.Log(" Disable " + index);
            if (LifeCycleControl.ToolKeepDic.TryGetValue(index,out tool)) {
                LifeCycleControl.ToolKeepDic.Remove(index);
            }
            if (tool != null) LifeCycleControl.RecycleList.Add(tool);
        }
    }
}
