using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.Interface {


    public interface ISyncComponent 
    {

        void UpdateLogic(int frameCount);

        void Update(float interpolationValue);

    }
}

