using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.Network.UdpSocket
{

    public interface IMessageSender
    {
         void SetMessageSend(Action<byte[]> sendMessageFuction);

         void SendMsg(byte[] msg);
    }

}

