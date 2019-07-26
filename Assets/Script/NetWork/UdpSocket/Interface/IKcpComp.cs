using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
namespace THEDARKKNIGHT.Network.Interface {

    public interface IKcpComp
    {

        void Init(uint localID,uint RemoteID);

        void ReceviceData(byte[] data, int offset, int length, string IPAddress);

        byte[] Send(byte[] data);

        void Dispose();

        void SetMsgCallback(Action<byte[]> data);

        void SetMsgSendFunction(Action<IntPtr, int> data);

        void OnError(Action<SocketError> error);

        bool GetWaitsnd();
    }

}
