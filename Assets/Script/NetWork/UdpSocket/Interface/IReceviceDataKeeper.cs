using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.Network.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.Network.UdpSocket
{

    public interface IReceviceDataKeeper
    {
        void SetCurrentID(uint ID);

        void MessageDataRecevice(byte[] data, int length, string IPAddress);

        void SetKcpComponent(IKcpComp comp);

        void Dispose();
    }

}
