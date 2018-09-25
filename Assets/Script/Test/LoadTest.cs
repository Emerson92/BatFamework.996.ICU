using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.ConstDefine;
using THEDARKKNIGHT.InputOperate;
using THEDARKKNIGHT.ResourceSystem;
using UnityEngine;

public class LoadTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        BResourceMgr.Instance();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P)) {
            //Debug.Log(BFameWorkDefine.BFameDataPath + "/dungouji.sdf");
            //GameObject ob = Resources.Load("dungouji.sdf") as GameObject;
            //Instantiate(ob);
            BResourceMgr.Instance().LoadCacheResAsync(Application.streamingAssetsPath + "/dragon.sdf", (AssetBundleCreateRequest request) =>
            {
                GameObject DunGunJI = request.assetBundle.LoadAsset<GameObject>("dragon") as GameObject;
                Instantiate(DunGunJI);
            });

        }
    }
}
