using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using THEDARKKNIGHT.Example.FameSync.Test;
using THEDARKKNIGHT.Network.TcpSocket;
using THEDARKKNIGHT.Network.TcpSocket.Client;
using THEDARKKNIGHT.Network.TcpSocket.Server;
using THEDARKKNIGHT.SyncSystem.FrameSync.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace THEDARKKNIGHT.Example.FameSync.UI { 

	public class UIControl : MonoBehaviour {

		public bool ISSTARTSERVER = true;

		public DeviceDiscovry discovry;

		public Button CreatRoomBtn;

		public Button ExitRoomBtn;

		public List<Room> RoomLobbyList = new List<Room>();

		public GameObject RoomItem;

		public Transform RoomScroll;

		private List<Room> removeList = new List<Room>();

		// Use this for initialization
		void Start () {
			discovry.OnBroadcastMsgCallback = OnMsgCallback;
			CreatRoomBtn.onClick.AddListener(OnCreatNewRoom);
			ExitRoomBtn.onClick.AddListener(OnExitRoom);
			InvokeRepeating("CheckRoomList",1,1);
		}

		private void OnExitRoom()
		{
			StartCoroutine(discovry.InitAndStartClient());
		}

		private void OnCreatNewRoom()
		{
			if (ISSTARTSERVER) {
				Room RoomLobby = new Room();
				RoomLobby.RoomIPAddress = discovry.IPAddress;
				RoomLobby.port = discovry.broadcastPort;
				RoomLobby.IsOwner = true;
				RoomLobby.UIItem = ChangeUIStaute(discovry.IPAddress, discovry.broadcastPort);
				RoomLobbyList.Add(RoomLobby);
				StartCoroutine(discovry.InitAndStartServer());
				TcpSocketServerMgr tcpMgr = new TcpSocketServerMgr(new ReceviceDataSKeeper(new MsgParseServer()),new MessagerDataSSender(new HeartbeatSolverServer().SetHeartbeatMsg(" ").SendPeriod(10).SetCheckTick(6)));
				tcpMgr.StartSever(discovry.IPAddress,8080);
				ISSTARTSERVER = false;
			}

		}

		private GameObject ChangeUIStaute(string IP,int port)
		{
			GameObject room = GameObject.Instantiate(RoomItem, RoomScroll);
			room.transform.GetChild(0).GetComponent<Text>().text = IP + ":" + port;
			room.transform.GetChild(1).GetComponent<Button>().gameObject.SetActive(false);
			//room.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(()=> {
			//	///TODO CreateConnnect
			//	TcpSocketClientMgr client = new TcpSocketClientMgr(new ReceviceDataCKeeper(new MsgParseClient()),new MessagerDataCSender(new HeartbeatSolverClient().SetHeartbeatMsg(" ").SendPeriod(10)));
			//	client.ConnectToServer(IP,8080);
			//});
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
                TcpSocketClientMgr client = new TcpSocketClientMgr(new ReceviceDataCKeeper(new MsgParseClient()), new MessagerDataCSender(new HeartbeatSolverClient().SetHeartbeatMsg(" ").SendPeriod(10)));
                client.ConnectToServer(IP, 8080);
            });
			RoomLobby = new Room();
			RoomLobby.RoomIPAddress = discovry.IPAddress;
			RoomLobby.port = discovry.broadcastPort;
			RoomLobby.IsOwner = false;
			RoomLobby.UIItem = item;
			if (RoomLobby != null ) RoomLobbyList.Add(RoomLobby);
			
		}

		public ProtoBuf.IExtensible Decode(string protoName, byte[] bytes, int offset, int count)
		{

			using (var memory = new MemoryStream(bytes, offset, count))
			{
				Type t = Type.GetType(protoName);
				return (ProtoBuf.IExtensible)ProtoBuf.Serializer.NonGeneric.Deserialize(t, memory);
			}
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
