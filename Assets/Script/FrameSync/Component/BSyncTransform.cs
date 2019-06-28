using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.SyncSystem.FrameSync.Struct;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync.Component {

    /// <summary>
    /// Sync the GameObject of the Transform by the Frame Sync Framework
    /// </summary>
    public class BSyncTransform : BSyncComponentCore
    {

        public BSyncTransform(uint componentID) : base(componentID,SYNCTYPE.TRANSFORM)
        {

        }

        public override void UpdateByNet(uint NframeCount, BFrameCommend data)
        {
            base.UpdateByNet(NframeCount, data);
        }

        public override BNOperateCommend UpdateLogic(int frameCount)
        {
            return base.UpdateLogic(frameCount);
        }

        /// <summary>
        /// lerp value in Render Update
        /// </summary>
        /// <param name="interpolationValue"></param>
        public override void Update(float interpolationValue)
        {
            base.Update(interpolationValue);
        }
    }

}
