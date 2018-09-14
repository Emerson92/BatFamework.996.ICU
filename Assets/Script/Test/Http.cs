using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT;
using UnityEngine;
using UnityEngine.Networking;

public class Http : MonoBehaviour {
    string url = "http://imtt.dd.qq.com/16891/67774101471C1324EA2E4185C219F5D1.apk?fsname=com.apowersoft.mirror_1.4.2_1427.apk&csr=1bbd";
    DownloadOperater operater;
    // Use this for initialization
    void Start () {
        operater = new DownloadOperater();
        operater.Init();
        operater.HttpStatusCallback += (float downSpeed, float uploadSpeed, float progress) =>
        {
            Debug.Log("downSpeed :" + downSpeed +"/MB "+ " uploadSpeed :" + uploadSpeed + "progress :" + progress);
        };
        operater.DownloadCompleteCallback += (byte[] data) => {
            Debug.Log("下载完成：" + data.Length);
        };
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P)) {
            operater.SetReuqestMethod(BHttpCore<UnityEngine.Networking.DownloadHandlerBuffer, UnityEngine.Networking.UploadHandlerRaw>.RequsetMethod.GET);
            operater.SetRequesUrl(url);
            operater.SetDownloadHelper(new DownloadHandlerBuffer());
            operater.StartSendResquest();
            
        }
	}
}
