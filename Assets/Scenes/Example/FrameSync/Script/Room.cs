using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.Example.FameSync { 
	public class Room  {

		public bool IsOwner;

		public string RoomIPAddress;

		public int port;

		public GameObject UIItem;

		//public List<Memeber> memberList = new List<Memeber>();

		public float CheckTime = 5;
		
	}

	public class RoomMemeber {

		public string IPAddress;

		public GameObject UIItem;

		public bool IsReady;
	
	}
}

