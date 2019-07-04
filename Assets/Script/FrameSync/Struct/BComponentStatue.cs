using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.SyncSystem.FrameSync.BStruct;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.Snapshot.BStruct
{
    [Serializable]
    public class BTransformComp
    {

        public int ComponentID;

        public COMPONENTLIFECYCLE statue;

        public FixVector3 Position;

        public FixVector3 EluerAngle;

        public FixVector3 Direction;

        public BFrameTransformCmd Cmd;
    }

}
