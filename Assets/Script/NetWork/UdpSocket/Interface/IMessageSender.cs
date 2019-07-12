using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using THEDARKKNIGHT.Network.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.Network.UdpSocket
{

    public interface IMessageSender
    {
        void SetCurrentID(uint ID);

        void SetMessageSend(Action<byte[]> sendMessageFuction);

        void SendMsg(byte[] msg);

        void SetKcpComponent(IKcpComp comp);

        void Dispose();

        void ConnectToserver(EndPoint sendIPAddress);
    }

}

