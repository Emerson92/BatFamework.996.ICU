using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using THEDARKKNIGHT.Log;
using UnityEngine;

namespace THEDARKKNIGHT.Network.TcpSocket.Server
{

    /// <summary>
    /// 消息发送器
    /// </summary>
    public class MessagerDataSSender
    {

        /// <summary>
        /// 发送消息
        /// </summary>
        private Action<byte[], int, int> SendMsgToAll;

        /// <summary>
        /// 发送消息
        /// </summary>
        private Action<byte[], int, int, string> SendMsgToClient;

        /// <summary>
        /// 消息发送完毕
        /// </summary>
        public Action MsgSendComplete;

        /// <summary>
        /// 消息发送队列
        /// </summary>
        Dictionary<string, MsgQuene> SendQuene = new Dictionary<string, MsgQuene>();

        private bool IsClosing = false;

        private HeartbeatSolverServer Heartbeat;

        public MessagerDataSSender()
        {

        }

        public MessagerDataSSender(HeartbeatSolverServer Heartbeat)
        {
            SetHeartbeat(Heartbeat);
        }

        public void SetHeartbeat(HeartbeatSolverServer Heartbeat)
        {
            if (Heartbeat != null)
            {
                this.Heartbeat = Heartbeat;
                this.Heartbeat.SetSendMsgAuthority(SendMsg);
                //this.Heartbeat.StartToSendHeartbeat();
            }
        }

        public Action<string> GetHearbeatFeekbackPtr()
        {
            return Heartbeat.HeartbeatFeekback;
        }

        public void SetSendMsgFunction(Action<byte[], int, int> function)
        {
            this.SendMsgToAll = function;
        }

        public void SetSendMsgToOneFunction(Action<byte[], int, int, string> function)
        {
            this.SendMsgToClient = function;
        }

        /// <summary>
        /// 自动断开连接
        /// </summary>
        /// <param name="connect"></param>
        public void AutoDisConnect(Action<string> connect)
        {
            if (Heartbeat != null) Heartbeat.AutoDisConnect(connect);
        }

        /// <summary>
        /// 新的客户端加入
        /// </summary>
        /// <param name="IPAddress"></param>
        public void NewClientConnected(string IPAddress)
        {
            //新建对应的消息队列
            if (!SendQuene.ContainsKey(IPAddress))
            {
                MsgQuene quene = new MsgQuene();
                quene.Isabandon = false;
                SendQuene.Add(IPAddress, quene);
            }
            else {
                SendQuene[IPAddress].Isabandon = false;
            }
            this.Heartbeat.StartToSendHeartbeat(IPAddress);
            //需要把连接信息通知应用上层
        }


        public void Close(TcpSocketServer.NetWorkLife currentState)
        {
            switch (currentState)
            {
                case TcpSocketServer.NetWorkLife.Destorying:
                    ///
                    IsClosing = true;
                    CheckAllQueneIsClean();
                    break;
            }
        }

        public void SendMsg(byte[] body)
        {
            if (IsClosing) return;

            ///组装消息体 消息头 + 消息体
            Int16 headLength = (Int16)body.Length;
            byte[] headbytes = BitConverter.GetBytes(headLength);
            if (BitConverter.IsLittleEndian)//小端
            {
                if (!TcpSocketClientMgr.TransportLittleEndian) headbytes.Reverse();
            }
            else
            {
                //大端
                if (TcpSocketClientMgr.TransportLittleEndian) headbytes.Reverse();
            }
            byte[] msg = headbytes.Concat(body).ToArray();
            ByteArray buffer = new ByteArray(msg);
            lock (SendQuene)
            {
                foreach (KeyValuePair<string, MsgQuene> item in SendQuene)
                {
                    if (item.Value.Isabandon) continue;
                    item.Value.SendQuene.Enqueue(buffer);
                    ByteArray sendbuffer = item.Value.SendQuene.First();//先出队列
                    if (SendMsgToAll != null) SendMsgToClient(sendbuffer.Bytes, sendbuffer.ReadIdx, sendbuffer.Length, item.Key);
                }

            }
        }

