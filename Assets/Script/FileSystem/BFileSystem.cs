using System;
using System.IO;
using System.Threading;
using THEDARKKNIGHT.BatCore;
using THEDARKKNIGHT.Log;
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
            catch (Exception ex)
            {
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
            catch (Exception ex)
            {
                BLog.Instance().Log(ex.Message);
            }

        }

        public void CopyFile(string targetPath, string destinationPath)
        {
            try
            {
                File.Copy(targetPath, destinationPath);
            }
            catch (Exception ex)
            {
                BLog.Instance().Log(ex.Message);
            }
        }

        public void WriteFileToDiskAsync(byte[] data, string path, string fileName, bool IsForceWriter = false)
        {
            try
            {
                string filePath = path + "/" + fileName;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (File.Exists(filePath))
                {
                    if (IsForceWriter)
                        DeleteFile(filePath);
                }
                new Thread(() =>
                {
                    using (FileStream steam = CreateFile(path, fileName))
                    {

                        try
                        {
                            steam.Write(data, 0, data.Length);
                            steam.Flush();
                            steam.Dispose();
                            BLog.Instance().Log("Write The File "+ fileName + " Finish!");
                        }
                        catch (Exception ex)
                        {
                            BLog.Instance().Log(ex.Message);
                        }

                    }
                }).Start();
            }
            catch (Exception ex)
            {
                BLog.Instance().Log(ex.Message);
            }
        }

        public byte[] ReadFileFromDisk(string path, string fileName)
        {
            string filePath = path + "/" + fileName;
            return ReadFileFromDisk(filePath);
        }

        public byte[] ReadFileFromDisk(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    FileStream stream = new FileStream(filePath, FileMode.Open);
                    using (BinaryReader read = new BinaryReader(stream))
                    {
                        byte[] data = read.ReadBytes((int)stream.Length);
                        return data;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                BLog.Instance().Log(ex.Message);
                return null;
            }
        }
    }
}
