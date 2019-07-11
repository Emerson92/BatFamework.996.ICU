using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.Network.UdpSocket.Component
{

    public class MessageSender : IMessageSender
    {

        private Action<byte[]> callblack;

        public void SendMsg(byte[] msg)
        {
            if (callblack != null) callblack(msg);
        }

        public void SetMessageSend(Action<byte[]> sendMessageFuction)
        {
            this.callblack = sendMessageFuction;
        }
    }

}
