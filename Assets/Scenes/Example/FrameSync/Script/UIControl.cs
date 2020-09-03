using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using THEDARKKNIGHT.EventSystem;
using THEDARKKNIGHT.Example.FameSync.Test;
using THEDARKKNIGHT.Network.TcpSocket;
using THEDARKKNIGHT.Network.TcpSocket.Client;
using THEDARKKNIGHT.Network.TcpSocket.Server;
using THEDARKKNIGHT.SyncSystem.FrameSync.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static MsgDataParse;

namespace THEDARKKNIGHT.Example.FameSync.UI { 

	public class UIControl : MonoBehaviour {

		public bool ISSTARTSERVER = true;

		public DeviceDiscovry discovry;

		public Button CreatRoomBtn;

		public Button ExitRoomBtn;

		public List<Room> RoomLobbyList = new List<Room>();

		private List<Room> removeList = new List<Room>();

		public List<RoomMemeber> RoomRemberList = new List<RoomMemeber>();

		public GameObject ClientItem;

		public Transform ClientParent;

		public GameObject RoomItem;

		public Transform RoomScroll;

		private Room ServerRoom = null;

        // Use this for initialization
        void Start () {
			discovry.OnBroadcastMsgCallback = OnMsgCallback;
			CreatRoomBtn.onClick.AddListener(OnCreatNewRoom);
			ExitRoomBtn.onClick.AddListener(OnExitRoom);
			InvokeRepeating("CheckRoomList",1,1);
			BEventManager.Instance().AddListener("MsgOnReceive", OnMsgReceiveCallback);
		}

        private object OnMsgReceiveCallback(object data)
        {
			MsgBoxData msg = (MsgBoxData)data ;
			MsgBox box = msg.Body;
			
            switch (box.MsgType)
            {
                case 0:
                    break;
                case 1://进入房间
					RoomLobby statue = JsonUtility.FromJson<RoomLobby>(box.Msgbody);
					GameObject clientItem = GameObject.Instantiate(ClientItem, ClientParent);
					clientItem.transform.GetChild(0).GetComponent<Text>().text = msg.IPAddress;
					clientItem.transform.GetChild(1).GetComponent<Image>().color = statue.ClientStatue ? Color.green : Color.red;
					RoomMemeber memeber = new RoomMemeber();
                    memeber.IPAddress = msg.IPAddress;
                    memeber.IsReady = statue.ClientStatue ? true : false;
                    memeber.UIItem = clientItem;
                    RoomRemberList.Add(memeber);
					//TODO 下发房间列表
					SendAllRemember();
                    break;
                case 2://退出房间
					int i = 0;
					int num = 0;
					RoomRemberList.ForEach((remeber)=> {
						if (remeber.IPAddress == msg.IPAddress) {
							Destroy(remeber.UIItem);
							num = i;
						}
						i++;
					});
					RoomRemberList.RemoveAt(num);
					SendAllRemember();
					break;
				case 3://下发房间成员信息
					//TODO 新增 or 删除
					RoomList list = JsonUtility.FromJson<RoomList>(box.Msgbody);
					DestoryAllRemeberUI();
					RoomRemberList.Clear();
					list.RoomRemberList.ForEach((remeber)=> {
						GameObject c = GameObject.Instantiate(ClientItem, ClientParent);
						c.transform.GetChild(0).GetComponent<Text>().text = remeber.IPAddress;
						c.transform.GetChild(1).GetComponent<Image>().color = remeber.IsReady ? Color.green : Color.red;
						RoomMemeber m = new RoomMemeber();
						m.IPAddress = remeber.IPAddress;
						m.IsReady = remeber.IsReady ? true : false;
						m.UIItem = c;
						RoomRemberList.Add(m);
					});
					break;
			}
            return null;
        }

        private void DestoryAllRemeberUI()
        {
			Transform[] childes = RoomScroll.GetComponentsInChildren<Transform>();
			for (int i = 0; i < childes.Length; i++) {
				Destroy(childes[i]);
			}
		}

        private void SendAllRemember()
        {
			RoomList list = new RoomList();
			list.RoomRemberList = RoomRemberList;
			MsgBox b = new MsgBox();
			b.MsgType = 3;
			b.Msgbody = JsonUtility.ToJson(list);
			TcpCommuSystem.Instance().SendMsg(BFrameSyncUtility.Encode(b));
		}

        private void OnExitRoom()
		{
			StartCoroutine(discovry.InitAndStartClient());
		}

