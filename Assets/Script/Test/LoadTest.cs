using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.ConstDefine;
using THEDARKKNIGHT.ResourceSystem;
using UnityEngine;

public class LoadTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        BResourceMgr.Instance().Init();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P)) {
            Debug.Log(BFameWorkDefine.BFameDataPath + "/dungouji.sdf");
            GameObject ob = Resources.Load("dungouji.sdf") as GameObject;
            Instantiate(ob);
            //BResourceMgr.Instance().LoadCacheResAsync(BFameWorkDefine.BFameDataPath + "/dungouji.sdf", (AssetBundleCreateRequest request) => {
            //    GameObject DunGunJI = request.assetBundle.LoadAsset<GameObject>("dungouji") as GameObject;
            //    Instantiate(DunGunJI);
            //});

        }
    }
}
