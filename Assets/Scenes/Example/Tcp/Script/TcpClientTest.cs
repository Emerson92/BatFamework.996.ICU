using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using THEDARKKNIGHT.EventSystem;
using THEDARKKNIGHT.Log;
using THEDARKKNIGHT.Network.TcpSocket.Client;
using UnityEngine;
using UnityEngine.UI;

namespace THEDARKKNIGHT.Network.TcpSocket.Test
{

    public class TcpClientTest : MonoBehaviour
    {

        public GameObject otherWord;

        public GameObject SelfWord;


        [Header("消息输入框")]
        public InputField MsgInputField;

        [Header("IP地址输入框")]
        public InputField IPAddressInputField;

        [Header("端口号输入框")]
        public InputField PortInputField;

        [Header("发送消息按钮")]
        public Button SendMsgBtn;

        [Header("建立连接")]
        public Button ConnectBtn;

        [Header("断开连接")]
        public Button DisconnectBtn;

        [Header("消息聊天框")]
        public Transform view;

        private TcpSocketClientMgr tcpMgr;

        void Awake() {
            tcpMgr = new TcpSocketClientMgr(new ReceviceDataCKeeper(new MsgParseClient()),new MessagerDataCSender(new HeartbeatSolverClient().SetHeartbeatMsg(" ").SendPeriod(10)));
        }



        // Use this for initialization
        void Start()
        {
            ConnectBtn.onClick.AddListener(OnConnectServer);
            DisconnectBtn.onClick.AddListener(OnDisConnectServer);
            SendMsgBtn.onClick.AddListener(OnSenderMsgToServer);
            tcpMgr.LostServerConnected = LostConnect;
            BEventManager.Instance().AddListener("Msg", OnGetTalkMsg);
        }

        private void LostConnect(string ip)
        {
            Debug.Log("LostConnect :"+ip);
        }

        private object OnGetTalkMsg(object data)
        {
            GameObject word = GameObject.Instantiate(otherWord, view);
            Text t = word.GetComponent<Text>();
            t.text = Encoding.UTF8.GetString((byte[])data);
            return null;
        }

        private void OnSenderMsgToServer()
        {
            string text = MsgInputField.text;
            MsgTalk t = new MsgTalk();
            t.MsgType = 1;
            t.Msgbody = text;
            GameObject word = GameObject.Instantiate(SelfWord, view);
            Text w = word.GetComponent<Text>();
            w.text = text;
            tcpMgr.Send(Encode(t));
        }

        private void OnDisConnectServer()
        {
            tcpMgr.Close();
            tcpMgr = null;
        }

        private void OnConnectServer()
        {
            string IPAddress = IPAddressInputField.text;
            string IPort = PortInputField.text;
            if (tcpMgr != null) tcpMgr.ConnectToServer(IPAddress, int.Parse(IPort));
            else {
                tcpMgr = new TcpSocketClientMgr(new ReceviceDataCKeeper(new MsgParseClient()), new MessagerDataCSender(new HeartbeatSolverClient().SetHeartbeatMsg(" ").SendPeriod(5)));
                tcpMgr.ConnectToServer(IPAddress, int.Parse(IPort));
            }
        }

        public byte[] Encode(ProtoBuf.IExtensible MsgTalk) {
            using (var meomry = new System.IO.MemoryStream()) {
                ProtoBuf.Serializer.Serialize(meomry, MsgTalk);
                return meomry.ToArray();
            }
        }


        // Update is called once per frame
        void Update()
        {

        }

        void OnDestroy() {
            BLog.Instance().Log("OnDestroy");
            if(tcpMgr !=null) tcpMgr.Close();
        }
    }


}
