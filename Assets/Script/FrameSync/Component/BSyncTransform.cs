using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.SyncSystem.FrameSync.BBuffer;
using THEDARKKNIGHT.SyncSystem.FrameSync.BStruct;
using THEDARKKNIGHT.SyncSystem.FrameSync.Utility;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.Component
{

    /// <summary>
    /// Sync the GameObject of the Transform by the Frame Sync Framework
    /// </summary>
    public class BSyncTransform : BSyncComponentCore<BFrameTransformCmd>
    {

        private FixVector3? NtargetPos;
        private FixVector3? NtargetRot;
        private FixVector3? NtargetScale;

        private FixVector3? LtargetPos;
        private FixVector3? LtargetRot;
        private FixVector3? LtargetScale;

        private Transform TargetTransform;

        private FrameSnapshot checkSnapshot = new FrameSnapshot() {
            TtargetPos = FixVector3.Zero,
            TtargetRot = FixVector3.Zero,
            TtargetScale = FixVector3.Zero,
            FreshData = false
        };

        public Func<COMPONENTLIFECYCLE, Transform> RecoverTransfrom;

        public BSyncTransform(uint componentID, Transform target) : base(componentID, SYNCTYPE.TRANSFORM)
        {
            this.TargetTransform = target;
        }

        public void SyncOperate(FixVector3? pos, FixVector3? rot, FixVector3? scale)
        {
            LtargetPos = pos;
            LtargetRot = rot;
            LtargetScale = scale;
        }

        public override bool UpdateByNet(uint NframeCount, object data)
        {
            if (base.UpdateByNet(NframeCount, data))
            {
                return true;/////Server frame is not match the local predicting frame ,so we perpare to rollback current frame;
            }
            else
            {
                BFrameTransformCmd cmd = GetNetworkCmd(NframeCount);
                NtargetPos = cmd.TDirection + new FixVector3(TargetTransform.position);
                NtargetRot = new FixVector3(TargetTransform.rotation * Quaternion.Euler(cmd.TRotation.ToVector3()).eulerAngles);
                NtargetScale = cmd.TScale + new FixVector3(TargetTransform.localScale);
                return false;
            }
        }

        public override BNOperateCommend UpdateLogic(int frameIndex)
        {
            ////TODO Create the new Commend
            BFrame<BFrameTransformCmd> localFrame = new BFrame<BFrameTransformCmd>();
            BFrameTransformCmd lcmd = new BFrameTransformCmd();
            if (LtargetPos != null) lcmd.TDirection = LtargetPos.GetValueOrDefault() - new FixVector3(TargetTransform.position);
            if (LtargetRot != null) lcmd.TRotation = LtargetRot.GetValueOrDefault() - new FixVector3(TargetTransform.rotation.eulerAngles);
            if (LtargetScale != null) lcmd.TScale = LtargetScale.GetValueOrDefault() - new FixVector3(TargetTransform.localScale);
            localFrame.FrameNum = (uint)frameIndex;
            LocalFrameBuffer.EnQuene(localFrame);
            LtargetPos = null;
            LtargetRot = null;
            LtargetScale = null;
            return base.UpdateLogic(frameIndex);
        }

        /// <summary>
        /// lerp value in Render Update
        /// </summary>
        /// <param name="interpolationValue"></param>
        public override void Update(float interpolationValue)
        {
            base.Update(interpolationValue);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// if current Network frame excution finish ,we can predicted the tranform according to user input
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            TargetTransform.position = TransformLerp(TargetTransform.position, NtargetPos, LtargetPos, interpolationValue);
            TargetTransform.rotation = Quaternion.Euler(TransformLerp(TargetTransform.rotation.eulerAngles, NtargetRot, LtargetRot, interpolationValue)) ;
            TargetTransform.localScale = TransformLerp(TargetTransform.localScale, NtargetScale, LtargetScale, interpolationValue);
        }

        /// <summary>
        /// it method contain predicted frame and confirm frame from network
        /// </summary>
        /// <param name="curValue"></param>
        /// <param name="ntarValue"></param>
        /// <param name="ltarValue"></param>
        /// <param name="interpolationValue"></param>
        /// <returns></returns>
        private Vector3 TransformLerp(Vector3 curValue,FixVector3? ntarValue,FixVector3? ltarValue,float interpolationValue)
        {
            Vector3 lerpValue;
            if (NtargetPos != null)
            {
                lerpValue = Vector3.Lerp(curValue, ntarValue.GetValueOrDefault().ToVector3(), interpolationValue);
                if (curValue == ntarValue.GetValueOrDefault().ToVector3())////if the lerp network frame excute is finish,we start to excute predicting frame
                {
                    ntarValue = null;
                }
            }
            else
            {
                lerpValue = Vector3.Lerp(curValue, ltarValue.GetValueOrDefault().ToVector3(), interpolationValue);////predicting frame
            }
            return lerpValue;
        }

        /// <summary>
        /// Disable Transform Sync
        /// </summary>
        public override void Dispose()
        {
            TargetTransform = null;
            base.Dispose();
        }

        public new void TakeSnapshot(SnapshotWriter writer)
        {
            base.TakeSnapshot(writer);
            FixVector3? TDirection = null;
            FixVector3? TRotation = null;
            FixVector3? TScale = null;
            if (LtargetPos != null) TDirection = LtargetPos.GetValueOrDefault() - new FixVector3(TargetTransform.position);
            if (LtargetRot != null) TRotation = LtargetRot.GetValueOrDefault() - new FixVector3(TargetTransform.rotation.eulerAngles);
            if (LtargetScale != null) TScale = LtargetScale.GetValueOrDefault() - new FixVector3(TargetTransform.localScale);
            writer.Write(new FixVector3(TargetTransform.position));////Position
            writer.Write(new FixVector3(TargetTransform.rotation.eulerAngles));/////Rotation of angle(eluer)
            writer.Write(new FixVector3(TargetTransform.localScale));////Scale
            writer.Write(TDirection.GetValueOrDefault(FixVector3.Zero));////Move Direction
            writer.Write(TRotation.GetValueOrDefault(FixVector3.Zero));/////Rotation of angle(eluer)
            writer.Write(TScale.GetValueOrDefault(FixVector3.Zero));////Change of Scale
        }

        public override void RollbackTo(SnapshotReader reader)
        {
            base.RollbackTo(reader);
            switch (Statue)
            {
                case COMPONENTLIFECYCLE.DEATH://///if the tranform of the gameobject is destory , we should recreate it and give it right position 、rotation and scale
                    TargetTransform = RecoverTransfrom(Statue);
                    break;
                case COMPONENTLIFECYCLE.HIDDEN://///if the tranform of the gameobject is hidden,we should make itself show up,and give it right status
                    RecoverTransfrom(Statue);
                    break;
            }
            TargetTransform.position = reader.ReadFixVector3().ToVector3();////Position
            TargetTransform.rotation = Quaternion.Euler(reader.ReadFixVector3().ToVector3());/////Rotation of angle(eluer)
            TargetTransform.localScale = reader.ReadFixVector3().ToVector3();////Scale
        }

        public override bool FrameConfirm(uint NframeIndex, object data)
        {
            BFrameTransformCmd Scmd = (BFrameTransformCmd)data;
            if (!checkSnapshot.FreshData) {
                Debug.Log("we do not get the frame snapshot,so we follow the server frame status and roback the status! the frameIndex:"+ NframeIndex);
                return false;//////Rollback the frame
            }
            if ((Scmd.TDirection != checkSnapshot.TtargetPos) || (Scmd.TRotation != checkSnapshot.TtargetRot) || (Scmd.TRotation != checkSnapshot.TtargetScale))
            {
                Debug.Log(" the predicting frame snapshot is not match the Server frame !!! the frameIndex:" + NframeIndex);
                checkSnapshot.Reset();
                return false;//////Rollback the frame
            }
            else {
                checkSnapshot.Reset();
                return true;///////Continue to excute the frame
            }
              
        }

        /// <summary>
        /// get the predicting frame snapshot
        /// </summary>
        /// <param name="reader"></param>
        public override void DistributeSnapshot(SnapshotReader reader)
        {
            if (reader == null) checkSnapshot.Reset();
            FixVector3 position = reader.ReadFixVector3();////Position
            FixVector3 rotation = reader.ReadFixVector3();/////Rotation of angle(eluer)
            FixVector3 localScale = reader.ReadFixVector3();////Scale
            FixVector3 TtargetPos = reader.ReadFixVector3();//////recorde the transform move direction
            FixVector3 TtargetRot = reader.ReadFixVector3();//////recorde the transform rotate the angle
            FixVector3 TtargetScale = reader.ReadFixVector3();/////recorde the transform the change value of scale
            checkSnapshot.TtargetPos = TtargetPos;
            checkSnapshot.TtargetRot = TtargetRot;
            checkSnapshot.TtargetScale = TtargetScale;
            checkSnapshot.FreshData = true;
        }

        /// <summary>
        /// the predicting frame snapshot
        /// </summary>
        public struct FrameSnapshot{

            public FixVector3 TtargetPos;

            public FixVector3 TtargetRot;

            public FixVector3 TtargetScale;

            public bool FreshData;

            public void Reset() {
                FreshData = false;
            }
        }
    }

}
