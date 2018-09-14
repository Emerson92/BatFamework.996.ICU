using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.FileSystem;
using THEDARKKNIGHT.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.ResourceSystem {
    public class BCacheFileMgr : ICacheFileSystem
    {
        public byte[] LoadData(string path)
        {
            return BFileSystem.Instance().ReadFileFromDisk(path);
        }

        public void SaveData(string name,string path, byte[] data)
        {
            BFileSystem.Instance().WriteFileToDiskAsync(data, path, name,true);
        }
    }
}

