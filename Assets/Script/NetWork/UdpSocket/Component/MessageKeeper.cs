using System.Collections;
using System.Collections.Generic;
using System.Text;
using THEDARKKNIGHT.Network.UdpSocket;
using UnityEngine;
namespace THEDARKKNIGHT.Network.UdpSocket.Component {

    public class MessageKeeper : IReceviceDataKeeper
    {
        public void MessageDataRecevice(byte[] data, int length, string IPAddress)
        {
            Debug.Log(Encoding.UTF8.GetString(data));
        }
    }

}
