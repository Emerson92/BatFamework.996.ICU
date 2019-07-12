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
            }
            get {
                return send;
            }
        }

        public IKcpComp Comp {
            set {
                if(send != null) send.SetKcpComponent(value);
                if(keep != null) keep.SetKcpComponent(value);
                kcp = value;
            }
            get {
                return kcp;
            }
        }

        /// <summary>
        /// Is Coonecting to server
        /// </summary>
        public static bool ISCONNECTED = false;

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
                this.Heartbeat.SetSendMsgAuthority(SendMsg);
            }
        }

        public override void InitSuccess(string IPAddress, uint listernPort)
        {
            //if (comp != null) comp.InitSuccess(IPAddress, listernPort, sendPort);
        }

        public override void ReceviceData(byte[] data, int length, string IPAddress)
        {
            //if (comp != null) data = comp.ReceviceData(data, length, IPAddress);
            if (keep != null) keep.MessageDataRecevice(data, length, IPAddress);
        }

        public override byte[] SendData(byte[] data)
        {
            //if (comp != null) data = comp.Send(data);
            return data;
        }

        public override void Dispose()
        {
            //if (comp != null) comp.Dispose();
            send.Dispose();
            keep.Dispose();
        }

        public override void ConnectToServer(string IP, int sendPort)
        {
            base.ConnectToServer(IP, sendPort);
            //using (MemoryStream s = new MemoryStream()) {
            //    byte[] buffer = s.GetBuffer();
            //    buffer.WriteTo(0,KcpProtocalType.SYN);
            //    buffer.WriteTo(1, ID);
            //    return buffer;
            //}
            send.ConnectToserver(sendIPAddress);
        }
    }


}
