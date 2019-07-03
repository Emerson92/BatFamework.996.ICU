using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.SyncSystem.FrameSync.BBuffer;
using THEDARKKNIGHT.SyncSystem.FrameSync.BStruct;
using UnityEngine;

namespace THEDARKKNIGHT.SyncSystem.FrameSync.Interface {


    public interface ISyncComponent 
    {

        BSyncComponentCore.SYNCTYPE GetComponentType();

        BNOperateCommend UpdateLogic(int frameCount);

        void Update(float interpolationValue);

        bool UpdateByNet(uint NframeCount, BFrameCommend data);

    }
}

