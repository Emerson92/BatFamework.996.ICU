using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using THEDARKKNIGHT.Interface;
using THEDARKKNIGHT.Log;
using THEDARKKNIGHT.Network.Interface;
using THEDARKKNIGHT.Network.UdpSocket;
using THEDARKKNIGHT.Network.UdpSocket.Protocl;
using UnityEngine;
namespace THEDARKKNIGHT.Network.Kcp {

    public class BKcpHandler : IKcpComp,ILifeCycle
    {

        private IntPtr kcp;

        private Action<byte[]> messageCallback;

        private Action<IntPtr, int> sendMsgCallback;

        private Action<SocketError> errorCallback;

        private uint timeNow;

        private long timeStart;

        private MemoryStream memoryStream;

        private bool IsDisposed = false;

        private kcp_output kcpOutput;

        public int KcpOutput(IntPtr bytes, int len, IntPtr kcp, IntPtr user)
        {
            if (sendMsgCallback != null) sendMsgCallback(bytes, len);else BLog.Instance().Error("BKcpHandler sendMsgCallback is NULL!");
            return len;
        }

        public BKcpHandler() {
            this.Enable().SetLifeCycle(LifeCycleTool.LifeType.Update,true);
            timeStart = TimeHelper.ClientNow();
            timeNow = (uint)(TimeHelper.ClientNow() - timeStart);
            memoryStream = UdpSocketClientMgr.memoryStream.GetStream("message", ushort.MaxValue);
            kcpOutput = KcpOutput;
        }

        public void Init(uint localID, uint RemoteID) {
            this.kcp = BKcpCore.Create(RemoteID, new IntPtr(localID));
            BKcpCore.Nodelay(this.kcp, 1, 10, 1, 1);
            BKcpCore.Wndsize(this.kcp, 256, 256);
            BKcpCore.Setmtu(this.kcp, 470);
            BKcpCore.Setoutput(this.kcp, kcpOutput);
        }

        public void ReceviceData(byte[] data, int offset, int length, string IPAddress)
        {
            BKcpCore.Input(this.kcp, data, offset, length);
            while (true)
            {
                if (this.IsDisposed)
                {
                    return;
                }
                int n = BKcpCore.Peeksize(this.kcp);
                if (n < 0)
                {
                    return ;
                }
                if (n == 0)
                {
                    this.errorCallback(SocketError.NetworkReset);
                    return ;
                }

                byte[] buffer = this.memoryStream.GetBuffer();
                this.memoryStream.SetLength(n);
                this.memoryStream.Seek(0, SeekOrigin.Begin);
                int count = BKcpCore.Recv(this.kcp, buffer, ushort.MaxValue);
                if (n != count)
                {
                    return ;
                }
                if (count <= 0)
                {
                    return ;
                }

                //this.lastRecvTime = this.GetService().TimeNow;

                //this.OnRead(this.memoryStream);
                if (messageCallback != null)messageCallback(buffer);else BLog.Instance().Error("BkcpHandler messageCallback is NULL");
            }
           
        }

        public byte[] Send(byte[] data)
        {
            BKcpCore.Send(this.kcp, data, data.Length);
            return null;
        }

        public void Dispose()
        {
            IsDisposed = true;
            BKcpCore.Release(this.kcp);
        }

        public void SetMsgCallback(Action<byte[]> callback)
        {
            this.messageCallback = callback;
        }

        public void OnError(Action<SocketError> errorCallback)
        {
            this.errorCallback = errorCallback;
        }


        public void BAwake(MonoBehaviour main){}
        public void BStart(MonoBehaviour main){}
        public void BOnEnable(MonoBehaviour main){}
        public void BDisable(MonoBehaviour main){}
        public void BFixedUpdate(MonoBehaviour main){}
        public void BLateUpdate(MonoBehaviour main){}
        public void BOnApplicationFocus(MonoBehaviour main){}
        public void BOnApplicationPause(MonoBehaviour main){}
        public void BOnApplicationQuit(MonoBehaviour main){}
        public void BOnDestroy(MonoBehaviour main){}

        public void BUpdate(MonoBehaviour main){

            this.timeNow = (uint)(TimeHelper.ClientNow() - this.timeStart);

            try
            {
                BKcpCore.Update(this.kcp, timeNow);
            }
            catch (Exception e)
            {
                //Log.Error(e);
                this.errorCallback(SocketError.SocketError);
                return;
            }

            if (this.kcp != IntPtr.Zero)
            {
                uint nextUpdateTime = BKcpCore.KcpCheck(this.kcp, timeNow);
                //this.GetService().AddToUpdateNextTime(nextUpdateTime, this.Id);
            }
        }

        public void SetMsgSendFunction(Action<IntPtr, int> send)
        {
            this.sendMsgCallback = send;
        }

        public bool GetWaitsnd()
        {
            if (BKcpCore.Waitsnd(this.kcp) > 256 * 2)return false;else return true;
        }
    }
}
