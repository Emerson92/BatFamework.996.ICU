using ProtoBuf;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.Utility
{
    public class BFrameSyncUtility
    {
        /// <summary>
        /// tranform the seralizalbe to the bytes array by protobuf tool
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="seralizable"></param>
        /// <returns></returns>
        public static byte[] NSeralizableClassTobytes<T>(T seralizable) where T : class {
            try
            {
                if (seralizable == null) return null;
                using (MemoryStream stream = new MemoryStream())
                {
                    Serializer.Serialize<T>(stream, seralizable);
                    return stream.ToArray();
                }

            }
            catch (Exception ex) {
                Debug.LogError(ex.StackTrace);
                return null;
            }

        }

        /// <summary>
        /// transform the bytes array to seralizalbe class by protobuf tool
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T NBytesToSeralizableClass<T>(byte[] data) where T :class {
            try
            {
                if (data == null) return null;
                using (MemoryStream stream = new MemoryStream(data))
                {
                    T Sclass = Serializer.Deserialize<T>(stream);
                    return Sclass;
                }
            }
            catch (Exception ex) {
                Debug.LogError(ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// tranform the seralizalbe to the bytes array by BinaryFormatter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="seralizable"></param>
        /// <returns></returns>
        public static byte[] SeralizableClassTobytes<T>(T seralizable) where T : class
        {
            try
            {
                if (seralizable == null) return null;
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter bm = new BinaryFormatter();
                    bm.Serialize(stream, seralizable);
                    return stream.ToArray();
                }

            }
            catch (Exception ex)
            {
                Debug.LogError(ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// transform the bytes array to seralizalbe class by BinaryFormatter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T BytesToSeralizableClass<T>(byte[] data) where T : class
        {
            try
            {
                if (data == null) return null;
                using (MemoryStream stream = new MemoryStream(data))
                {
                    BinaryFormatter bm = new BinaryFormatter();
                    T Sclass = bm.Deserialize(stream) as T;
                    return Sclass;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.StackTrace);
                return null;
            }
        }

        public static IExtensible Decode(string protoName, byte[] bytes, int offset, int count)
        {

            using (var memory = new MemoryStream(bytes, offset, count))
            {
                Type t = Type.GetType(protoName);
                return (IExtensible)Serializer.NonGeneric.Deserialize(t, memory);
            }
        }

        public static byte[] Encode(IExtensible proto)
        {
            using (var meomry = new MemoryStream())
            {
                Serializer.Serialize(meomry, proto);
                return meomry.ToArray();
            }
        }
    }
}
