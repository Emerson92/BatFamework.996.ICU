using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using THEDARKKNIGHT.Log;
using UnityEngine;
namespace THEDARKKNIGHT.Network.TcpSocket.Server
{

    public class HeartbeatSolverServer
    {

        Action<string> AutoDisconnect;

        public Action<byte[]> SendMsgFunction;

        public byte[] HeartbeatMsg;

        private int Period = 1;

        private int DelayTime = 0;

        private Timer HeartbeatTimer;

        public int TickDefault = 60;

        //public Dictionary<string, CheckTickTime> checkTickDic = new Dictionary<string, CheckTickTime>();

        public Dictionary<string, int> checkTickDic = new Dictionary<string, int>();

        public HeartbeatSolverServer()
        {

        }

        public void HeartbeatFeekback(string IPAddress) {
            RestCheckTick(IPAddress);
        }

        private void RestCheckTick(string IPAddress) {
            Debug.Log("RestCheckTick");
            lock (checkTickDic) {
                if (checkTickDic.ContainsKey(IPAddress))
                {
                    //checkTickDic[IPAddress].checkTick = TickDefault;
                    checkTickDic[IPAddress] = TickDefault;
                } else
                    Debug.Log("checkTickDic 不包含 ：" + IPAddress);
            }

        }

        public void AutoDisConnect(Action<string> callback) {
            this.AutoDisconnect = callback;
        }

        public HeartbeatSolverServer SetCheckTick(int time) {
            this.TickDefault = time;
            return this;
        }

        public HeartbeatSolverServer SendPeriod(int period)
        {
            this.Period = period * 1000;
            return this;
        }

        public HeartbeatSolverServer SetHeartbeatMsg(string msg)
        {
            byte[] body = Encoding.UTF8.GetBytes(msg);
            this.HeartbeatMsg = body;
            return this;
        }

        public HeartbeatSolverServer SetHeartbeatMsg(byte[] msg)
        {
            this.HeartbeatMsg = msg;
            return this;
        }

        public void SetSendMsgAuthority(Action<byte[]> authority)
        {
            this.SendMsgFunction = authority;
        }

        public HeartbeatSolverServer SetDelayTime(int timer) {
            this.DelayTime = timer;
            return this;
        }


        public void StartToSendHeartbeat(string IPAddress) {
            lock (checkTickDic)
            {
                if (!checkTickDic.ContainsKey(IPAddress))
                {
                    //checkTickDic.Add(IPAddress, new CheckTickTime(TickDefault));
                    checkTickDic.Add(IPAddress, TickDefault);
                }
            }
            if (HeartbeatTimer == null) HeartbeatTimer = new Timer(SendHeartbeatMsg, null, DelayTime, Period);
        }

        public virtual void SendHeartbeatMsg(object state)
        {
            try
            {
                lock (checkTickDic)
                {
                    foreach (KeyValuePair<string, int> item in checkTickDic)
                    //foreach (KeyValuePair<string, CheckTickTime> item in checkTickDic)
                    {
                        BLog.Instance().Log("IPAddress ：" + item.Key + " checkTime :" + item.Value);
                        //BLog.Instance().Log("IPAddress ：" + item.Key + " checkTime :" + checkTickDic[item.Key]);
                        if (checkTickDic[item.Key]-- < 0)
                        {
                            ////判断已经失去连接,重新连接
                            if (AutoDisconnect != null) AutoDisconnect(item.Key);
                            checkTickDic.Remove(item.Key);
                        }
                    }
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

        public void Close(TcpSocketServer.NetWorkLife currentState) {
            SendMsgFunction = null;
            if (HeartbeatTimer != null)
                HeartbeatTimer.Dispose();
            HeartbeatTimer = null;
        }

        public class CheckTickTime{

            public int checkTick;

            public CheckTickTime(int time) {
                this.checkTick = time;
            }
        }
    }
}
