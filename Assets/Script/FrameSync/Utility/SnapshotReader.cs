using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.Utility {

    public class SnapshotReader
    {
        private FastBinnayReader reader;

        private long rawPosition;
        private Stream s;

        public SnapshotReader() {

        }
        public void Init(Stream s) {
            reader.Init(s);
        }

        public void Reset() {
            reader = null;
        }

        public void Clear() {
            reader.clear();
            reader = null;
        }

        public void ReadUnit() {
            reader.ReadUInt32();
        }

        public void ReadInt() {
            reader.ReadInt32();
        }

        public bool ReadBool() {
            return reader.ReadBoolean();
        }

        public T ReadSerailiable<T>() where T : class {
            BinaryFormatter bFormatter = new BinaryFormatter();
            using (MemoryStream s = new MemoryStream())
            {
                byte[] data = reader.ReadByteArray();
                s.Write(data,0, data.Length);
                return (T)bFormatter.Deserialize(s);
            }
        }

        public FixVector3 ReadFixVector3() {
            long x = reader.ReadInt64();
            long y = reader.ReadInt64();
            long z = reader.ReadInt64();
            FixVector3 vector3 = new FixVector3(Fix64.FromRaw(x), Fix64.FromRaw(y), Fix64.FromRaw(z));
            return vector3;
        }

        public FixVector2 ReadFixVector2() {
            long x = reader.ReadInt64();
            long y = reader.ReadInt64();
            FixVector2 vector3 = new FixVector2(Fix64.FromRaw(x), Fix64.FromRaw(y));
            return FixVector2.Zero;
        }

    }
}
