﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.Example.FameSync { 
	public class Room  {

		public bool IsOwner;

		public string RoomIPAddress;

		public int port;

		public GameObject UIItem;

		public List<Memeber> memberList = new List<Memeber>(); 
		
	}

	public class Memeber {

		public bool IsReady;
	
	}
}
