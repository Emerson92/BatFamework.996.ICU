using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using THEDARKKNIGHT.Log;
namespace THEDARKKNIGHT.TcpSocket
{

    public abstract class TcpSocketServer
    {
        public enum NetWorkLife
        {
            None,
            Create,
            Connecting,
            Connected,
            Disconnected,
            Destory
        }

        public static NetWorkLife TcpLifeCycle;

        private ManualResetEvent allDone;

        private int ListenNum;

        private Dictionary<string, StateObject> ClientDic = new Dictionary<string, StateObject>();

        private List<string> CloseClients = new List<string>();

        private Thread SocketListenThread;

        Socket severSocket = null;

        public TcpSocketServer()
        {
            TcpLifeCycle = NetWorkLife.None;
            ListenNum = 5;
            allDone = new ManualResetEvent(false);
            CreateStateObjectPool();
        }

        private void CreateStateObjectPool()
        {
            StateObjectPool.Instance().CreateStateObjectPool (ListenNum);
        }

        public void StartSever(string IP, int port = 50015)
        {
            SocketListenThread = new Thread(() =>
            {
                IPAddress ipAddress = IPAddress.Parse(IP);
                IPEndPoint endPoint = new IPEndPoint(ipAddress, port);
                severSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                severSocket.Bind(endPoint);
                severSocket.Listen(ListenNum);
                BLog.Instance().Log("IP地址为 :" + IP + " 端口为 :" + port + " 服务已建立");
                try
                {
                    while (TcpLifeCycle != NetWorkLife.Destory)
                    {
                        allDone.Reset();
                        BLog.Instance().Log("等待客服端连接");
                        TcpLifeCycle = NetWorkLife.Connecting;
                        if(severSocket!=null)
                            severSocket.BeginAccept(new AsyncCallback(AcceptCallback), severSocket);
                        allDone.WaitOne();
                    }
                }
                catch (ThreadAbortException ex)
                {
                    BLog.Instance().Log("Socket监听线程关闭 :"+ex.Message);
                    return;
                }
                catch (Exception e)
                {
                    BLog.Instance().Log(e.Message);
                }
                BLog.Instance().Log("Socket监听线程正常退出");
            });
            SocketListenThread.Start();
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            TcpLifeCycle = NetWorkLife.Connected;
            allDone.Set();
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            StateObject state = StateObjectPool.Instance().OutQuene();
            state.workSocket = handler;
            ClientDic.Add(handler.RemoteEndPoint.ToString(), state);
            ConnectSuccess(handler.RemoteEndPoint.ToString(), state);
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            int bytesRead = handler.EndReceive(ar);
            BLog.Instance().Log("bytesRead :"+ bytesRead);
            if (bytesRead > 0)
            {
                ReceviceData(state.buffer, bytesRead, handler.RemoteEndPoint.ToString());
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            }
            else
            {
                TcpLifeCycle = NetWorkLife.Disconnected;
                BLog.Instance().Log("连接已经断开:");
                ClientConnectClose(state);
                ClientDic.Remove(state.workSocket.RemoteEndPoint.ToString());
                StateObjectPool.Instance().EnQuene(state);
            }
           
        }

        public abstract void ConnectSuccess(string IPAddress, StateObject state);

        public abstract void ReceviceData(byte[] data, int length, string IPAddress);

        public abstract void ClientConnectClose(StateObject state);

        public void SendToAll(String data)
        {
            CloseClients.Clear();
            foreach (KeyValuePair<string, StateObject> item in ClientDic)
            {
                try
                {
                    Send(item.Value.workSocket, data);
                }
                catch
                {
                    CloseClients.Add(item.Key);
                }
            }
            CloseClients.ForEach((string IPaddress) =>
            {
                StateObjectPool.Instance().EnQuene(ClientDic[IPaddress]);
                ClientDic.Remove(IPaddress);
            });
        }

        public void CloseServer()
        {
            try
            {
                severSocket.Close();
                severSocket = null;
                TcpLifeCycle = NetWorkLife.Destory;
                foreach (KeyValuePair<string, StateObject> item in ClientDic)
                {
                    item.Value.workSocket.Close();
                }
                SocketListenThread.Abort();   
            }
            catch (Exception ex) {
                BLog.Instance().Log("关闭Socket连接发生异常:"+ex.Message);
            }
        }

        private void Send(Socket handler, String data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            handler.BeginSend(byteData, 0, byteData.Length, 0, null, handler);
        }
    }

}
