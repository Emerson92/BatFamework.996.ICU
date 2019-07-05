using ProtoBuf;
using System;
using System.Collections.Generic;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.BStruct.NetworkProtocol
{

    [Serializable]
    [ProtoContract]
    public class BNFrameCommdend {

        ////from the network data
        [ProtoMember(1)]
        public int NFrameNum;

        [ProtoMember(2)]
        public List<BNOperateCommend> OperateCmd;
    }

    [Serializable]
    [ProtoContract]
    public class BNOperateCommend
    {
        [ProtoMember(1)]
        public SYNCTYPE OperateType;

        [ProtoMember(2)]
        public uint ComponentID;

        [ProtoMember(3)]
        public byte[] cmd;/////// you must considert the extend ability
    }

    [Serializable]
    [ProtoContract]
    public class BCmdBase {

    }

    [Serializable]
    [ProtoContract]
    public class BFrameTransformCmd : BCmdBase
    {
        //////Wait to extend data struct
        [ProtoMember(1)]
        public FixVector3 TDirection;

        [ProtoMember(2)]
        public FixVector3 TRotation;

        [ProtoMember(3)]
        public FixVector3 TScale;

    }

    [Serializable]
    [ProtoContract]
    public struct CommendPack {

        [ProtoMember(1)]
        public uint ComponentID;////Component ID

        [ProtoMember(2)]
        public uint CmdList; ////Cmd frame index
    }

    [Serializable]
    [ProtoContract]
    public struct CommendC2S {

        [ProtoMember(1)]
        public uint ComponentID;////Component ID

        [ProtoMember(2)]
        public uint StartFrame;////the Start frame index

        [ProtoMember(3)]
        public BNFrameCommdend Cmd;////The Operate Commend

        [ProtoMember(4)]
        public uint LastFrameIndex;////the Client reciver the last Frame package

        [ProtoMember(5)]
        public uint PackID;////the package ID

    }

    [Serializable]
    [ProtoContract]
    public struct CommendS2C {

        [ProtoMember(1)]
        public uint ServerFrame;////the server Current Frame index

        [ProtoMember(2)]
        public uint ServerStartFrame;////the server start frame index

        [ProtoMember(3)]
        public BNFrameCommdend Cmd;////the cmd broadcast cmd to client

        [ProtoMember(4)]
        public uint RecvPlayerFrame;////the server has recvier the client's the last frame index

        [ProtoMember(5)]
        public uint PackID;///the Package ID

        [ProtoMember(6)]
        public uint PackTimeOffset;////the time between send to recvier;

    }
}

