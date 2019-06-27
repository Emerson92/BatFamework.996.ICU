using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.SyncSystem.FrameSync.Buffer;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.Interface {


    public interface ISyncComponent 
    {

        void UpdateLogic(int frameCount);

        void Update(float interpolationValue);

        void networkLogicUpdate<T>(BFrame<T>[] data) where T :class;

    }
}

