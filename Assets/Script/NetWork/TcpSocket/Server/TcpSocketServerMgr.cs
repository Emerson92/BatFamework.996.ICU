
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace THEDARKKNIGHT.TcpSocket
{

    /// <summary>
    /// Socket管理类
    /// </summary>
    public class TcpSocketServerMgr : TcpSocketServer
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

        public TcpSocketServerMgr(ReceviceDataKeeper Keeper = null, MessagerDataSender Messger = null) {
            if (Keeper != null)
                this.Keeper = Keeper;
            if (Messger != null) {
                this.Messger = Messger;
                Messger.SetSendMsgFunction(SendToAll);//给予发送消息的权利
            }
        }
        public void SetHeartbeat(HeartbeatSolver Heartbeat)
        {
            if (Heartbeat != null)
            {
                this.Heartbeat = Heartbeat;
                this.Heartbeat.SetSendMsgAuthority(SendToAll);
            }
        }

        /// <summary>
        /// 获取消息发送器
        /// </summary>
        /// <returns></returns>
        public MessagerDataSender GetSendAssist() {
            return Messger;
        }

        public override void ConnectSuccess(string IPAddress, StateObject state)
        {
            if (Heartbeat != null)
                Heartbeat.StartToSendHeartbeat();
        }

        public override void ReceviceData(byte[] data, int length,string IPAddress)
        {
            if(Keeper!=null)
                Keeper.MessageDataRecevice(data, length, IPAddress);
        }

        public override void ClientConnectClose(StateObject state)
        {
            
        }

        public void OnDestoryServer() {
            if(Keeper != null )
                Keeper.Close();
            if(Messger != null )
                Messger.Close();
            if (Heartbeat != null)
                Heartbeat.Close();
            CloseServer();
        }
    }
}
