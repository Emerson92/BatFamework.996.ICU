using System.Collections;
using System.Collections.Generic;
using System.Text;
using THEDARKKNIGHT.Network.UdpSocket;
using THEDARKKNIGHT.Network.UdpSocket.Component;
using UnityEngine;
namespace THEDARKKNIGHT.Example.UdpSocket
{

    public class UdpSocketServerTest : MonoBehaviour
    {
        UdpSocketServerMgr UdpServer;

        public string IPAddress;

        public int ListernPort;

        public int SenderPort;

        // Use this for initialization
        void Start()
        {
            UdpServer = new UdpSocketServerMgr(IPAddress, ListernPort, SenderPort);
            UdpServer.MessageKeeper = new MessageKeeper(new UMessageSolver());
            UdpServer.MessageSend = new MessageSender();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P)) {
                UdpServer.MessageSend.SendMsg(Encoding.UTF8.GetBytes("Hello world!"));
            }
        }
    }

}
