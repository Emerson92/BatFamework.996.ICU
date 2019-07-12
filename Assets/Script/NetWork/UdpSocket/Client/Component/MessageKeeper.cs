using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using THEDARKKNIGHT.Network.Interface;
using THEDARKKNIGHT.Network.UdpSocket;
using THEDARKKNIGHT.Network.UdpSocket.Protocl;
using UnityEngine;
namespace THEDARKKNIGHT.Network.UdpSocket.Component {

    public class MessageKeeper : IReceviceDataKeeper
    {
        private uint currentID;

        private IKcpComp comp;

        public void MessageDataRecevice(byte[] data, int length, string IPAddress)
        {
            Debug.Log(Encoding.UTF8.GetString(data));
            byte flag = data[0];
            uint serverID = 0;
            uint clientID = 0;
            switch (flag) {
                case KcpProtocalType.SYN://Connecting msg
                    serverID = BitConverter.ToUInt32(data, 1);
                    break;
                case KcpProtocalType.ACK://Connecting Ack
                    serverID = BitConverter.ToUInt32(data, 1);
                    clientID = BitConverter.ToUInt32(data, 5);
                    UdpSocketClientMgr.ISCONNECTED = true;
                    break;
                case KcpProtocalType.FIN://Finish Connecting
                    serverID = BitConverter.ToUInt32(data, 1);
                    clientID = BitConverter.ToUInt32(data, 5);
                    UdpSocketClientMgr.ISCONNECTED = false;
                    break;
                case KcpProtocalType.MSG://message Conent
                    serverID = BitConverter.ToUInt32(data, 1);
                    clientID = BitConverter.ToUInt32(data, 5);
                    break;
            }
        }

        public void SetKcpComponent(IKcpComp comp)
        {
            this.comp = comp;
        }

        public void Dispose()
        {

        }

        public void SetCurrentID(uint ID)
        {
            this.currentID = ID;
        }
    }

}
