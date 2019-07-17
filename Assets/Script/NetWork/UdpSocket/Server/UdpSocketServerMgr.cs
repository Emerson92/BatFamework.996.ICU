using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.Network.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.Network.UdpSocket {

    public class UdpSocketServerMgr : UdpSocketServer
    {

        private IKcpComp comp;

        private HeartbeatSolver Heartbeat;

        private IReceviceDataKeeper keep;

        private IMessageSender send;

        public IReceviceDataKeeper MessageKeeper
        {
            set
            {
                keep = value;

            }
            get
            {
                return keep;
            }
        }

        public IMessageSender MessageSend
        {
            set
            {
                send = value;
                send.SetMessageSend(SendMsg);
            }
            get
            {
                return send;
            }
        }

        public IKcpComp Comp
        {
            set
            {
                comp = value;
            }
            get
            {
                return comp;
            }
        }


        public UdpSocketServerMgr(string IP, int listernPort, int sendPort) : base(IP, listernPort, sendPort) {

        }

        public void SetHeartbeat(HeartbeatSolver Heartbeat)
        {
            if (Heartbeat != null)
            {
                this.Heartbeat = Heartbeat;
                this.Heartbeat.SetSendMsgAuthority(SendMsg);
            }
        }

        public override void InitSuccess(string IPAddress, uint listernPort, uint sendPort)
        {

        }

        public override void ReceviceData(byte[] data, int length, string IPAddress)
        {
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
