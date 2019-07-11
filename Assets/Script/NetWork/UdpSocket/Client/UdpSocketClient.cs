using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using THEDARKKNIGHT.Log;

namespace THEDARKKNIGHT.Network.UdpSocket
{

    public abstract class UdpSocketClient
    {
        public int ListenNum = 10;

        public StateObject SocketClient;

        private List<string> CloseClients = new List<string>();

        private EndPoint sendIPAddress;
        private EndPoint listernAddress;

        public UdpSocketClient(string IP, int listernPort,int sendPort)
        {
            StateObjectPool.Instance().CreateStateObjectPool(ListenNum);
            InitUdpSocket(IP, listernPort, sendPort);
        }

        private void InitUdpSocket(string IP, int listernPort,int sendPort)
        {
            Socket socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sendIPAddress = CreateRemoteIP(IP, sendPort);
            listernAddress = CreateRemoteIP(IP, listernPort);
            socketClient.Bind(listernAddress);
            SocketClient = StateObjectPool.Instance().OutQuene();
            SocketClient.workSocket = socketClient;
            InitSuccess(sendIPAddress.ToString());
            EndPoint tempRemoteEP = new IPEndPoint(IPAddress.Any, 0);
            socketClient.BeginReceiveFrom(SocketClient.buffer, 0, StateObject.BufferSize, SocketFlags.None, ref tempRemoteEP, new AsyncCallback(ReadCallback), SocketClient);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            EndPoint receiver = new IPEndPoint(IPAddress.Any, 0);
            int bytesRead = handler.EndReceiveFrom(ar,ref receiver);
            //BLog.Instance().Log("bytesRead :" + bytesRead + " receiver :"+ receiver.ToString());
            if (bytesRead > 0)
            {
                ReceviceData(state.buffer, bytesRead, receiver.ToString());
            }
            EndPoint tempRemoteEP = new IPEndPoint(IPAddress.Any, 0);
            handler.BeginReceiveFrom(state.buffer, 0, StateObject.BufferSize, 0,ref tempRemoteEP, new AsyncCallback(ReadCallback), state);
        }

        public void SendMsg(byte[] msg)
        {
            try
            {
                Send(SocketClient.workSocket, msg);
            }
            catch(Exception ex)
            {
                BLog.Instance().Log(ex);
            }
        }

        private void Send(Socket handler, string data)
        {
            byte[] byteData = SendData(Encoding.UTF8.GetBytes(data));
            handler.BeginSend(byteData, 0, byteData.Length, 0, null, handler);
        }

        private void Send(Socket handler, byte[] data)
        {
            byte[] byteData = SendData(data);
            handler.SendTo(byteData, sendIPAddress);
        }

        private EndPoint CreateRemoteIP(string ipAddress, int port)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            return endPoint;
        }

        public void CloseClient()
        {
            try
            {
                if (SocketClient != null) {
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

        public abstract void InitSuccess(string IPAddress);

        public abstract void ReceviceData(byte[] data, int length, string IPAddress);

        public abstract byte[] SendData(byte[] data);

        public abstract void Dispose();
    }

}
