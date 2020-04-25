using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using THEDARKKNIGHT.Example.FameSync.Test;
using THEDARKKNIGHT.SyncSystem.FrameSync.Utility;
using UnityEngine;

public class UIControl : MonoBehaviour {

	public DeviceDiscovry discovry;

	// Use this for initialization
	void Start () {
		discovry.OnBroadcastMsgCallback = OnMsgCallback;
	}

    private void OnMsgCallback(string data)
    {
		Broadcast msg = BFrameSyncUtility.BytesToSeralizableClass<Broadcast>(Encoding.UTF8.GetBytes(data));
		if (msg != null) {
			Debug.Log("IPAddress :" + msg.IPAddress + "Prot :" + msg.Port);
		}
	}

    // Update is called once per frame
    void Update () {
		
	}
}
