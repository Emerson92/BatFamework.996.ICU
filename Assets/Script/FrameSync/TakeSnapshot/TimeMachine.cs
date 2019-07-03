using System.Collections;
using System.Collections.Generic;
using System.IO;
using THEDARKKNIGHT.SyncSystem.FrameSync.Interface;
using THEDARKKNIGHT.SyncSystem.FrameSync.Utility;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.Snapshot
{
    /// <summary>
    /// 当前面某一帧的数据，通过服务器消息确认后（预测和实际操作一致），就可以丢弃这份快照（因为预测正确，不需要回滚到这份快照了），
    /// 将快照对应的memorysteam放到pool里重复使用。这样，
    /// 通常，根据延迟自动调整的逻辑，我们通常只保存5~10份快照。然后根据后续战斗结束比对快照的需求（防外挂的部分考虑），
    /// 每隔一段时间（比如30秒）保存一份快照。
    /// </summary>
    public class TimeMachine
    {
        public static List<IRoallbackable> ComponentList = new List<IRoallbackable>();

        public static readonly uint SaveSnapshotIntervalFrame = (uint)(5 * BFrameSyncCore.LogicFrameRate);

        private SortedDictionary<uint, MemoryStream> saveSnapshotDic = new SortedDictionary<uint, MemoryStream>();
        private List<MemoryStream> frameSnapshot = new List<MemoryStream>();
        private Queue<MemoryStream> streamPool = new Queue<MemoryStream>();
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

