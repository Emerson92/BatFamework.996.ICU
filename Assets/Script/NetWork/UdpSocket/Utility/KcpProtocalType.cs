using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.Network.UdpSocket.Protocl
{
    public static class KcpProtocalType
    {
        public const byte SYN = 1; ///Send Connecting Request 
        public const byte ACK = 2; ///Send Confirmed Connected information
        public const byte FIN = 3; ///Connected is Close
        public const byte MSG = 4; /// this is normal msg
    }
}
