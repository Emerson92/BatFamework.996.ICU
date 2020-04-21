using System;
using System.Net;
using THEDARKKNIGHT.Log;
using THEDARKKNIGHT.Network.TcpSocket.Client;
using UnityEngine;

namespace THEDARKKNIGHT.Network.TcpSocket
{
    public class TcpSocketClientMgr : TcpSocketClient
    {

        public Action<string> LostServerConnected;

        /// <summary>
        /// 使用小端方式进行通讯
        /// </summary>
        public static bool TransportLittleEndian = true;

        /// <summary>
        /// 消息处理器
        /// </summary>
        private ReceviceDataCKeeper Keeper;

        /// <summary>
        /// 消息发送器
        /// </summary>
        private MessagerDataCSender Sender;

        public TcpSocketClientMgr(ReceviceDataCKeeper Keeper, MessagerDataCSender Messger)
        {
            if (Keeper != null)
                this.Keeper = Keeper;
            if (Messger != null)
            {
                this.Sender = Messger;
                Messger.SetSendMsgFunction(SendMsg);//给予发送消息的权利
                Messger.MsgSendComplete = MsgSendCompletCallback;///消息发送完毕通知
                Messger.ReConnect(OnReconnect);///重新建立连接
                if(Keeper != null) Keeper.ReceivedFeedbackMsg = Messger.TellHeartbeatMsg();///收到消息回复通知心跳管理器
            }
            CurrentState = STATE.ONINIT;
        }

        /// <summary>
        /// 心跳超时重新建立连接
        /// </summary>
        private void OnReconnect()
        {
            BLog.Instance().Log("心跳超时重新建立连接：");
            CloseClient();
            ConnectToServer(IP,Port);
        }

        /// <summary>
        /// 获取消息发送器
        /// </summary>
        /// <returns></returns>
        public MessagerDataCSender GetSendAssist()
        {
            return Sender;
        }

        public void Send(byte[] msg) {
            if (Sender != null) Sender.SendMsg(msg);
        }

        private void MsgSendCompletCallback()
        {
            BLog.Instance().Log("消息队列发送完毕，现在关闭连接");
            base.CloseClient();
            CurrentState = STATE.ONDESTORY;
            Sender.MsgSendComplete = null;
            Sender = null;
        }



        protected override void ClientLostConnect(string IPAddress)
        {
            CurrentState = STATE.ONDISCONNECT;
            BLog.Instance().Log("ClientConnectClose ：" + IPAddress);
            base.CloseClient();
            if (LostServerConnected != null) LostServerConnected(IPAddress);
        }

        protected override void ConnectSuccess(string IPAddress)
        {
            CurrentState = STATE.ONCONNECTED;
            BLog.Instance().Log("ConnectSuccess ：" + IPAddress);
            if (Sender != null) Sender.ConnectSuccess(IPAddress);
        }

        protected override void ReceviceCallback(ByteArray data, int length, string IPAddress)
        {
            if (Keeper != null)
                Keeper.MessageDataRecevice(data, length, IPAddress);
        }


        protected override void SendCallback(int count)
        {
            if (Sender != null)
                Sender.SendCallback(count);
        }


        public void Close()
        {
            BLog.Instance().Log("关闭连接");
            CurrentState = STATE.ONDESTORYING;
            if (Keeper != null)
                Keeper.Close(CurrentState);
            if (Sender != null)
                Sender.Close(CurrentState);
            Keeper = null;
        }

        protected override void ConnectTimeout()
        {
            Debug.Log("连接超时");
        }
    }
}

