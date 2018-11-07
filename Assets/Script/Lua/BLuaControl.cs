using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using THEDARKKNIGHT.BatCore;
using THEDARKKNIGHT.ConstDefine;
using THEDARKKNIGHT.Interface;
using UnityEngine;
using XLua;

namespace THEDARKKNIGHT.Lua
{
    public class BLuaControl : BatSingletion<BLuaControl>, ILifeCycle
    {

        public enum LOADTYPE {
            ASSETBUNDLE,
            LOCALFILE
        }

        public LOADTYPE LoadFileType = LOADTYPE.LOCALFILE;

        public LuaEnv LuaEnvRoot;

        private LifeCycleTool Tool;

        private BLuaControl()
        {
            this.Enable();
            Tool = this.GetLifeCycleTool();
            LuaEnvRoot = new LuaEnv();
        }

        public void Init()
        {
            Tool.SetLifeCycle(LifeCycleTool.LifeType.Update, true);
            Tool.SetLifeCycle(LifeCycleTool.LifeType.OnDestroy, true);
            LuaEnvRoot.AddLoader(LuaFileLoad);
        }

        public void LoadLuaFile(string fileName) {
            LuaEnvRoot.DoString("require '"+ fileName + "'");
        }

        public LuaTable LoadFileWithCondition(string fileName,Action<LuaTable> callBack = null)
        {
            LuaTable scriptEnv = LuaEnvRoot.NewTable();
            // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
            LuaTable meta = LuaEnvRoot.NewTable();
            Debug.Log("LuaEnvRoot.Global :" + LuaEnvRoot.Global);
            meta.Set("__index", LuaEnvRoot.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose();
            if (callBack != null)
                callBack(scriptEnv);
            LuaEnvRoot.DoString("require '" + fileName + "'", fileName, scriptEnv);
            return scriptEnv;
        }

        public void LoadFile(byte[] bytes, string fileName, LuaTable table) {
            LuaEnvRoot.DoString(bytes, fileName, table);
        }

        private byte[] LuaFileLoad(ref string fileName){
            //@"F:\5.6.1\FishingJoy\AssetBundles\" + fileName + ".lua.txt"
            switch (LoadFileType) {
                case LOADTYPE.LOCALFILE:
                    string absPath = BFameWorkPathDefine.BFameLuaLoadPath + "/" + fileName + ".lua.txt";
                    Debug.Log(absPath);
                    return System.Text.Encoding.UTF8.GetBytes(File.ReadAllText(absPath));
                case LOADTYPE.ASSETBUNDLE:
                    //TODO need to load the file from the AssetBundle
                    return null;
                default:
                    return null;
            }
           
        }

        public void BAwake(MonoBehaviour main){}
        public void BDisable(MonoBehaviour main){}
        public void BFixedUpdate(MonoBehaviour main){}
        public void BLateUpdate(MonoBehaviour main){}
        public void BOnApplicationFocus(MonoBehaviour main){}
        public void BOnApplicationPause(MonoBehaviour main){}
        public void BOnApplicationQuit(MonoBehaviour main){}
        public void BOnEnable(MonoBehaviour main){}
        public void BStart(MonoBehaviour main){}
        public void BUpdate(MonoBehaviour main)
        {
            if (LuaEnvRoot != null)
                LuaEnvRoot.Tick();
        }

        public void BOnDestroy(MonoBehaviour main)
        {
            this.Disable();
        }
    }


}
