using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.Network.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.Network.UdpSocket
{

    public interface IReceviceDataKeeper
    {
        void SetCurrentID(uint ID);

        void MessageDataRecevice(byte[] data, int length, string IPAddress);

        void SetSendComp(IMessageSender sender);

        void SetKcpComponent(IKcpComp comp);

        void ConnectStatusChange(UdpSocketClient.CONNECTSTATUS status);

        void SetConnectStatus(Action<UdpSocketClient.CONNECTSTATUS> status);

        void Dispose();
    }

}
