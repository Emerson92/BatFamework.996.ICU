using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using THEDARKKNIGHT.Log;
using UnityEngine;

namespace THEDARKKNIGHT.Network.TcpSocket.Server
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
            Destorying,
            Destory
        }

        public  NetWorkLife TcpLifeCycle;

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
            StateObject state = null;
            lock (ClientDic) {
                if (ClientDic.ContainsKey(handler.RemoteEndPoint.ToString()))
                {
                    state = ClientDic[handler.RemoteEndPoint.ToString()];
                    state.workSocket = handler;
                }
                else
                {
                    state = StateObjectPool.Instance().OutQuene();
                    state.workSocket = handler;
                    ClientDic.Add(handler.RemoteEndPoint.ToString(), state);
                }
            }
            NewClientConnected(handler.RemoteEndPoint.ToString(), state);
            handler.BeginReceive(state.Buffer.Bytes, state.Buffer.WriteIdx, state.Buffer.Remain, 0, ReadCallback, state);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;
                if (handler == null || severSocket == null) return;
                int bytesRead = handler.EndReceive(ar);
                if (bytesRead > 0)
                {
                    state.Buffer.WriteIdx += bytesRead;
                    if (state.Buffer.Remain < 8)
                    {
                        state.Buffer.MoveBytes();
                        state.Buffer.Resize(state.Buffer.Length * 2);
                    }
                    ReceviceData(state.Buffer, bytesRead, handler.RemoteEndPoint.ToString());
                    handler.BeginReceive(state.Buffer.Bytes, state.Buffer.WriteIdx, state.Buffer.Remain, 0, ReadCallback, state);
                }
                else
                {
                    TcpLifeCycle = NetWorkLife.Disconnected;
                    Debug.Log("连接已经断开:" + handler.RemoteEndPoint.ToString());
                    ClientLoseConnect(state);
                    ClientDic.Remove(state.workSocket.RemoteEndPoint.ToString());
                    StateObjectPool.Instance().EnQuene(state);
                }
            }
            catch (Exception ex) {
                Debug.LogError("ReadCallback :" + ex);
            }
        }

        protected abstract void NewClientConnected(string IPAddress, StateObject state);

        protected abstract void ReceviceData(ByteArray data, int length, string IPAddress);

        protected virtual void ClientLoseConnect(StateObject state)
        {
            //if(ClientDic.ContainsKey(state.workSocket.RemoteEndPoint.ToString())) ClientDic.Remove(state.workSocket.RemoteEndPoint.ToString());
            CleanData(state);
        }

        private static void CleanData(StateObject state)
        {
            state.workSocket.Dispose();
            state.workSocket = null;
        }

        protected void DisableClient(string IPAddress) {
            StateObject state = null;
            if (ClientDic.TryGetValue(IPAddress,out state)) {
                CleanData(state);
            }
        }


        protected abstract void SendCallback(int senderCount, string IPAddress);


        public void SendToClient(byte[] data, int from, int count,string IPAddress) {
            CloseClients.Clear();
            lock (ClientDic) { 
                if (ClientDic.ContainsKey(IPAddress)) {
                    try {
                        Send(ClientDic[IPAddress].workSocket, data, from, count);
                    }
                    catch{
                        CloseClients.Add(IPAddress);
                    }
                }
                CloseClients.ForEach((string IPaddress) =>
                {
                    CleanData(ClientDic[IPaddress]);
                });
            }
        }


        public void SendToAll(byte[] data, int from, int count) {
            CloseClients.Clear();
            lock (ClientDic) { 
                foreach (KeyValuePair<string, StateObject> item in ClientDic)
                {
                    try
                    {
                        Send(item.Value.workSocket, data, from , count);
                    }
                    catch
                    {
                        CloseClients.Add(item.Key);
                    }
                }
                CloseClients.ForEach((string IPaddress) =>
                {
                    CleanData(ClientDic[IPaddress]);
                });
            }
        }

        private void Send(Socket handler, byte[] data, int from, int count)
        {
            if(handler != null) handler.BeginSend(data, 0, data.Length, 0, SenderCallback, handler);
        }

        private void SenderCallback(IAsyncResult ar)
        {
            Socket sender = (Socket)(ar.AsyncState);
            int count = sender.EndSend(ar);
            SendCallback(count, sender.RemoteEndPoint.ToString());
        }

        protected void CloseServer()
        {
            try
            {
                TcpLifeCycle = NetWorkLife.Destorying;
    
            }
            catch (Exception ex) {
                BLog.Instance().Log("CloseServer 关闭Socket连接发生异常:" + ex.Message);
            }
        }

        protected void DestoryServer() {
            try
            {
                TcpLifeCycle = NetWorkLife.Destory;
                foreach (KeyValuePair<string, StateObject> item in ClientDic)
                {
                    item.Value.workSocket.Close();
                    item.Value.workSocket = null;
                }
                if(severSocket !=null) severSocket.Close();
                severSocket = null;
                SocketListenThread.Abort();
                BLog.Instance().Log("服务器关闭");
            }
            catch (Exception ex)
            {
                BLog.Instance().Log("DestoryServer Socket连接发生异常:" + ex.Message);
            }
        }

    }

}
