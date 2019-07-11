using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.Network.UdpSocket
{

    public interface IReceviceDataKeeper
    {

        void MessageDataRecevice(byte[] data, int length, string IPAddress);

    }

}
