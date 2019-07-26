using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using THEDARKKNIGHT.Log;
using THEDARKKNIGHT.Network.Interface;
using THEDARKKNIGHT.Network.UdpSocket.Protocl;
using UnityEngine;
namespace THEDARKKNIGHT.Network.UdpSocket.Component
{

    public class MessageSender : IMessageSender
    {
        private uint currentID;

        private IKcpComp kcpComp;

        private Action<byte[], int> sendMsg;

        private readonly MemoryStream memoryStream;

        private System.Timers.Timer timerCaculator;

        private UdpSocketClient.CONNECTSTATUS currentStatus;

        private Action<UdpSocketClient.CONNECTSTATUS> statusChangeFunction;

        public MessageSender()
        {
            memoryStream = UdpSocketClientMgr.memoryStream.GetStream("message", ushort.MaxValue);
        }

        public void SendMsg(byte[] msg)
        {
            if (kcpComp.GetWaitsnd())
            {
                if (kcpComp != null) kcpComp.Send(msg); else BLog.Instance().Error("MessageSender kcpComp is NULL!");
            }
            else {
                BLog.Instance().Error("MessageSender SendMsg GetWaitsnd is FALSE");
            }
        }

        public void SetMessageSend(Action<byte[], int> sendMessageFuction)
        {
            this.sendMsg = sendMessageFuction;
        }

        public void SetKcpComponent(IKcpComp comp)
        {
            this.kcpComp = comp;
            this.kcpComp.SetMsgSendFunction(StartToSendMsg);
        }

        private void StartToSendMsg(IntPtr bytes, int length)
        {
            byte[] buffer = this.memoryStream.GetBuffer();
            buffer.WriteTo(0, KcpProtocalType.MSG);
            // writer down the connectID;
            buffer.WriteTo(1, this.currentID);
            Marshal.Copy(bytes, buffer, 5, length);
            if (sendMsg != null) sendMsg(buffer, length + 5); else BLog.Instance().Error("MessageSender sendMsg is NULL!");
        }

        public void ConnectToserver(EndPoint sendIPAddress)
        {
            TryToConnectServer();
            timerCaculator = new System.Timers.Timer();
            timerCaculator.Enabled = true;
            timerCaculator.Interval = 200;
            timerCaculator.Elapsed += new System.Timers.ElapsedEventHandler((object source, ElapsedEventArgs e) =>
            {
                if (currentStatus == UdpSocketClient.CONNECTSTATUS.CONNECTED)///Check if connect is establish
                    timerCaculator.Stop();
                else
                    TryToConnectServer();
            });
            timerCaculator.Start();
        }

        /// <summary>
        /// try to send connect package to the Server
        /// </summary>
        private void TryToConnectServer()
        {
            byte[] buffer = this.memoryStream.GetBuffer();
            buffer.WriteTo(0, KcpProtocalType.SYN);
            buffer.WriteTo(1, this.currentID);
            byte[] data = memoryStream.ToArray();
            SendMsg(data);
        }

        public void SetCurrentID(uint ID)
        {
            this.currentID = ID;
        }

        public void Dispose()
        {
            if (timerCaculator != null)
            {
                timerCaculator.Stop();
                timerCaculator = null;
            }

        }

        public void ConnectStatusChange(UdpSocketClient.CONNECTSTATUS status)
        {
            this.currentStatus = status;
        }

        public void SetConnectStatus(Action<UdpSocketClient.CONNECTSTATUS> callback)
        {
            this.statusChangeFunction = callback;
        }
    }

}
