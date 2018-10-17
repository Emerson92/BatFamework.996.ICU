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

        public LOADTYPE LoadFile = LOADTYPE.LOCALFILE;

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

        private byte[] LuaFileLoad(ref string fileName){
            //@"F:\5.6.1\FishingJoy\AssetBundles\" + fileName + ".lua.txt"
            switch (LoadFile) {
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
            if (LuaEnvRoot != null)
            {
                LuaEnvRoot.Dispose();
            }
        }
    }


}
