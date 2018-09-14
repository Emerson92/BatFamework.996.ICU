using System.Collections;
using System.Collections.Generic;

namespace THEDARKKNIGHT.Interface {

    public interface ICacheFileSystem
    {
        void SaveData(string name,string path, byte[] data);

        byte[] LoadData(string path);
    }
}
