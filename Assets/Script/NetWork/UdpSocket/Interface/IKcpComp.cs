using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.Network.Interface {
    public interface IKcpComp
    {

        void InitSuccess(string IPAddress, uint listernPort, uint sendPort);

        byte[] ReceviceData(byte[] data, int length, string IPAddress);

        byte[] Send(byte[] data);

        void Dispose();
    }
}
