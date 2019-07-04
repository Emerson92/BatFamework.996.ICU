using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.BStruct {

    [Serializable]
    public class BNFrameCommdend {
        ////from the network data
        public int NFrameNum;

        public List<BNOperateCommend> OperateCmd;
    }

    [Serializable]
    public class BNOperateCommend
    {

        public SYNCTYPE OperateType;

        public uint ComponentID;

        public object cmd;/////// you must considert the extend ability
        ///////Wait to finish data

    }

    [Serializable]
    public class BFrameTransformCmd
    {
        //////Wait to extend data struct
        public FixVector3 TDirection;
        public FixVector3 TRotation;
        public FixVector3 TScale;

    }
}

