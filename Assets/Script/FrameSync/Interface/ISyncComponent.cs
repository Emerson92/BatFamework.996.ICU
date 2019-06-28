using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.SyncSystem.FrameSync.Buffer;
using THEDARKKNIGHT.SyncSystem.FrameSync.Struct;
using UnityEngine;

namespace THEDARKKNIGHT.SyncSystem.FrameSync.Interface {


    public interface ISyncComponent 
    {

        BSyncComponentCore.SYNCTYPE GetComponentType();

        BNOperateCommend UpdateLogic(int frameCount);

        void Update(float interpolationValue);

        void UpdateByNet(uint NframeCount, BFrameCommend data);

    }
}

