using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using THEDARKKNIGHT.EventSystem;
using THEDARKKNIGHT.SyncSystem.FrameSync.Utility;
using UnityEngine;
using UnityEngine.Networking;

namespace THEDARKKNIGHT.Example.FameSync.Test
{

    public class DeviceDiscovry : NetworkDiscovery
    {

        public Action<string> OnBroadcastMsgCallback;

        private string IPAddress;

        private byte[] data;

        public void Awake()
        {
            IPAddress = GetIPAddress();
            Broadcast cast = new Broadcast();
            cast.IPAddress = IPAddress;
            cast.Port = broadcastPort;
            data = BFrameSyncUtility.NSeralizableClassTobytes(cast);
            broadcastData = Encoding.UTF8.GetString(data);
            broadcastInterval = 2 * 1000;
            Debug.Log(" cast.IPAddress :" + cast.IPAddress + "Prot :" + broadcastPort + " data Length:" + data.Length);
        }

        public void Start()
        {
            InitAndStartServer();
            //InitAndStartClient();
        }



        /// <summary>
        /// 收到广播
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="data"></param>
        public override void OnReceivedBroadcast(string fromAddress, string data)
        {
            //base.OnReceivedBroadcast(fromAddress, data);
            Debug.Log("OnReceivedBrocast " + data);
            OnBroadcastMsgCallback?.Invoke(data);
        }



        public string GetIPAddress()
        {
            //获取说有网卡信息
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                   IPInterfaceProperties ip = adapter.GetIPProperties();
                    //获取单播地址集
                    UnicastIPAddressInformationCollection ipCollection = ip.UnicastAddresses;
                    foreach (UnicastIPAddressInformation ipadd in ipCollection)
                    {
                        if (ipadd.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            if (ip.GatewayAddresses[0].Address.ToString() != "0.0.0.0")
                            {
                                return ipadd.Address.ToString();
                            }
                        }
                    }
                }
            }
            return null;
        }

        public void ReBroadcast()
        {
            StartCoroutine(IEnum_ReBroadcast());
        }

        IEnumerator IEnum_ReBroadcast()
        {
            TryStopBroadCast();
            yield return null;
            InitAndStartServer();
        }

        private void InitAndStartServer()
        {

            Initialize();
            StartAsServer();
        }

        private void InitAndStartClient()
        {
            Initialize();
            StartAsClient();
        }

        private void TryStopBroadCast()
        {
            StopBroadcast();
        }
    }
}
