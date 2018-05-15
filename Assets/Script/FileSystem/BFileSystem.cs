using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace THEDARKKNIGHT.FileSystem
{
    public class BFileSystem : BatSingletion<BFileSystem>
    {

        private BFileSystem() { }

        public FileStream CreateFile(string path, string fileName)
        {
            FileStream file = null;
            try
            {
                file = File.Create(path + "/" + fileName);
            }
            catch (Exception ex)
            {
                BLog.Instance().Log(ex.Message);
            }

            return file;
        }

        public FileStream OpenFile(string path, string FileName)
        {
            FileStream file = null;
            try
            {
                file = OpenFile(path + "/" + FileName);
            }
            catch (Exception ex)
            {
                BLog.Instance().Log(ex.Message);
            }
            return file;
        }

        public FileStream OpenFile(string fullPath)
        {
            FileStream file = null;
            try
            {
                file = File.Open(fullPath, FileMode.Append);
            }
            catch (Exception ex) {
                BLog.Instance().Log(ex.Message);
            }
            
            return file;
        }

        public void DeleteFile(string fullpath, bool deleteAll = false)
        {
            try
            {
                File.Delete(fullpath);
            }
            catch (Exception ex)
            {
                BLog.Instance().Log(ex.Message);
            }

        }

        public void MoveFile(string targetPath, string destinationPath)
        {
            try
            {
                File.Move(targetPath, destinationPath);
            }
            catch (Exception ex) {
                BLog.Instance().Log(ex.Message);
            }
           
        }

        public void CopyFile(string targetPath, string destinationPath)
        {
            try
            {
                File.Copy(targetPath, destinationPath);
            }
            catch (Exception ex) {
                BLog.Instance().Log(ex.Message);
            }
        }
    }
}
