using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.SyncSystem.FrameSync.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.ExtendMethod
{
    public static partial class ExtendMethod
    {

        public static void EnableSync(this ISyncComponent i)
        {
            BFrameSyncSystem.SyncObjectGroup.Add(i);
        }

        public static void DisEnableSync(this ISyncComponent i)
        {
            BFrameSyncSystem.SyncObjectGroup.Remove(i);
        }

    }
}
