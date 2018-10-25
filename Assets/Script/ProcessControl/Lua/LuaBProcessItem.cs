using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.Lua;
using UnityEngine;
using XLua;

namespace THEDARKKNIGHT.ProcessCore.Lua {
    public class LuaBProcessItem : BProcessItem
    {

        //private Action<object> LuaAssetInit;
        //private Action<object> LuaDataInit;

        [CSharpCallLua]
        public delegate int LuaAssetInit(object data, LuaBProcessItem ob);

        [CSharpCallLua]
        public delegate int LuaDataInit(object data, LuaBProcessItem ob);

        [CSharpCallLua]
        public LuaAssetInit AssetInitCallback;

        [CSharpCallLua]
        public LuaDataInit DataInitCallback;

        private Action LuaDestory;
        private Action LuaFixedUpdate;
        private Action LuaProcessExcute;
        private Action LuaStopExcute;
        private Action LuaUpdate;
        private LuaTable scriptEnv;

        public LuaBProcessItem(string luaPath, string name) {
            this.TaskName = name;
            BLuaControl.Instance().LoadLuaFile(luaPath);
            scriptEnv = BLuaControl.Instance().LoadFileWithCondition(luaPath);
            AssetInitCallback = BLuaControl.Instance().LuaEnvRoot.Global.Get<LuaAssetInit>("AssetInit");
            DataInitCallback  = BLuaControl.Instance().LuaEnvRoot.Global.Get<LuaDataInit> ("DataInit");
            scriptEnv.Set("self", this);
            scriptEnv.Get("Destory", out LuaDestory);
            scriptEnv.Get("FixedUpdate", out LuaFixedUpdate);
            scriptEnv.Get("ProcessExcute", out LuaProcessExcute);
            scriptEnv.Get("StopExcute", out LuaStopExcute);
            scriptEnv.Get("Update", out LuaUpdate);
        }


        public override void AssetInit(object data)
        {
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
    }
}

