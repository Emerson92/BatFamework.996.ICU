using System;
namespace THEDARKKNIGHT.Network.TcpSocket
{

    /// <summary>
    /// 消息发送器
    /// </summary>
    public class MessagerDataSender
    {

        Action<byte[]> SendMsgToClient;


        public MessagerDataSender() {

        }

        public void SetSendMsgFunction(Action<byte[]> function) {
            this.SendMsgToClient = function;
        }

        public void Close() {

        }

        public void SendMsg(byte[] msg) {
            if (SendMsgToClient != null)
                SendMsgToClient(msg);
        }

    }
}
