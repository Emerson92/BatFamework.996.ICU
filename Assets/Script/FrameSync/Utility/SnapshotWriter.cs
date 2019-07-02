using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.Utility {

    public class SnapshotWriter
    {
        private FastBinnayWriter writer;

        public SnapshotWriter() {


        }

        public void Init(Stream s) {
            writer.Init(s);
        }

        public void Reset() {

        }

        public void clear() {

        }

        public void Write(uint value) {
            writer.Write(value);
        }

        public void Write(int value) {
            writer.Write(value);
        }

        public void Write(bool value) {
            writer.Write(value);
        }

        public void Write(FixVector3 value) {
            //writer.Write(value);
        }

        public void Write(FixVector2 value) {

        }

        public void Flush() {
            writer.Flush();
        }
    }
}