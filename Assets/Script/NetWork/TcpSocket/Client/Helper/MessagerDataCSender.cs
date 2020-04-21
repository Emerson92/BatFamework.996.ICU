using System;
using System.Collections.Generic;
using System.Linq;
using THEDARKKNIGHT.Log;
using UnityEngine;

namespace THEDARKKNIGHT.Network.TcpSocket.Client
{

    /// <summary>
    /// 消息发送器
    /// </summary>
    public class MessagerDataCSender
    {

        /// <summary>
        /// 发送消息
        /// </summary>
        private Action<byte[], int, int> SendMsgToClient;

        /// <summary>
        /// 消息发送完毕
        /// </summary>
        public Action MsgSendComplete;

        Queue<ByteArray> SendQuene = new Queue<ByteArray>();

        private bool IsClosing = false;

        private HeartbeatSolverClient Heartbeat;

        public MessagerDataCSender()
        {

        }

        public MessagerDataCSender(HeartbeatSolverClient Heartbeat)
        {
            SetHeartbeat(Heartbeat);
        }

        public void SetHeartbeat(HeartbeatSolverClient Heartbeat)
        {
            if (Heartbeat != null)
            {
                this.Heartbeat = Heartbeat;
                this.Heartbeat.SetSendMsgAuthority(SendMsg);
            }
        }

        public Action TellHeartbeatMsg()
        {
            return Heartbeat.HeartbeatFeekback;
        }

        public void SetSendMsgFunction(Action<byte[], int, int> function)
        {
            this.SendMsgToClient = function;
        }

        public void ReConnect(Action connect)
        {
            if (Heartbeat != null) Heartbeat.ReConnect(connect);
        }


        public void ConnectSuccess(string IPAddress)
        {
            IsClosing = false;
            if (Heartbeat != null)
                Heartbeat.StartToSendHeartbeat();
        }


        public void Close(TcpSocketClient.STATE currentState)
        {
            switch (currentState)
            {
                case TcpSocketClient.STATE.ONDESTORYING:

                    if (SendQuene.Count > 0)
                    {
                        IsClosing = true;
                    }
                    else
                    {
                        if (Heartbeat != null)
                        {
                            Heartbeat.Close(currentState);
                            Heartbeat.SetSendMsgAuthority(null);
                            Heartbeat = null;
                        }
                        if (MsgSendComplete != null) MsgSendComplete();
                    }

                    break;
            }
        }

        public void SendMsg(byte[] body)
        {
            if (IsClosing) return;
            lock (SendQuene)
            {
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
                SendQuene.Enqueue(buffer);
                ByteArray sendbuffer = SendQuene.First();//先出队列
                if (SendMsgToClient != null) SendMsgToClient(sendbuffer.Bytes, sendbuffer.ReadIdx, sendbuffer.Length);
            }
        }

        /// <summary>
        /// 发送消息回馈函数
        /// </summary>
        /// <param name="ar"></param>
        public virtual void SendCallback(int senderCount)
        {
            lock (SendQuene)
            {
                if (SendQuene.Count > 0)
                {
                    if (CheckAndSendData(senderCount))
                    {
                        if (SendQuene.Count <= 0) return;
                        ByteArray sendbuffer = SendQuene.First();
                        if (SendMsgToClient != null) SendMsgToClient(sendbuffer.Bytes, sendbuffer.ReadIdx, sendbuffer.Length);
                    }
                }
                if (IsClosing && SendQuene.Count <= 0)
                {
                    if (Heartbeat != null)
                    {
                        Heartbeat.Close(TcpSocketClient.STATE.ONDESTORYING);
                        Heartbeat.SetSendMsgAuthority(null);
                        Heartbeat = null;
                    }
                    if (MsgSendComplete != null) MsgSendComplete();
                }
            }
        }

        public bool CheckAndSendData(int senderCount)
        {
            ByteArray sendbuffer = SendQuene.First();
            if (sendbuffer == null)
            {
                SendQuene.Dequeue();
                return true;
            }
            /////检查并发送当前buffer中剩余的数据
            sendbuffer.ReadIdx += senderCount;
            if (sendbuffer.Length > 0)
            {
                if (SendMsgToClient != null) SendMsgToClient(sendbuffer.Bytes, sendbuffer.ReadIdx, sendbuffer.Length);
                return false;
            }
            else
            {
                SendQuene.Dequeue();
                /////继续发送消息队列里的信息
                return true;
            }
        }

    }
}
