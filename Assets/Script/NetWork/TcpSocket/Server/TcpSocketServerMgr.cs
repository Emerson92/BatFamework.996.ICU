
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using THEDARKKNIGHT.Log;
using UnityEngine;
namespace THEDARKKNIGHT.Network.TcpSocket.Server
{
    /// <summary>
    /// Socket管理类
    /// </summary>
    public class TcpSocketServerMgr : TcpSocketServer
    {

        /// <summary>
        /// 消息处理器
        /// </summary>
        private ReceviceDataSKeeper Keeper;

        /// <summary>
        /// 消息发送器
        /// </summary>
        private MessagerDataSSender Sender;


        public TcpSocketServerMgr(ReceviceDataSKeeper Keeper = null, MessagerDataSSender Sender = null) {
            if (Keeper != null) { 
                this.Keeper = Keeper;
                this.Keeper.SetMessagerFeedback(Sender.SendMsg);
            }
            if (Sender != null) {
                this.Sender = Sender;
                Sender.SetSendMsgFunction(SendToAll);//给予发送消息的权利
                Sender.SetSendMsgToOneFunction(SendToClient);
                Sender.MsgSendComplete = MsgSendCompletCallback;///消息发送完毕通知
                Sender.AutoDisConnect(AutoDisConnection);
                if (Keeper != null) Keeper.ReceivedFeedbackMsg = Sender.GetHearbeatFeekbackPtr();
            }
        }

        /// <summary>
        /// 处理掉线客户端
        /// </summary>
        /// <param name="IPAddress"></param>
        private void AutoDisConnection(string IPAddress)
        {
            Sender.LostConnect(IPAddress);
            DisableClient(IPAddress);
            BLog.Instance().Log("AutoDisConnection ：" + IPAddress);
        }

        private void MsgSendCompletCallback()
        {
            BLog.Instance().Log("消息队列发送完毕，现在关闭连接");
            base.DestoryServer();
            TcpLifeCycle = NetWorkLife.Destory;
            Sender.MsgSendComplete = null;
            Sender = null;
        }


        /// <summary>
        /// 获取消息发送器
        /// </summary>
        /// <returns></returns>
        public MessagerDataSSender GetSendAssist() {
            return Sender;
        }

        public void Send(byte[] msg)
        {
            if (Sender != null) Sender.SendMsg(msg);
        }

        protected override void NewClientConnected(string IPAddress, StateObject state)
        {
            BLog.Instance().Log("ConnectSuccess ：" + IPAddress);
            if (Sender != null) Sender.NewClientConnected(IPAddress);
        }


        protected override void ReceviceData(ByteArray data, int length, string IPAddress)
        {
            if (Keeper != null) Keeper.MessageDataRecevice(data, length, IPAddress);
        }

        protected override void SendCallback(int sendCount, string IPAddress)
        {
            if (Sender != null)
                Sender.SendCallback(sendCount, IPAddress);
        }


        protected override void ClientLoseConnect(StateObject state)
        {
            
        }

        public void Close() {
            base.CloseServer();
            if (Keeper != null)
                Keeper.Close(TcpLifeCycle);
            if (Sender != null)
                Sender.Close(TcpLifeCycle);
        }


    }
}
