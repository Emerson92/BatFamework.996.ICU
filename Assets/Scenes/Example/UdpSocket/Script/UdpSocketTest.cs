using System.Collections;
using System.Collections.Generic;
using System.Text;
using THEDARKKNIGHT.Network.UdpSocket;
using THEDARKKNIGHT.Network.UdpSocket.Component;
using UnityEngine;
namespace THEDARKKNIGHT.Example.UdpSocket
{

    public class UdpSocketTest : MonoBehaviour
    {
        UdpSocketClientMgr UdpClient;

        public string IPAddress;

        public int ListernPort;

        public int SenderPort;

        // Use this for initialization
        void Start()
        {
            UdpClient = new UdpSocketClientMgr(IPAddress, ListernPort, SenderPort);
            UdpClient.MessageKeeper = new MessageKeeper();
            UdpClient.MessageSend = new MessageSender();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P)) {
                UdpClient.MessageSend.SendMsg(Encoding.UTF8.GetBytes("Hello world!"));
            }
        }
    }

}
