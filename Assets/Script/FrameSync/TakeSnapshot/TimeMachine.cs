using System.Collections.Generic;
using System.IO;
using THEDARKKNIGHT.SyncSystem.FrameSync.Interface;
using THEDARKKNIGHT.SyncSystem.FrameSync.Utility;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.Snapshot
{
    /// <summary>
    ////just TimeMachine,you knew it what meaning
    /// </summary>
    public class TimeMachine
    {
        public static List<IRoallbackable> ComponentList = new List<IRoallbackable>();

        public static readonly uint SaveSnapshotIntervalFrame = (uint)(5 * BFrameSyncCore.LogicFrameRate);////cycle time

        private SortedDictionary<uint, MemoryStream> saveSnapshotDic = new SortedDictionary<uint, MemoryStream>();////every cycle we will record the snapshot
        private List<MemoryStream> frameSnapshot = new List<MemoryStream>();////every frame snapshot
        private Queue<MemoryStream> streamPool = new Queue<MemoryStream>();////this is a small memoryStream pool;
        private SnapshotWriter snapshotWriter = new SnapshotWriter();
        private SnapshotReader snapshotReader = new SnapshotReader();
        private uint lastConfirmFrame = 0;

        public void TakeSnapshot()
        {
            uint currFrame = (uint)BFrameSyncCore.GameLogicFrame;
            MemoryStream stream;
            if (currFrame < frameSnapshot.Count)
            {
                stream = frameSnapshot[(int)currFrame];
                if (stream == null)
                {
                    if (streamPool.Count > 0)
                    {
                        stream = streamPool.Dequeue();
                    }
                    else
                    {
                        stream = new MemoryStream();
                    }
                }
            }
            else
            {
                if (streamPool.Count > 0)
                {
                    stream = streamPool.Dequeue();
                }
                else {
                    stream = new MemoryStream();
                }
                frameSnapshot.Add(stream);
            }
            stream.Position = 0;
            /////Tell the every component take snapshot
            snapshotWriter.Init(stream);
            for (int i = 0; i < ComponentList.Count; i++) {
                ComponentList[i].TakeSnapshot(snapshotWriter);
            }
            snapshotWriter.Flush();
        }

        /// <summary>
        /// get the current frame snapshot
        /// </summary>
        /// <param name="frameIndex"></param>
        public void GetFrameSnapshot(uint frameIndex) {
            if (frameIndex >= frameSnapshot.Count)
            {
                Debug.Log("try get this frame,but frame's count is less than Server frame:" + frameIndex);
                for (int i = 0; i < ComponentList.Count; i++)
                {
                    ComponentList[i].DistributeSnapshot(null);
                }
            }
            else {
                MemoryStream curSnapshot = frameSnapshot[(int)frameIndex];
                snapshotReader.Init(curSnapshot);
                for (int i = 0; i < ComponentList.Count; i++)
                {
                    ComponentList[i].DistributeSnapshot(snapshotReader);
                }
                snapshotReader.Reset();
            }
        }

        public void RollbackTo(uint frameIndex)
        { 
            if (frameIndex >= frameSnapshot.Count) {
                Debug.Log("try roll back to 没跑的过帧:"+frameIndex);
                return;
            }
            MemoryStream data = frameSnapshot[(int)frameIndex];
            if (data == null) {
                Debug.Log("this frame"+frameIndex+"snapshot released,can not roll back");
            }
            /////Tell the every component rollback to this frame
            snapshotReader.Init(data);
            for (int i = 0 ; i < ComponentList.Count ; i++) {
                ComponentList[i].RollbackTo(snapshotReader);
            }
            snapshotReader.Reset();

        }

        /// <summary>
        /// confire this frame data from server
        /// </summary>
        /// <param name="value"></param>
        public void ConfiremedFrame(uint value) {
            if (value <= lastConfirmFrame) {
                return;
            }
            for (uint i = lastConfirmFrame; i < value; i++) {
                if (i > frameSnapshot.Count) {
                    lastConfirmFrame = (uint)(frameSnapshot.Count - 1);
                    return;
                }
                MemoryStream stream = frameSnapshot[(int)i];
                if (stream == null) {
                    Debug.Log(BFrameSyncCore.GameLogicFrame+"----------------errro confirm snapshot----------------------"+value);
                }
                frameSnapshot[(int)i] = null;
                if (i % SaveSnapshotIntervalFrame == 0)
                {
                    if (saveSnapshotDic.ContainsKey(i))
                    {
                        saveSnapshotDic[i] = stream;
                    }
                    else
                    {
                        saveSnapshotDic.Add(i, stream);
                    }

                }
                else {
                    streamPool.Enqueue(stream);
                }
            }
        }
    }

}

