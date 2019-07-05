using THEDARKKNIGHT.SyncSystem.FrameSync.BStruct.NetworkProtocol;

namespace THEDARKKNIGHT.SyncSystem.FrameSync.Interface
{


    public interface ISyncComponent
    {

        SYNCTYPE GetComponentType();

        BNOperateCommend UpdateLogic(int frameCount);

        void Update(float interpolationValue);

        bool UpdateByNet(uint NframeCount, byte[] data);

    }
}

