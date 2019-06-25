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
        float frameLength = 0;

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
        public abstract void FrameLockLogic(int frameConut);

        /// <summary>
        /// Render Frame Function
        /// </summary>
        /// <param name="interpolationValue"></param>
        public abstract void UpdateRender(float interpolationValue);
        
    }

}

