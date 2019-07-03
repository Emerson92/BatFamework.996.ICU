using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
            writer = null;
        }

        public void clear() {
            writer.clear();
            writer = null;
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
            Fix64 x = value.x;
            Fix64 y = value.y;
            Fix64 z = value.z;
            writer.Write(x.RawValue);
            writer.Write(y.RawValue);
            writer.Write(z.RawValue);
        }

        public void Write(FixVector2 value) {
            Fix64 x = value.x;
            Fix64 y = value.y;
            writer.Write(x.RawValue);
            writer.Write(y.RawValue);
        }

        public void Write(string value) {
            writer.Write(value);
        }

        /// <summary>
        /// create the serializble the class to the memorystream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializable"></param>
        public void Write<T>(T serializable) where T : class {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream()) {
                formatter.Serialize(stream, serializable);
                byte[] value = stream.GetBuffer();
                writer.Write(value);
            }
                
        }

        /// <summary>
        /// flush the data from 
        /// </summary>
        public void Flush() {
            writer.Flush();
        }
    }
}