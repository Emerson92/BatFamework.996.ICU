using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;
namespace THEDARKKNIGHT.Network
{

    public class HeartbeatSolver
    {

        public Action<byte[]> SendMsgFunction;

        private byte[] HeartbeatMsg;

        private int Period;

        private int DelayTime = 0;

        private Timer HeartbeatTimer;

        public HeartbeatSolver()
        {

        }

        public HeartbeatSolver SendPeriod(int period)
        {
            this.Period = period;
            return this;
        }

        public HeartbeatSolver SetHeartbeatMsg(string msg)
        {
            this.HeartbeatMsg = Encoding.UTF8.GetBytes(msg);
            return this;
        }

        public void SetSendMsgAuthority(Action<byte[]> authority)
        {
            this.SendMsgFunction = authority;
        }

        public void SetDelayTime(int timer) {
            this.DelayTime = timer;
        }


        public void StartToSendHeartbeat() {
            HeartbeatTimer = new Timer(SendHeartbeatMsg, null , DelayTime, Period);
        }

        public virtual void SendHeartbeatMsg(object state)
        {
            Debug.Log("SendHeartbeatMsg");
            try
            {
                if (SendMsgFunction != null)
                    SendMsgFunction(HeartbeatMsg);
            }
            catch (Exception ex) {
                SendHeartbeatException(ex);
            }

        }

        /// <summary>
        /// 发送心跳数据异常处理
        /// </summary>
        /// <param name="ex"></param>
        public virtual void SendHeartbeatException(Exception ex) {

        }

        public void Close() {
            if (HeartbeatTimer != null)
                HeartbeatTimer.Dispose();
        }
    }
}
