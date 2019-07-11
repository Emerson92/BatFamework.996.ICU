using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.Network.Interface {
    public interface INetworkComp
    {

        void InitSuccess(string IPAddress);

        byte[] ReceviceData(byte[] data, int length, string IPAddress);

        byte[] Send(byte[] data);

        void Dispose();
    }
}
