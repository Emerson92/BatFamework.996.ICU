using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using THEDARKKNIGHT.Interface;
using THEDARKKNIGHT.Network.Interface;
using THEDARKKNIGHT.Network.UdpSocket;
using UnityEngine;
namespace THEDARKKNIGHT.Network.Kcp {

    public class BKcpHandler : IKcpComp,ILifeCycle
    {

        private IntPtr kcp;

        private Action<byte[]> messageCallback;

        private Action<SocketError> errorCallback;

        private uint timeNow;

        private long timeStart;

        private MemoryStream memoryStream;

        private bool IsDisposed = false;

        public BKcpHandler() {
            this.Enable().SetLifeCycle(LifeCycleTool.LifeType.Update,true);
            timeStart = TimeHelper.ClientNow();
            timeNow = (uint)(TimeHelper.ClientNow() - timeStart);
            memoryStream = UdpSocketClientMgr.memoryStream.GetStream("message", ushort.MaxValue);
        }

        public void Init(uint localID, uint RemoteID) {
            this.kcp = BKcpCore.Create(RemoteID, new IntPtr(localID));
            BKcpCore.Nodelay(this.kcp, 1, 10, 1, 1);
            BKcpCore.Wndsize(this.kcp, 256, 256);
            BKcpCore.Setmtu(this.kcp, 470);
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
                if (messageCallback != null) messageCallback(buffer);
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


    }
}
