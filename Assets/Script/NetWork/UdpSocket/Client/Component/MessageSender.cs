using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Timers;
using THEDARKKNIGHT.Network.Interface;
using THEDARKKNIGHT.Network.UdpSocket.Protocl;
using UnityEngine;
namespace THEDARKKNIGHT.Network.UdpSocket.Component
{

    public class MessageSender : IMessageSender
    {
        private uint currentID;

        private IKcpComp kcpComp;

        private Action<byte[]> callblack;

        private readonly MemoryStream memoryStream;

        private System.Timers.Timer timerCaculator;

        public MessageSender() {
            memoryStream = UdpSocketClientMgr.memoryStream.GetStream("message", ushort.MaxValue);
        }

        public void SendMsg(byte[] msg)
        {
            if (callblack != null) callblack(msg);
        }

        public void SetMessageSend(Action<byte[]> sendMessageFuction)
        {
            this.callblack = sendMessageFuction;
        }

        public void SetKcpComponent(IKcpComp comp)
        {
            this.kcpComp = comp;
        }

        public void ConnectToserver(EndPoint sendIPAddress)
        {
            TryToConnectServer();
            timerCaculator = new System.Timers.Timer();
            timerCaculator.Enabled = true;
            timerCaculator.Interval = 200;
            timerCaculator.Elapsed += new System.Timers.ElapsedEventHandler((object source, ElapsedEventArgs e) => {
                if (UdpSocketClientMgr.ISCONNECTED)///Check if connect is establish
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
            if (timerCaculator != null) {
                timerCaculator.Stop();
                timerCaculator = null;
            }
               
        }
    }

}
