using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.BatCore;
using UnityEngine;
namespace THEDARKKNIGHT.ThreadHelper
{
    /// <summary>
    ///                             多线程交互类
    ///                             
    /// 多线程交互类，主要用于子线程与主线程的方法回调传递，保证子线程能够调用主线程内部的方法
    /// P.S:在调用ExcutionFunc方法时候，回调函数不一定立即执行，会有一段等待时间，改时间主要由
    /// 几方面决定：
    ///  1.受设备的执行帧率，如果设备性能有限或是在特殊渲染环境下，update执行频率受到帧率影响。
    ///  2.执行形式本身就收到线程队列的影响，有个先后顺序，如果上一个回调过于耗时，将会影响后
    ///  一个回调的执行。
    /// </summary>
    public class ThreadCrossHelper : BatMonoSingletion<ThreadCrossHelper>
    {

        /// <summary>
        /// 回调函数管理字典
        /// </summary>
        Dictionary<string, Action> FuncDic = new Dictionary<string, Action>();

        /// <summary>
        /// 线程回调执行队列
        /// </summary>
        Queue<Action> ExcutionQuenen = new Queue<Action>();

        // Use this for initialization

        // Update is called once per frame
        void Update()
        {
            lock (ExcutionQuenen) {
                if (ExcutionQuenen.Count > 0)
                {
                    Action callback = ExcutionQuenen.Dequeue();
                    if (callback != null)
                        callback();
                    callback = null;
                }
            }
        }

        public void CreatThreadCrossHelp()
        {
            Debug.Log("CreatThreadCrossHelp");
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public string SubscribeFunc(Action callback)
        {
            object token = new object();
            string tokenID = token.GetHashCode().ToString();
            if (!FuncDic.ContainsKey(tokenID))
            {
                FuncDic.Add(tokenID, callback);
            }
            else
            {
                Debug.Log("已经注册相关回调 ：" + tokenID);
            }
            token = null;
            return tokenID;
        }

        /// <summary>
        /// 根据令牌执行相关操作
        /// </summary>
        /// <param name="tokenID"></param>
        public void ExcutionFunc(string tokenID)
        {
            if (FuncDic.ContainsKey(tokenID))
            {
                ExcutionQuenen.Enqueue(FuncDic[tokenID]);
                FuncDic.Remove(tokenID);
            }
            else
            {
                Debug.Log("没有找到注册相关回调 ：" + tokenID);
            }
        }

        /// <summary>
        /// 立马执行回调函数
        /// </summary>
        /// <param name="tokenID"></param>
        public void ExcutionFunc(Action callback)
        {
            if (ExcutionQuenen != null)
            {
                lock (ExcutionQuenen)
                {
                    ExcutionQuenen.Enqueue(callback);
                }
            }
        }
    }
}
