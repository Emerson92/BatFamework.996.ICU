using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        }

        public void Reset() {

        }

        public void Clear() {

        }

        public void ReadUnit() {

        }

        public void ReadInt() {

        }

        public bool ReadBool() {
            return true;
        }

        public FixVector3 ReadFixVector3() {
            return FixVector3.Zero;
        }

        public FixVector2 ReadFixVector2() {
            return FixVector2.Zero;
        }

    }
}
