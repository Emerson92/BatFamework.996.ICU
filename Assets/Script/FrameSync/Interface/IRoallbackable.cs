using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.SyncSystem.FrameSync.Utility;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.Interface
{
    public interface IRoallbackable
    {
        void TakeSnapshot(SnapshotWriter writer);

        void RollbackTo(SnapshotReader reader);

        void DistributeSnapshot(SnapshotReader reader);
    }
}
