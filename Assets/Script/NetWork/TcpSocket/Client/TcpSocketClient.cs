using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using THEDARKKNIGHT.Log;
using System.Threading;

namespace THEDARKKNIGHT.Network.TcpSocket.Client
{


    public abstract class TcpSocketClient
    {
        public enum STATE
        {
            ONINIT, //初始化
            ONCONNECTED, //连接中
            ONDISCONNECT, //断开连接中
            ONDESTORYING,//销毁连接中
            ONDESTORY//连接关闭中
        }

        public STATE CurrentState = STATE.ONINIT;


        public Socket SocketClient;

        ByteArray readBuffer = new ByteArray();

        protected string IP;

        protected int Port;

        private ManualResetEvent TimeoutObject = new ManualResetEvent(false);
        public TcpSocketClient()
        {

        }

        public void ConnectToServer(string IP, int Port)
        {
            CurrentState = STATE.ONINIT;
            this.IP = IP;
            this.Port = Port;
            try
            {
                SocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                EndPoint IPAddress = CreateRemoteIP(IP, Port);
                IAsyncResult statue = SocketClient.BeginConnect(IPAddress, OnSuccessConnected, SocketClient);
                if (TimeoutObject.WaitOne(5000, false))
                {
                    if (CurrentState != STATE.ONCONNECTED)
                        ConnectTimeout();
                }
                else {
                    ConnectTimeout();
                }
             
            }
            catch (Exception ex) {
                Debug.Log(ex);
            }
        }

        private void OnSuccessConnected(IAsyncResult ar)
        {
            Socket ConnectSocket = ((Socket)ar.AsyncState);
            Debug.Log("LocalIP:" + ConnectSocket.LocalEndPoint.ToString());
            ConnectSuccess(ConnectSocket.RemoteEndPoint.ToString());
            TimeoutObject.Set();
            ConnectSocket.BeginReceive(readBuffer.Bytes, readBuffer.WriteIdx, readBuffer.Remain, 0, ReadCallback, ConnectSocket);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                if (SocketClient == null) return;
                Socket socket = (Socket)(ar.AsyncState);
                int bytesRead = socket.EndReceive(ar);
                if (bytesRead > 0)
                {
                    readBuffer.WriteIdx += bytesRead;
                    if (readBuffer.Remain < 8)
                    {
                        readBuffer.MoveBytes();
                        readBuffer.Resize(readBuffer.Length * 2);
                    }
                    ReceviceCallback(readBuffer, bytesRead, socket.RemoteEndPoint.ToString());
                    socket.BeginReceive(readBuffer.Bytes, readBuffer.WriteIdx, readBuffer.Remain, 0, ReadCallback, socket);
                }
                else
                {
                    BLog.Instance().Log("连接已经断开:");
                    ClientLostConnect(socket.RemoteEndPoint.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }

        }

        protected abstract void ConnectSuccess(string IPAddress);

        protected abstract void ReceviceCallback(ByteArray data, int length, string IPAddress);

        protected abstract void SendCallback(int senderCount);

        protected abstract void ClientLostConnect(string IPAddress);

        protected abstract void ConnectTimeout();

        protected void SendMsg(byte[] msg, int from, int count)
        {
            try
            {
                Send(SocketClient, msg, from, count);
            }
            catch
            {
                ClientLostConnect(SocketClient.RemoteEndPoint.ToString());
            }
        }

        protected void SendMsg(byte[] msg)
        {
            try
            {
                Send(SocketClient, msg);
            }
            catch
            {
                ClientLostConnect(SocketClient.RemoteEndPoint.ToString());
            }
        }




        private void Send(Socket handler, byte[] data)
        {
            if (handler != null) handler.BeginSend(data, 0, data.Length, 0, OnSendCallback, handler);
        }

        private void Send(Socket handler, byte[] data, int from, int count)
        {
            if (handler != null) handler.BeginSend(data, 0, data.Length, 0, OnSendCallback, handler);
        }


        private void OnSendCallback(IAsyncResult ar)
        {
            Socket sender = (Socket)(ar.AsyncState);
            if (sender != null && sender.Connected) { 
                int count = sender.EndSend(ar);
                SendCallback(count);
            }
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
                if (SocketClient != null)
                {
                    SocketClient.Shutdown(SocketShutdown.Both);
                    SocketClient.Close();
                }
                SocketClient = null;
                CurrentState = STATE.ONDESTORY;
            }
            catch (Exception ex)
            {
                BLog.Instance().Log(ex.Message);
            }
        }
    }
}

