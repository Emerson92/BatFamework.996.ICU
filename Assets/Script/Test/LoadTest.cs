using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.ConstDefine;
using THEDARKKNIGHT.ResourceSystem;
using UnityEngine;
using UnityEngine.UI;

public class LoadTest : MonoBehaviour {

    GameObject currentObject;

	// Use this for initialization
	void Start () {
        //LoadTestButton.onClick.AddListener(LoadDragonAssetBundle);
        BResourceMgr.Instance().Init();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void LoadSpaceShipAssetBundle()
    {
        Debug.Log("Excution LoadSpaceShipAssetBundle!");
        BResourceMgr.Instance().LoadCacheResAsync(Application.streamingAssetsPath + "/spaceship.sdf", (AssetBundleCreateRequest request) =>
        {
            if(currentObject!=null) {
                Destroy(currentObject);
                Resources.UnloadUnusedAssets();
            }
            GameObject model = request.assetBundle.LoadAsset<GameObject>("Spaceship") as GameObject;
            currentObject = Instantiate(model) as GameObject;
            request.assetBundle.Unload(false);
            //dragon.transform.position = new Vector3(0,-4,6f);
            //dragon.transform.localScale = Vector3.one;
        });
    }

    public void LoadDragonAssetBundle()
    {
        Debug.Log("Excution LoadDragonAssetBundle!");
        BResourceMgr.Instance().LoadCacheResAsync(Application.streamingAssetsPath + "/dragon.sdf", (AssetBundleCreateRequest request) =>
        {
            if (currentObject != null)
            {
                Destroy(currentObject);
                Resources.UnloadUnusedAssets();
            }
            GameObject model = request.assetBundle.LoadAsset<GameObject>("dragon") as GameObject;
            currentObject = Instantiate(model) as GameObject;
            request.assetBundle.Unload(false);
            //dragon.transform.position = new Vector3(0,-4,6f);
            //dragon.transform.localScale = Vector3.one;
        });
    }
}
