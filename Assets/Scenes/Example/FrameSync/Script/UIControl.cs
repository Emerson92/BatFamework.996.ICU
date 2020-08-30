using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using THEDARKKNIGHT.Example.FameSync.Test;
using THEDARKKNIGHT.SyncSystem.FrameSync.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace THEDARKKNIGHT.Example.FameSync.UI { 

	public class UIControl : MonoBehaviour {

		public DeviceDiscovry discovry;

		public Button CreatRoomBtn;

		public Button ExitRoomBtn;

		public List<Room> RoomLobbyList = new List<Room>();

		public GameObject RoomItem;

		public Transform RoomScroll;

		// Use this for initialization
		void Start () {
			discovry.OnBroadcastMsgCallback = OnMsgCallback;
			CreatRoomBtn.onClick.AddListener(OnCreatNewRoom);
			ExitRoomBtn.onClick.AddListener(OnExitRoom);

		}

		private void OnExitRoom()
		{
			StartCoroutine(discovry.InitAndStartClient());
		}

		private void OnCreatNewRoom()
		{
			Room RoomLobby = new Room();
			RoomLobby.RoomIPAddress = discovry.IPAddress;
			RoomLobby.port = discovry.broadcastPort;
			RoomLobby.IsOwner = true;
			RoomLobby.UIItem = ChangeUIStaute(discovry.IPAddress, discovry.broadcastPort);
			RoomLobbyList.Add(RoomLobby);
			StartCoroutine(discovry.InitAndStartServer());
		}

		private GameObject ChangeUIStaute(string IP,int port)
		{
			GameObject room = GameObject.Instantiate(RoomItem, RoomScroll);
			room.transform.GetChild(0).GetComponent<Text>().text = IP + ":" + port;
			room.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(()=> { 
				///TODO CreateConnnect
			
			});
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
			string IPAddress = IP + ":" + port;
			Room RoomLobby = null;
			if (RoomLobbyList.Count > 0) { 
				for (int i = 0;i < RoomLobbyList.Count;i++) {
					if (RoomLobbyList[i].RoomIPAddress == IP && RoomLobbyList[i].port == port)
						yield break;
				}
			}
			GameObject item = GameObject.Instantiate(RoomItem, RoomScroll);
			item.transform.GetChild(0).GetComponent<Text>().text = IPAddress;
			item.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
			{
				///TODO CreateConnnect

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
	}

}
