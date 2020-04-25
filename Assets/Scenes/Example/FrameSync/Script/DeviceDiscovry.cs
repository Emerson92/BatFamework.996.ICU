using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using THEDARKKNIGHT.SyncSystem.FrameSync.Utility;
using UnityEngine;
using UnityEngine.Networking;

namespace THEDARKKNIGHT.Example.FameSync.Test
{

    public class DeviceDiscovry : NetworkDiscovery
    {

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
           //InitAndStartServer();
        }


        public string GetIPAddress()
        {
            //获取说有网卡信息
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    //获取以太网卡<a href="https://www.baidu.com/s?wd=%E7%BD%91%E7%BB%9C%E6%8E%A5%E5%8F%A3&tn=44039180_cpr&fenlei=mv6quAkxTZn0IZRqIHckPjm4nH00T1Ydm1TzP1NhmWw9nvn3nADd0ZwV5Hcvrjm3rH6sPfKWUMw85HfYnjn4nH6sgvPsT6KdThsqpZwYTjCEQLGCpyw9Uz4Bmy-bIi4WUvYETgN-TLwGUv3EnHnvP10YnHRznjf1n1bznjnLrf" target="_blank" class="baidu-highlight">网络接口</a>信息
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

        private void TryStopBroadCast()
        {
            StopBroadcast();
            running = false;
        }
    }


}
