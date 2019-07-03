using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.Interface {

    public interface IFrameConfirm 
    {


        bool FrameConfirm<T>(T data) where T : class;

    }

}
