using THEDARKKNIGHT.SyncSystem.FrameSync.Utility;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.Interface
{
    public interface IRoallbackable
    {
        void TakeSnapshot(SnapshotWriter writer);

        void RollbackTo(SnapshotReader reader);

        void DistributeSnapshot(SnapshotReader reader);
    }
}