		private void OnCreatNewRoom()
		{
			if (ISSTARTSERVER)
            {
                Room RoomLobby = new Room();
                RoomLobby.RoomIPAddress = discovry.IPAddress;
                RoomLobby.port = discovry.broadcastPort;
                RoomLobby.IsOwner = true;
                RoomLobby.UIItem = ChangeUIStaute(discovry.IPAddress, discovry.broadcastPort);
                CreatServerRemeber(RoomLobby);
                StartCoroutine(discovry.InitAndStartServer());
                //TODO Set ServerStart'
                TcpCommuSystem.Instance().StartServer(discovry.IPAddress);
                CreatRoomBtn.transform.GetChild(0).GetComponent<Text>().text = "关闭房间";
                ISSTARTSERVER = false;
            }
            else {
				if (ServerRoom != null) {
					Destroy(ServerRoom.UIItem);
					TcpCommuSystem.Instance().CloseServer();
					ServerRoom = null;
					CreatRoomBtn.transform.GetChild(0).GetComponent<Text>().text = "创建房间";
					ISSTARTSERVER = true;
				}
			}

		}

        private void CreatServerRemeber(Room RoomLobby)
        {
            GameObject c = GameObject.Instantiate(ClientItem, ClientParent);
            c.transform.GetChild(0).GetComponent<Text>().text = discovry.IPAddress;
            c.transform.GetChild(1).GetComponent<Image>().color = Color.green;
            RoomMemeber m = new RoomMemeber();
            m.IPAddress = discovry.IPAddress;
            m.IsReady = true;
            m.UIItem = c;
            RoomRemberList.Add(m);
            ServerRoom = RoomLobby;
        }

        private GameObject ChangeUIStaute(string IP,int port)
		{
			GameObject room = GameObject.Instantiate(RoomItem, RoomScroll);
			room.transform.GetChild(0).GetComponent<Text>().text = IP + ":" + port;
			room.transform.GetChild(1).GetComponent<Button>().gameObject.SetActive(false);
			return room;
		}

		private void OnMsgCallback(string data)
		{
			try
			{
				string[] msg = data.Split('|');
				Broadcast broadcast = JsonUtility.FromJson<Broadcast>(msg[0]);
				if (msg != null)
				{
					Debug.Log("IPAddress :" + broadcast.IPAddress + "Prot :" + broadcast.Port);
					StartCoroutine(AddRoomUI(broadcast.IPAddress, broadcast.Port));
				}
			}
			catch (Exception ex) {
				Debug.Log(ex);
			}
		}

		private IEnumerator AddRoomUI(string IP,int port)
		{
			yield return new WaitForEndOfFrame();
			//CheckRoomList();
			string IPAddress = IP + ":" + port;
			Room RoomLobby = null;
			if (RoomLobbyList.Count > 0) { 
				for (int i = 0;i < RoomLobbyList.Count;i++) {
					if (RoomLobbyList[i].RoomIPAddress == IP && RoomLobbyList[i].port == port) {
						RoomLobbyList[i].CheckTime = 5;
						yield break;
					}

				}
			}
			GameObject item = GameObject.Instantiate(RoomItem, RoomScroll);
			item.transform.GetChild(0).GetComponent<Text>().text = IPAddress;
			item.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
			{
				///TODO CreateConnnect
				TcpCommuSystem.Instance().ConnectToServer(IP);
				RoomLobby statue = new RoomLobby();
				statue.RoomStatue = false;
				statue.LobbyStatue = 1;
				statue.ClientStatue = false;
				MsgBox b = new MsgBox();
				b.MsgType = 1;
				b.Msgbody = JsonUtility.ToJson(statue);
				TcpCommuSystem.Instance().SendMsg(BFrameSyncUtility.Encode(b));
			});
			RoomLobby = new Room();
			RoomLobby.RoomIPAddress = discovry.IPAddress;
			RoomLobby.port = discovry.broadcastPort;
			RoomLobby.IsOwner = false;
			RoomLobby.UIItem = item;
			if (RoomLobby != null ) RoomLobbyList.Add(RoomLobby);
			
		}

		// Update is called once per frame
		void Update () {



		}

		public void CheckRoomList()
		{
			if (RoomLobbyList.Count > 0)
			{
				RoomLobbyList.ForEach((room) => {
					if (room != null)
					{
						if (!room.IsOwner) {
							room.CheckTime -= 1;
							if (room.CheckTime < 0)
							{
								Destroy(room.UIItem);
								removeList.Add(room);
							}
						}
					}
				});
			}

			if (removeList.Count > 0)
			{
				removeList.ForEach((room) =>
				{
					RoomLobbyList.Remove(room);
				});
				removeList.Clear();
			}
		}

        public void OnDestroy()
        {
			CancelInvoke("CheckRoomList");
        }
    }
}
