using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.Lua;
using UnityEngine;
using XLua;

namespace THEDARKKNIGHT.ProcessCore.Lua {
    public class LuaBProcessItem : BProcessItem
    {
        public Action<object,object> AssetInitCallback;
   
        public Action<object, object> DataInitCallback;
        private Action LuaDestory;
        private Action LuaFixedUpdate;
        private Action LuaProcessExcute;
        private Action LuaStopExcute;
        private Action LuaUpdate;
        private LuaTable scriptEnv;
        private string LuaPathName;
        public LuaBProcessItem(string luaPathName, string name)
        {
            this.TaskName = name;
            LuaPathName = luaPathName;
        }

        private void LuaInit(string luaPath)
        {
            BLuaControl.Instance().LoadLuaFile(luaPath);
            scriptEnv = BLuaControl.Instance().LoadFileWithCondition(luaPath, (param) =>
            {
                param.Set("self", this);
            });
            scriptEnv.Get("AssetInit", out AssetInitCallback);
            scriptEnv.Get("DataInit", out DataInitCallback);
            scriptEnv.Get("Destory", out LuaDestory);
            scriptEnv.Get("FixedUpdate", out LuaFixedUpdate);
            scriptEnv.Get("ProcessExcute", out LuaProcessExcute);
            scriptEnv.Get("StopExcute", out LuaStopExcute);
            scriptEnv.Get("Update", out LuaUpdate);
        }

        public override void AssetInit(object data)
        {
            LuaInit(LuaPathName);
            if (AssetInitCallback != null)
                AssetInitCallback(data,this);
        }

        public override void DataInit(object data)
        {
            if (DataInitCallback != null)
                DataInitCallback(data,this);
        }

        public override void Destory()
        {
            if (LuaDestory!=null)
                LuaDestory();
            if (scriptEnv !=null)
                scriptEnv.Dispose();
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
            if (LuaFixedUpdate != null)
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
            if (LuaUpdate != null)
                LuaUpdate();
        }

        public override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            base.OnDestroy();
            Debug.Log(this.TaskName + " OnDestroy ");

            if (LuaDestory != null)
                LuaDestory();
            if (scriptEnv != null)
                scriptEnv.Dispose();
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

