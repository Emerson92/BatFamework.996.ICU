using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using THEDARKKNIGHT.Network.Interface;
using THEDARKKNIGHT.Network.UdpSocket;
using THEDARKKNIGHT.Network.UdpSocket.Protocl;
using UnityEngine;
namespace THEDARKKNIGHT.Network.UdpSocket.Component {

    public class MessageKeeper : IReceviceDataKeeper
    {
        private uint currentID;

        private IKcpComp comp;

        private Action<UdpSocketClient.CONNECTSTATUS> StatusChangeFunction;

        private UdpSocketClient.CONNECTSTATUS currentStatus;

        private IMessageSolver messageSolver;

        private IMessageSender sendComp;

        private readonly MemoryStream memoryStream;

        private MessageKeeper() {
            memoryStream = UdpSocketClientMgr.memoryStream.GetStream("message", ushort.MaxValue);
        }

        public MessageKeeper(IMessageSolver solver) {
            this.messageSolver = solver;
        }

        public void MessageDataRecevice(byte[] data, int length, string IPAddress)
        {
            Debug.Log(Encoding.UTF8.GetString(data));
            byte flag = data[0];
            uint serverID = 0;
            uint clientID = 0;
            switch (flag) {
                case KcpProtocalType.SYN://Connecting msg
                    Debug.Log("Recevice the connected requeset message!");
                    serverID = BitConverter.ToUInt32(data, 1);
                    byte[] buffer = this.memoryStream.GetBuffer();
                    buffer.WriteTo(0, KcpProtocalType.ACK);
                    buffer.WriteTo(1, currentID);
                    buffer.WriteTo(5, serverID);
                    ///Start to send ACK Msg
                    if (sendComp != null) sendComp.SendMsg(buffer);
                    StatusChangeFunction?.Invoke(UdpSocketClient.CONNECTSTATUS.CONNECTING);
                    break;
                case KcpProtocalType.ACK://Connecting Ack
                    Debug.Log("Recevice the Connected Confirm message!");
                    serverID = BitConverter.ToUInt32(data, 1);
                    clientID = BitConverter.ToUInt32(data, 5);
                    if (comp != null) comp.Init(clientID, serverID);
                     StatusChangeFunction?.Invoke(UdpSocketClient.CONNECTSTATUS.CONNECTED);
                    break;
                case KcpProtocalType.FIN://Finish Connecting
                    serverID = BitConverter.ToUInt32(data, 1);
                    clientID = BitConverter.ToUInt32(data, 5);
                    if (comp != null) comp.Dispose();
                    StatusChangeFunction?.Invoke(UdpSocketClient.CONNECTSTATUS.DISCONNECTED);
                    break;
                case KcpProtocalType.MSG://message Conent
                    serverID = BitConverter.ToUInt32(data, 1);
                    clientID = BitConverter.ToUInt32(data, 5);
                    if (comp != null) comp.ReceviceData(data, 5, length - 5, IPAddress);
                    break;
            }
        }

        public void MessageCallback(byte[] data) {
            if(messageSolver != null) messageSolver.MessageSolver(data, 0, data.Length);
        }

        public void SetKcpComponent(IKcpComp comp)
        {
            this.comp = comp;
            this.comp.SetMsgCallback(MessageCallback);
            this.comp.OnError(ConnectError);
        }

        /// <summary>
        /// network error
        /// </summary>
        /// <param name="e"></param>
        public void ConnectError(SocketError e) {
            switch (e) {
                case SocketError.SocketError:
                    break;
                case SocketError.NetworkReset:
                    break;
            }
        }

        public void Dispose()
        {
            if (comp != null) comp.Dispose();
        }

        public void SetCurrentID(uint ID)
        {
            this.currentID = ID;
        }

        public void ConnectStatusChange(UdpSocketClient.CONNECTSTATUS status)
        {
            this.currentStatus = status;
        }

        public void SetConnectStatus(Action<UdpSocketClient.CONNECTSTATUS> callback)
        {
            this.StatusChangeFunction = callback;
        }

        public void SetSendComp(IMessageSender sender)
        {
            this.sendComp = sender;
        }
    }

}
