﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using THEDARKKNIGHT.ConstDefine;
using THEDARKKNIGHT.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.ResourceSystem {

    public class BResourceMgr : BatSingletion<BResourceMgr>, ILifeCycle
    {
        private Dictionary<string, AssetBaseInfo> ResMgrDic = new Dictionary<string, AssetBaseInfo>();

        private MonoBehaviour mainMono;

        private ICacheFileSystem CacheSystem = new BCacheFileMgr();


        private BResourceMgr() { }

        public void Init() {
            this.Enable();
        }

        /// <summary>
        ///  设置文件缓存器
        /// </summary>
        /// <param name="cache"></param>
        public void SetCacheFileSystem(ICacheFileSystem cache) {
            this.CacheSystem = cache;
        }
        public void BAwake(MonoBehaviour main)
        {
            this.mainMono = main;
            LifeCycleTool tool = this.GetLifeCycleTool();
            tool.SetLifeCycle(LifeCycleTool.LifeType.Update,true)
                .SetLifeCycle(LifeCycleTool.LifeType.OnApplicationQuit,true);
        }
        public void BDisable(MonoBehaviour main){}
        public void BFixedUpdate(MonoBehaviour main){}
        public void BLateUpdate(MonoBehaviour main){}
        public void BOnApplicationFocus(MonoBehaviour main){}
        public void BOnApplicationPause(MonoBehaviour main){}
        public void BOnApplicationQuit(MonoBehaviour main){
            AssetBaseInfo[] tempArray = new AssetBaseInfo[ResMgrDic.Count];
            int i = 0;
            foreach (AssetBaseInfo item in ResMgrDic.Values)
            {
                tempArray[i] = item;
                i++;
            }
            AssetInfoGroup group = new AssetInfoGroup(tempArray);
            CacheSystem.SaveData("ResourceConfig",BFameWorkDefine.BFameDataPath+"/Config",Encoding.UTF8.GetBytes(JsonUtility.ToJson(group)));
            BLog.Instance().Log("保存资源配置文件完毕! : "+ BFameWorkDefine.BFameDataPath + "/Config/ResourceConfig");
        }

        public void BOnDestory(MonoBehaviour main){}
        public void BOnDestroy(MonoBehaviour main){}
        public void BOnEnable(MonoBehaviour main){}
        public void BStart(MonoBehaviour main) {
            CacheSystem.LoadData(BFameWorkDefine.BFameDataPath + "/Config/ResourceConfig");
            BLog.Instance().Log("成功读取资源配置文件完毕! : "+ BFameWorkDefine.BFameDataPath + "/Config/ResourceConfig");
        }
        public void BUpdate(MonoBehaviour main){}

        public AssetBundle LoadCacheRes(string loadPath) {
            AssetBundle ab = AssetBundle.LoadFromFile(loadPath);
            return ab;
        }

        public AssetBundle LoadRes(string resName)
        {
            AssetBaseInfo asset = null;
            ResMgrDic.TryGetValue(resName, out asset);
            if (asset != null)
            {
                AssetBundle ab = AssetBundle.LoadFromFile(asset.urlPath);
                return ab;
            }
            else
                return null;
        }

        public AssetBundle LoadRes(string resName,byte[] data, float version) {
           AssetBundle ab = AssetBundle.LoadFromMemory(data);
           CacheAssetInfo(resName, data, version, ab);
           return ab;
        }

        public string[] LoadResDependent(string resName, AssetBundle ab) {
            AssetBundleManifest manifest = (AssetBundleManifest)ab.LoadAsset("AssetBundleManifest");
            string[] dependencies = manifest.GetAllDependencies(resName); 
            ab.Unload(false);
            return dependencies;
        }

        public void LoadCacheResAsync(string resPath, Action<AssetBundleCreateRequest> LoadFinishCallback) {
            mainMono.StartCoroutine(LoadRes(resPath, LoadFinishCallback));
        }


        public void LoadResAsync(string resName, Action<AssetBundleCreateRequest> LoadFinishCallback)
        {
            AssetBaseInfo asset = null;
            ResMgrDic.TryGetValue(resName, out asset);
            if(asset !=null)
                mainMono.StartCoroutine(LoadRes(asset.urlPath, LoadFinishCallback));
        }


        public void LoadResAsync(string resName,byte[] data, float version, Action<AssetBundleCreateRequest> LoadFinishCallback) {
            mainMono.StartCoroutine(LoadRes(resName, data, version, LoadFinishCallback));
        }

        private IEnumerator LoadRes(string resName, byte[] data, float version, Action<AssetBundleCreateRequest> LoadFinishCallback)
        {
            AssetBundleCreateRequest rRequest = AssetBundle.LoadFromMemoryAsync(data);
            yield return rRequest;
            if (!rRequest.isDone)
            {
                BLog.Instance().Warn("Fail load AssetBundle file at " + resName);
                yield break;
            }
            else
            {
                if (rRequest.assetBundle != null)
                {
                    CacheAssetInfo(resName, data, version, rRequest.assetBundle);
                    if (LoadFinishCallback != null)
                        LoadFinishCallback(rRequest);
                }
                else
                {
                    BLog.Instance().Warn("Fail load AssetBundle's  all assets at " + resName);
                    yield break;
                }

            }

        }

        /// <summary>
        /// 存储相关AB信息
        /// </summary>
        /// <param name="resName"></param>
        /// <param name="data"></param>
        /// <param name="version"></param>
        /// <param name="asset"></param>
        private void CacheAssetInfo(string resName, byte[] data, float version, AssetBundle asset)
        {
            AssetBundleManifest manifest = (AssetBundleManifest)asset.LoadAsset("AssetBundleManifest");
            string SaveUrl = BFameWorkDefine.BFameDataPath + "/" + resName;
            if (ResMgrDic.ContainsKey(resName))
            {
                AssetBaseInfo oldAsset = ResMgrDic[resName];
                if (oldAsset.Version < version)
                {
                    AssetBaseInfo assetInfo = new AssetBaseInfo(resName, SaveUrl, manifest.GetAllDependencies(resName), version);
                    CacheSystem.SaveData(resName,SaveUrl, data);
                    ResMgrDic[resName] = assetInfo;
                }
            }
            else
            {
                AssetBaseInfo assetInfo = new AssetBaseInfo(resName, SaveUrl, manifest.GetAllDependencies(resName), version);
                CacheSystem.SaveData(resName,SaveUrl, data);
                ResMgrDic.Add(resName, assetInfo);
            }
        }

        private IEnumerator LoadRes(string resPath,Action<AssetBundleCreateRequest> LoadFinishCallback)
        {
            AssetBundleCreateRequest rRequest = AssetBundle.LoadFromFileAsync(resPath);
            yield return rRequest;
            if (!rRequest.isDone)
            {
                BLog.Instance().Warn("Fail load AssetBundle file at " + resPath);
                yield break;
            }
            else
            {
                if (rRequest.assetBundle != null)
                {

                    if (LoadFinishCallback != null)
                        LoadFinishCallback(rRequest);
                    //AssetBundleRequest rABReq = rRequest.assetBundle.LoadAllAssetsAsync();
                    //yield return rABReq;
                    //if (rABReq.isDone)
                    //{
                    //    if (LoadFinishCallback != null)
                    //        LoadFinishCallback(rABReq);
                    //    //AssetBundleManifest rManifest = rABReq.asset as AssetBundleManifest;
                    //    //string[] rAllAssetNames = rManifest.GetAllAssetBundles();
                    //    //for (int asset_index = 0; asset_index < rAllAssetNames.Length; asset_index++)
                    //    //{
                    //    //    string[] rDependencsName = rManifest.GetAllDependencies(rAllAssetNames[asset_index]);
                    //    //    for (int i = 0; i < rDependencsName.Length; i++)
                    //    //    {
                    //    //        Debug.LogError(rDependencsName[i]);
                    //    //    }
                    //    //    AssetReqBaseInfo rBaseInfo = new AssetReqBaseInfo(rAllAssetNames[asset_index], rDependencsName, rVersion);
                    //    //    mAssetBundleInfoDic.Add(rAllAssetNames[asset_index], rBaseInfo);
                    //    //}
                    //}
                }
                else
                {
                    BLog.Instance().Warn("Fail load AssetBundle's  all assets at " + resPath);
                    yield break;
                }
            }
        }

        [Serializable]
        private class AssetInfoGroup {

            AssetBaseInfo[] info;

            public AssetInfoGroup(AssetBaseInfo[] info) {
                this.info = info;
            }
        }


        [Serializable]
        private class AssetBaseInfo
        {
            public string ABName;//ab名称

            public string urlPath; // 存储地址

            public string[] DependenceAB;//相关依赖关系

            public float Version; //对应的ab版本号

            public AssetBaseInfo(string rABName, string cachePath, string[] rDepABName, float rVersion)
            {
                ABName = rABName;
                urlPath = cachePath;
                DependenceAB = rDepABName;
                Version = rVersion;
            }
        }
    }

}