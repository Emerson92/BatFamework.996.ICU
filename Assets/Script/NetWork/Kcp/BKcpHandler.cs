using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using THEDARKKNIGHT.Interface;
using THEDARKKNIGHT.Network.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.Network.Kcp {

    public class BKcpHandler : IKcpComp,ILifeCycle
    {

        private IntPtr kcp;

        public BKcpHandler() {
            this.Enable().SetLifeCycle(LifeCycleTool.LifeType.Update,true);
        }

        public void InitSuccess(string IPAddress,uint listernPort,uint sendPort)
        {
            //this.kcp = BKcpCore.Create(this.RemoteConn, new IntPtr(this.LocalConn));
            //BKcpCore.Nodelay(this.kcp, 1, 10, 1, 1);
            //BKcpCore.Wndsize(this.kcp, 256, 256);
            //BKcpCore.Setmtu(this.kcp, 470);
        }

        public byte[] ReceviceData(byte[] data, int length, string IPAddress)
        {
            return null;
        }

        public byte[] Send(byte[] data)
        {
            return null;
        }

        public void Dispose()
        {
           
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
        public void BUpdate(MonoBehaviour main){}


    }
}
