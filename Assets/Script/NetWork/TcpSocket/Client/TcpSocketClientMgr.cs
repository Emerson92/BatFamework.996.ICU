using THEDARKKNIGHT.Log;
namespace THEDARKKNIGHT.Network.TcpSocket
{
    public class TcpSocketClientMgr : TcpSocketClient
    {

        /// <summary>
        /// 消息处理器
        /// </summary>
        private ReceviceDataKeeper Keeper;

        /// <summary>
        /// 消息发送器
        /// </summary>
        private MessagerDataSender Messger;


        private HeartbeatSolver Heartbeat;

        public TcpSocketClientMgr(ReceviceDataKeeper Keeper, MessagerDataSender Messger) {
            if (Messger != null)
                this.Keeper = Keeper;
            if (Messger != null) {
                this.Messger = Messger;
                Messger.SetSendMsgFunction(SendMsg);//给予发送消息的权利
            }
        }

        public void SetHeartbeat(HeartbeatSolver Heartbeat)
        {
            if (Heartbeat != null) {
                this.Heartbeat = Heartbeat;
                this.Heartbeat.SetSendMsgAuthority(SendMsg);
            }
        }

        public override void ClientConnectClose(string IPAddress)
        {
            BLog.Instance().Log("ClientConnectClose ：" + IPAddress);
            OnDestoryClient();
        }

        public override void ConnectSuccess(string IPAddress)
        {
            BLog.Instance().Log("ConnectSuccess ："+ IPAddress);
            if (Heartbeat != null)
                Heartbeat.StartToSendHeartbeat();
        }

        public override void ReceviceData(byte[] data, int length, string IPAddress)
        {
            if (Keeper != null)
                Keeper.MessageDataRecevice(data, length, IPAddress);
        }

        public void OnDestoryClient()
        {
            if (Keeper != null)
                Keeper.Close();
            if (Messger != null)
                Messger.Close();
            if (Heartbeat != null)
                Heartbeat.Close();
            CloseClient();
        }
    }
}

