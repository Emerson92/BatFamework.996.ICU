using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;
namespace THEDARKKNIGHT.TcpSocket
{

    /// <summary>
    /// 消息发送器
    /// </summary>
    public class MessagerDataSender
    {

        Action<string> SendMsgToClient;


        public MessagerDataSender() {

        }

        public void SetSendMsgFunction(Action<string> function) {
            this.SendMsgToClient = function;
        }

        public void Close() {

        }

        public void SendMsg(string msg) {
            if (SendMsgToClient != null)
                SendMsgToClient(msg);
        }

    }
}
