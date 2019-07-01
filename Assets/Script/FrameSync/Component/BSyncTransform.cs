using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.SyncSystem.FrameSync.BBuffer;
using THEDARKKNIGHT.SyncSystem.FrameSync.Struct;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.Component {

    /// <summary>
    /// Sync the GameObject of the Transform by the Frame Sync Framework
    /// </summary>
    public class BSyncTransform : BSyncComponentCore
    {

        private FixVector3 NtargetPos;
        private FixVector3 NtargetRot;
        private FixVector3 NtargetScale;

        private FixVector3? LtargetPos;
        private FixVector3? LtargetRot;
        private FixVector3? LtargetScale;

        private Transform TargetTransform;

        public BSyncTransform( uint componentID, Transform target) : base(componentID,SYNCTYPE.TRANSFORM)
        {
            this.TargetTransform = target;
        }

        public void SyncOperate(FixVector3? pos, FixVector3? rot, FixVector3? scale) {
            LtargetPos   = pos;
            LtargetRot   = rot;
            LtargetScale = scale;
        }

        public override void UpdateByNet(uint NframeCount, BFrameCommend data)
        {
            base.UpdateByNet(NframeCount, data);
            BFrameCommend cmd = GetNetworkCmd(NframeCount);
            NtargetPos   = cmd.TDirection + new FixVector3(TargetTransform.position);
            NtargetRot   = new FixVector3(TargetTransform.rotation * Quaternion.Euler(cmd.TRotation.ToVector3()).eulerAngles);
            NtargetScale = cmd.TScale + new FixVector3(TargetTransform.localScale);
        }

        public override BNOperateCommend UpdateLogic(int frameIndex)
        {
            ////TODO Create the new Commend
            BFrame<BFrameCommend> localFrame = new BFrame<BFrameCommend>();
            BFrameCommend lcmd = new BFrameCommend();
            if (LtargetPos != null)   lcmd.TDirection  = LtargetPos.GetValueOrDefault()   - new FixVector3(TargetTransform.position);
            if (LtargetRot != null)   lcmd.TRotation   = LtargetRot.GetValueOrDefault()   - new FixVector3(TargetTransform.rotation.eulerAngles);
            if (LtargetScale != null) lcmd.TScale      = LtargetScale.GetValueOrDefault() - new FixVector3(TargetTransform.localScale);
            localFrame.FrameNum = (uint)frameIndex;
            LocalFrameBuffer.EnQuene(localFrame);
            LtargetPos   = null;
            LtargetRot   = null;
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
            TargetTransform.position   = Vector3.Lerp(TargetTransform.position, NtargetPos.ToVector3(), interpolationValue);
            TargetTransform.rotation   = Quaternion.Euler(Vector3.Lerp(TargetTransform.rotation.eulerAngles, NtargetRot.ToVector3(), interpolationValue));
            TargetTransform.localScale = Vector3.Lerp(TargetTransform.localScale, NtargetScale.ToVector3(), interpolationValue);
        }

        /// <summary>
        /// Disable Transform Sync
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            TargetTransform = null;
        }

    }

}
