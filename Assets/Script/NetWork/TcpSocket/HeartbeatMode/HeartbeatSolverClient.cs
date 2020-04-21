using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using THEDARKKNIGHT.Log;
using UnityEngine;
namespace THEDARKKNIGHT.Network.TcpSocket.Client
{

    public class HeartbeatSolverClient
    {

        Action Reconnect;

        public Action<byte[]> SendMsgFunction;

        private byte[] HeartbeatMsg;

        private int Period = 1000;

        private int DelayTime = 0;

        private Timer HeartbeatTimer;

        private int CheckTick = 10;

        public HeartbeatSolverClient()
        {

        }

        public void HeartbeatFeekback() {
            RestCheckTick();
        }

        private void RestCheckTick() {
            Debug.Log("RestCheckTick");
            this.CheckTick = 10;
        }

        public void ReConnect(Action callback) {
            this.Reconnect = callback;
        }

        public HeartbeatSolverClient SetCheckTick(int time) {
            this.CheckTick = time;
            return this;
        }

        public HeartbeatSolverClient SendPeriod(int period)
        {
            this.Period = period * 1000;
            return this;
        }

        public HeartbeatSolverClient SetHeartbeatMsg(string msg)
        {
            byte[] body = Encoding.UTF8.GetBytes(msg);
            MsgTalk t = new MsgTalk();
            t.MsgType = 0;
            t.Msgbody = msg;
            this.HeartbeatMsg = Encode(t);
            return this;
        }

        public void SetSendMsgAuthority(Action<byte[]> authority)
        {
            this.SendMsgFunction = authority;
        }

        public HeartbeatSolverClient SetDelayTime(int timer) {
            this.DelayTime = timer;
            return this;
        }


        public void StartToSendHeartbeat() {
            HeartbeatTimer = new Timer(SendHeartbeatMsg, null , DelayTime, Period);
        }

        public virtual void SendHeartbeatMsg(object state)
        {
          
            try
            {
                Debug.Log("SendHeartbeatMsg ：" + CheckTick);
                if (CheckTick-- <= 0)
                {
                    ////判断已经失去连接,重新连接
                    if (Reconnect != null) Reconnect();
                    RestCheckTick();
                    if (HeartbeatTimer != null)
                        HeartbeatTimer.Dispose();
                }
                else { 
                    if (SendMsgFunction != null)SendMsgFunction(HeartbeatMsg);else BLog.Instance().Error("HeartbeatSolver SendMsgFunction is NULL!");
                }
                
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

        public void Close(TcpSocket.TcpSocketClientMgr.STATE currentState) {
            //SendMsgFunction = null;
            if (HeartbeatTimer != null)
                HeartbeatTimer.Dispose();
            HeartbeatTimer = null;
        }

        public byte[] Encode(ProtoBuf.IExtensible MsgTalk)
        {
            using (var meomry = new System.IO.MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(meomry, MsgTalk);
                return meomry.ToArray();
            }
        }
    }
}
