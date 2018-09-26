using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using THEDARKKNIGHT.BatCore;
using UnityEngine;
namespace THEDARKKNIGHT
{

    /// <summary>
    /// 代码执行效率监控器
    /// </summary>
    public class CodeWatcher : BatSingletion<CodeWatcher>
    {
        Stopwatch stopwatch;

        private CodeWatcher() { }

        public void Init() {
            stopwatch = new Stopwatch();
        }

        /// <summary>
        /// 建立监控点
        /// </summary>
        public void SetWatchPoint() {
            stopwatch.Start();
        }

        public void EndWatch() {
            stopwatch.Stop();
        }

        public void Clear() {
            stopwatch.Reset();
        }

        /// <summary>
        /// 获取现在耗时时间(单位为毫秒)
        /// </summary>
        /// <returns></returns>
        public double GetCostSumTime() {
            TimeSpan timespan = stopwatch.Elapsed;
            return timespan.TotalMilliseconds;
        }
    }
}
