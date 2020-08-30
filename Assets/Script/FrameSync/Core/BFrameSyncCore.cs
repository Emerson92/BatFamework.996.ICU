using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.SyncSystem.FrameSync {

    public abstract class BFrameSyncCore
    {
        /// <summary>
        /// the total time
        /// </summary>
        float accumilateTime = 0;

        /// <summary>
        /// the next time length
        /// </summary>
        float nextGameTime = 0;

        /// <summary>
        /// each of the logic updat time
        /// </summary>
        public static readonly float frameLength = (float)Fix64.FromRaw(273);

        /// <summary>
        /// the lerp time 
        /// </summary>
        float interpolation = 0;

        private float deltaTime = 0.25f;

        public float DeltaTime {
            set {
                deltaTime = value;
            }
            get {
                return deltaTime;
            }
        }

        /// <summary>
        /// the frameLogic account
        /// </summary>
        public static int GameLogicFrame = 0;

        /// <summary>
        /// the network of frame
        /// </summary>
        public static int GameNetworkFrame = 0;

        public static int LogicFrameRate => (int)(1 / frameLength);

        protected void UpdateLogic() {

            accumilateTime += deltaTime;

            while (accumilateTime > nextGameTime) {

                FrameLockLogic(GameLogicFrame);

                nextGameTime += frameLength;

                GameLogicFrame++;
            }

            interpolation = (accumilateTime + frameLength - nextGameTime) / frameLength;

            UpdateRender(interpolation);
        }

        /// <summary>
        /// Logic Frame Update Function
        /// </summary>
        public abstract void FrameLockLogic(int frameIndex);

        /// <summary>
        /// Render Frame Function
        /// </summary>
        /// <param name="interpolationValue"></param>
        public abstract void UpdateRender(float interpolationValue);
        
    }

}

