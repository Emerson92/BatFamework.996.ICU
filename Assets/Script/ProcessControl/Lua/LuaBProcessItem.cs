using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using THEDARKKNIGHT.ConstDefine;
using THEDARKKNIGHT.Lua;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace THEDARKKNIGHT.ProcessCore.Lua
{
    public class LuaBProcessItem : BProcessItem
    {
        public Action<object, object> AssetInitCallback;

        public Action<object, object> DataInitCallback;
        private Action LuaDestory;
        private Action LuaFixedUpdate;
        private Action LuaProcessExcute;
        private Action LuaStopExcute;
        private Action LuaUpdate;
        public string LuaPathName;
        public LuaEnv LuaEnvRoot = new LuaEnv();
        private bool IsReadyUpdate = false;
        LuaTable scriptEnv = null;
        public LuaBProcessItem(string luaPath, string name)
        {
            this.TaskName = name;
            this.LuaPathName = luaPath;
        }

        private LuaTable LoadFileWithCondition(string fileName, Action<LuaTable> callBack = null)
        {
            scriptEnv = LuaEnvRoot.NewTable();
            LuaTable meta = LuaEnvRoot.NewTable();
            meta.Set("__index", LuaEnvRoot.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose();
            LoadFile(LuaFileLoad(LuaPathName), fileName, scriptEnv);
            if (callBack != null)
                callBack(scriptEnv);
       
            return scriptEnv;
        }

        private void LoadFile(byte[] bytes, string fileName, LuaTable table)
        {
            LuaEnvRoot.DoString(bytes, fileName, table);
        }

        private byte[] LuaFileLoad(string path)
        {
            //@"F:\5.6.1\FishingJoy\AssetBundles\" + fileName + ".lua.txt"
            return System.Text.Encoding.UTF8.GetBytes(File.ReadAllText(path));
        }

        private void LuaInit(string luaPath)
        {
            LoadFileWithCondition(luaPath, (scriptEnv) =>
            {
                scriptEnv.Get("AssetInit", out AssetInitCallback);
                scriptEnv.Get("DataInit", out DataInitCallback);
                scriptEnv.Get("Destory", out LuaDestory);
                scriptEnv.Get("FixedUpdate", out LuaFixedUpdate);
                scriptEnv.Get("ProcessExcute", out LuaProcessExcute);
                scriptEnv.Get("StopExcute", out LuaStopExcute);
                scriptEnv.Get("Update", out LuaUpdate);
            });
        }

        public override void AssetInit(object data)
        {
            LuaInit(LuaPathName);
            if (AssetInitCallback != null)
                AssetInitCallback(data, this);
        }

        public override void DataInit(object data)
        {
            if (DataInitCallback != null)
                DataInitCallback(data, this);
            IsReadyUpdate = true; 
        }

        public override void Destory()
        {
            if (scriptEnv != null)
                scriptEnv.Dispose();
            if (LuaDestory != null)
                LuaDestory();
            AssetInitCallback = null;
            DataInitCallback = null;
            LuaFixedUpdate = null;
            LuaProcessExcute = null;
            LuaStopExcute = null;
            LuaUpdate = null;
            LuaDestory = null;
        }

        public override void FixedUpdate()
        {
            if (LuaFixedUpdate != null && IsReadyUpdate)
                LuaFixedUpdate();
        }

        public override void ProcessExcute()
        {
            if (LuaProcessExcute != null)
                LuaProcessExcute();
        }

        public override void StopExcute()
        {
            if (LuaStopExcute != null)
                LuaStopExcute();
        }

        public override void Update()
        {
            if (LuaUpdate != null && IsReadyUpdate)
                LuaUpdate();
        }

        public override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            base.OnDestroy();
            Debug.Log(this.TaskName + " OnDestroy ");

            if (LuaDestory != null)
                LuaDestory();
            AssetInitCallback = null;
            DataInitCallback = null;
            LuaFixedUpdate = null;
            LuaProcessExcute = null;
            LuaStopExcute = null;
            LuaUpdate = null;
            LuaDestory = null;
        }


        public override void OnDestroy()
        {

        }
    }
}

