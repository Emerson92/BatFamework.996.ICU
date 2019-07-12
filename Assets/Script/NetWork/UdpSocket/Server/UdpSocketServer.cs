using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using THEDARKKNIGHT.Log;
using UnityEngine;
namespace THEDARKKNIGHT.Network.UdpSocket
{

    public abstract class UdpSocketServer
    {
        public int ListenNum = 10;

        public StateObject SocketClient;

        private Dictionary<string,EndPoint> clientDic = new Dictionary<string,EndPoint>();

        private EndPoint sendIPAddress;
        private EndPoint listernAddress;

        public UdpSocketServer(string IP, int listernPort, int sendPort)
        {
            StateObjectPool.Instance().CreateStateObjectPool(ListenNum);
            InitUdpSocket(IP, listernPort, sendPort);
        }

        private void InitUdpSocket(string IP, int listernPort, int sendPort)
        {
            Socket socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sendIPAddress = CreateRemoteIP(IP, sendPort);
            listernAddress = CreateRemoteIP(IP, listernPort);
            socketClient.Bind(listernAddress);
            SocketClient = StateObjectPool.Instance().OutQuene();
            SocketClient.workSocket = socketClient;
            InitSuccess(IP, (uint)listernPort, (uint)sendPort);
            EndPoint tempRemoteEP = new IPEndPoint(IPAddress.Any, 0);
            socketClient.BeginReceiveFrom(SocketClient.buffer, 0, StateObject.BufferSize, SocketFlags.None, ref tempRemoteEP, new AsyncCallback(ReadCallback), SocketClient);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            EndPoint receiver = new IPEndPoint(IPAddress.Any, 0);
            int bytesRead = handler.EndReceiveFrom(ar, ref receiver);
            //BLog.Instance().Log("bytesRead :" + bytesRead + " receiver :"+ receiver.ToString());
            if (receiver != null && !clientDic.ContainsKey(receiver.ToString())) clientDic.Add(receiver.ToString(), receiver);
            if (bytesRead > 0)
            {
                ReceviceData(state.buffer, bytesRead, receiver.ToString());
            }
            EndPoint tempRemoteEP = new IPEndPoint(IPAddress.Any, 0);
            handler.BeginReceiveFrom(state.buffer, 0, StateObject.BufferSize, 0, ref tempRemoteEP, new AsyncCallback(ReadCallback), state);
        }

        public void SendMsg(byte[] msg)
        {
            try
            {
                Debug.Log("Server SendMsg");
                SendToAll(msg);
            }
            catch (Exception ex)
            {
                BLog.Instance().Log(ex);
            }
        }

        private void Send(Socket handler, string data, EndPoint ipAddress)
        {
            byte[] byteData = SendData(Encoding.UTF8.GetBytes(data));
            handler.SendTo(byteData, sendIPAddress);
        }

        private void Send(Socket handler, byte[] data, EndPoint ipAddress)
        {
            byte[] byteData = SendData(data);
            handler.SendTo(byteData, ipAddress);
        }

        private void SendToAll(byte[] data)
        {
            foreach (EndPoint item in clientDic.Values) {
                Send(SocketClient.workSocket, data, item);
            }
        }

        private EndPoint CreateRemoteIP(string ipAddress, int port)
        {
            IPEndPoint endPoint = new IPEndPoint(ipAddress!= null?IPAddress.Parse(ipAddress): IPAddress.Any, port);
            return endPoint;
        }

        public void CloseClient()
        {
            try
            {
                if (SocketClient != null)
                {
                    SocketClient.workSocket.Close();
                    SocketClient.workSocket = null;
                    StateObjectPool.Instance().EnQuene(SocketClient);
                }
            }
            catch (Exception ex)
            {
                BLog.Instance().Log(ex);
            }
        }

        public abstract void InitSuccess(string IPAddress, uint listernPort, uint sendPort);

        public abstract void ReceviceData(byte[] data, int length, string IPAddress);

        public abstract byte[] SendData(byte[] data);

        public abstract void Dispose();
    }
}
