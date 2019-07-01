using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.SyncSystem.FrameSync.Interface;
using THEDARKKNIGHT.SyncSystem.FrameSync.Utility;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.TimeMachine {
    /// <summary>
    /// 当前面某一帧的数据，通过服务器消息确认后（预测和实际操作一致），就可以丢弃这份快照（因为预测正确，不需要回滚到这份快照了），
    /// 将快照对应的memorysteam放到pool里重复使用。这样，
    /// 通常，根据延迟自动调整的逻辑，我们通常只保存5~10份快照。然后根据后续战斗结束比对快照的需求（防外挂的部分考虑），
    /// 每隔一段时间（比如30秒）保存一份快照。
    /// </summary>
    public class TimeMachine
    {

        public void TakeSnapshot(SnapshotWriter writer)
        {

        }

        public void RollbackTo(SnapshotReader reader)
        {
            
        }

    }

}

