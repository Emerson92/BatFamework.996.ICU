using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.Network.UdpSocket
{

    public interface IMessageSolver
    {

        void MessageSolver(byte[] date, int offset, int length);

    }
}
