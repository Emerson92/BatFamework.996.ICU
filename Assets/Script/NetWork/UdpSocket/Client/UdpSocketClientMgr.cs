using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.Network.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.Network.UdpSocket
{

    public class UdpSocketClientMgr : UdpSocketClient
    {

        private INetworkComp comp;

        private HeartbeatSolver Heartbeat;

        private IReceviceDataKeeper keep;

        private IMessageSender send;

        public IReceviceDataKeeper MessageKeeper {
            set {
                keep = value;
            }
            get {
                return keep;
            }
        }

        public IMessageSender MessageSend {
            set {
                send = value;
                send.SetMessageSend(SendMsg);
            }
            get {
                return send;
            }
        }

        public INetworkComp Comp {
            set {
                comp = value;
            }
            get {
                return comp;
            }
        }

        /// <summary>
        /// Create the UdpSocketMgr
        /// </summary>
        /// <param name="IP">IP Address</param>
        /// <param name="listernPort">Listern port</param>
        /// <param name="sendPort">Send port</param>
        public UdpSocketClientMgr(string IP, int listernPort, int sendPort) : base(IP, listernPort, sendPort) {

        }


        public void SetHeartbeat(HeartbeatSolver Heartbeat)
        {
            if (Heartbeat != null)
            {
                this.Heartbeat = Heartbeat;
                this.Heartbeat.SetSendMsgAuthority(SendMsg);
            }
        }

        public override void InitSuccess(string IPAddress)
        {
            if (comp != null) comp.InitSuccess(IPAddress);
        }

        public override void ReceviceData(byte[] data, int length, string IPAddress)
        {
            if (comp != null) data = comp.ReceviceData(data, length, IPAddress);
            if (keep != null) keep.MessageDataRecevice(data, length, IPAddress);
        }

        public override byte[] SendData(byte[] data)
        {
            if (comp != null) data = comp.Send(data);
            return data;
        }

        public override void Dispose()
        {
            if (comp != null) comp.Dispose();
        }
    }


}
