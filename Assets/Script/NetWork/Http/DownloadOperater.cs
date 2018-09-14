using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT;
using UnityEngine;
using UnityEngine.Networking;

namespace THEDARKKNIGHT
{
    public class DownloadOperater : BHttpCore<DownloadHandlerBuffer,UploadHandlerRaw>
    {
        public Action<byte[]> DownloadCompleteCallback;

        public Action HttpErrorHappenCallback;

        public Action NetworkErrorHappenCallback;

        public bool OpenReConnected = false;

        /// <summary>
        /// 设置重连次数
        /// </summary>
        public int ConnectedCountLimit = 0;

        /// <summary>
        /// 设置重连时间
        /// </summary>
        public float DelayTimeReConnected = 0;

        public void Init(){
            InitBHttpCore();
        }

        protected override void HttpErrorHappen(RequsetOperater oprater)
        {
            if (HttpErrorHappenCallback != null)
                HttpErrorHappenCallback();
            if (OpenReConnected)
                mono.Invoke("ReConnectionAgain", DelayTimeReConnected);
        }

        protected override void HttpRequsetDone(DownloadHandlerBuffer dHandler, UploadHandlerRaw uHandler){

            if (DownloadCompleteCallback != null)
                DownloadCompleteCallback(dHandler.data); 
            
        }

        protected override void NetworkErrorHappen(RequsetOperater oprater)
        {
            if (NetworkErrorHappenCallback != null)
                NetworkErrorHappenCallback();
            if (OpenReConnected)
                mono.Invoke("ReConnectionAgain", DelayTimeReConnected);
            
        }

        protected void ReConnectionAgain(RequsetOperater oprater){
            
            if(oprater.ConnectionCount < ConnectedCountLimit){
                StartSendResquest();
                oprater.ConnectionCount++;
            }
                
        }
    }
}
