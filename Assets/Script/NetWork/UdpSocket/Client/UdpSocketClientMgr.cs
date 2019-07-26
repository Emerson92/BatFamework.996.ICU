using Microsoft.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using THEDARKKNIGHT.Network.Interface;
using THEDARKKNIGHT.Network.UdpSocket.Protocl;
using UnityEngine;
namespace THEDARKKNIGHT.Network.UdpSocket
{

    public class UdpSocketClientMgr : UdpSocketClient
    {

        private uint ID;

        private IKcpComp kcp;

        private HeartbeatSolver Heartbeat;

        private IReceviceDataKeeper keep;

        private IMessageSender send;

        public IReceviceDataKeeper MessageKeeper {
            set {
                keep = value;
                MessageKeeper.SetCurrentID(ID);
                MessageKeeper.SetConnectStatus(SetConnectStatus);
                MessageKeeper.SetSendComp(MessageSend);
            }
            get {
                return keep;
            }
        }

        public IMessageSender MessageSend {
            set {
                send = value;
                send.SetCurrentID(ID);
                send.SetMessageSend(SendMsg);
                send.SetConnectStatus(SetConnectStatus);  
            }
            get {
                return send;
            }
        }

        public IKcpComp Comp {
            set {
                if (send != null) send.SetKcpComponent(value);
                if (keep != null) keep.SetKcpComponent(value);
                kcp = value;
            }
            get {
                return kcp;
            }
        }

        private CONNECTSTATUS status;

        /// <summary>
        /// Is Coonecting to server
        /// </summary>
        public CONNECTSTATUS ISCONNECTED {
            set {
                send.ConnectStatusChange(status);
                keep.ConnectStatusChange(status);
            }
            get {
                return status;
            }

        }

        public static RecyclableMemoryStreamManager memoryStream;

        /// <summary>
        /// Create the UdpSocketMgr
        /// </summary>
        /// <param name="IP">IP Address</param>
        /// <param name="listernPort">Listern port</param>
        /// <param name="sendPort">Send port</param>
        public UdpSocketClientMgr(string IP, int listernPort) : base(IP, listernPort) {
            ID = (uint)new System.Random().Next(1000, int.MaxValue);
            memoryStream = new RecyclableMemoryStreamManager();
        }

        public void SetHeartbeat(HeartbeatSolver Heartbeat)
        {
            if (Heartbeat != null)
            {
                this.Heartbeat = Heartbeat;
                this.Heartbeat.SetSendMsgAuthority(send.SendMsg);
            }
        }

        public override void InitSuccess(string IPAddress, uint listernPort)
        {
            
        }

        public override void ReceviceData(byte[] data, int length, string IPAddress)
        {
            if (keep != null) keep.MessageDataRecevice(data, length, IPAddress);
        }

        public override byte[] SendData(byte[] data)
        {
            return data;
        }

        public override void Dispose()
        {
            send.Dispose();
            keep.Dispose();
        }

        public override void ConnectToServer(string IP, int sendPort)
        {
            base.ConnectToServer(IP, sendPort);
            send.ConnectToserver(sendIPAddress);
        }

        public void SetConnectStatus(CONNECTSTATUS status) {
            this.ISCONNECTED = status;
        }

    }


}