        public void SendMsg(byte[] body, string IPAddress)
        {
            if (IsClosing) return;
            if (!SendQuene.ContainsKey(IPAddress)) return;
            ///组装消息体 消息头 + 消息体
            Int16 headLength = (Int16)body.Length;
            byte[] headbytes = BitConverter.GetBytes(headLength);
            if (BitConverter.IsLittleEndian)//小端
            {
                if (!TcpSocketClientMgr.TransportLittleEndian) headbytes.Reverse();
            }
            else
            {
                //大端
                if (TcpSocketClientMgr.TransportLittleEndian) headbytes.Reverse();
            }
            lock (SendQuene)
            {
                byte[] msg = headbytes.Concat(body).ToArray();
                ByteArray buffer = new ByteArray(msg);
                SendQuene[IPAddress].SendQuene.Enqueue(buffer);
                ByteArray sendbuffer = SendQuene[IPAddress].SendQuene.First();//先出队列
                if (SendMsgToAll != null) SendMsgToClient(sendbuffer.Bytes, sendbuffer.ReadIdx, sendbuffer.Length, IPAddress);
            }
        }


        /// <summary>
        /// 发送消息回馈函数
        /// </summary>
        /// <param name="ar"></param>
        public virtual void SendCallback(int sendCount, string IP)
        {
            try
            {
                lock (SendQuene)
                {
                    if (SendQuene.ContainsKey(IP))
                    {
                        Queue<ByteArray> dataQuene = SendQuene[IP].SendQuene;
                        if (dataQuene.Count > 0)
                        {
                            if (CheckAndSendData(sendCount, dataQuene))
                            {
                                if (dataQuene.Count <= 0) return;
                                ByteArray sendbuffer = dataQuene.First();
                                if (SendMsgToAll != null) SendMsgToAll(sendbuffer.Bytes, sendbuffer.ReadIdx, sendbuffer.Length);
                            }
                        }
                        if (IsClosing && dataQuene.Count <= 0)
                        {
                            /////需要处理当服务端主动断开连接的时候，需要发送完剩余的数据
                            //SendQuene[IP].IsQueneClean = true;
                            CheckAllQueneIsClean();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        private void CheckAllQueneIsClean()
        {
            bool checkResult = true;
            foreach (KeyValuePair<string, MsgQuene> item in SendQuene)
            {
                if (item.Value.SendQuene.Count > 0) checkResult = false;
            }
            if (checkResult)
            {
                if (Heartbeat != null)
                {
                    Heartbeat.Close(TcpSocketServer.NetWorkLife.Destorying);
                    Heartbeat.SetSendMsgAuthority(null);
                    Heartbeat = null;
                }
                if (MsgSendComplete != null) MsgSendComplete();
            }

        }

        public bool CheckAndSendData(int senderCount, Queue<ByteArray> quene)
        {
            ByteArray sendbuffer = quene.First();
            if (sendbuffer == null)
            {
                quene.Dequeue();
                return true;
            }
            /////检查并发送当前buffer中剩余的数据
            sendbuffer.ReadIdx += senderCount;
            if (sendbuffer.Length > 0)
            {
                if (SendMsgToAll != null) SendMsgToAll(sendbuffer.Bytes, sendbuffer.ReadIdx, sendbuffer.Length);
                return false;
            }
            else
            {
                quene.Dequeue();
                /////继续发送消息队列里的信息
                return true;
            }
        }

        public void LostConnect(string ip) {
            lock (SendQuene) {
                MsgQuene quene;
                if (SendQuene.TryGetValue(ip,out quene)) {
                    quene.Isabandon = true;
                    quene.SendQuene.Clear();
                }
            }
        }

        public class MsgQuene
        {

            public bool Isabandon;

            public Queue<ByteArray> SendQuene = new Queue<ByteArray>();
        }

    }
}
