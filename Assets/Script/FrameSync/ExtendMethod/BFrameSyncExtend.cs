using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.SyncSystem.FrameSync.Interface;
using THEDARKKNIGHT.SyncSystem.FrameSync.Snapshot;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.ExtendMethod
{
    public static partial class ExtendMethod
    {

        public static void EnableSync(this ISyncComponent i) 
        {
            BFrameSyncSystem.SyncObjectGroup.Add(i);
        }

        public static void DisableSync(this ISyncComponent i)
        {
            BFrameSyncSystem.SyncObjectGroup.Remove(i);
        }

        public static void EnalbeSnapshot(this IRoallbackable i)
        {
            TimeMachine.ComponentList.Add(i);
        }

        public static void DisalbeSnapshot(this IRoallbackable i)
        {
            //TimeMachine.ComponentList.Remove(i);
        }
    }
}
