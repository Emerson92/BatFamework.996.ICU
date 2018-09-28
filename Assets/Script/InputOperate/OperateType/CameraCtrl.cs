using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT;
using THEDARKKNIGHT.BatCore;
using THEDARKKNIGHT.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.InputOperate
{
    /// <summary>
    ///  Camera control Script 
    ///  this is script main to control the Camera status on the scene
    ///  such as the distance and rotation
    /// </summary>
    public class CameraCtrl : BatSingletion<CameraCtrl>, ILifeCycle
    {

        private float Radius = 5f;
        private Camera ObCamera;
        private Vector3 CameraLookDir;
        private Vector3 CenterPos;
        private float CurrentX = 0.0f;
        private float CurrentY = 0.0f;
        private float speedX = 15f;
        private float speedY = 7.5f;
        private float minLimitY = -360f;
        private float maxLimitY = 360f;
        private float LerpValue = 2.5f;
        private Quaternion TargetRotation;
        private Vector3 TargetPosition;
        private bool IsNeedLerp = false;

        public float TargetRadius { get; private set; }

        private CameraCtrl()
        {
            this.Enable();
        }

        public void SetCamera(Camera camera)
        {
            ObCamera = camera;
        }

        public Vector3 GetCenterPos()
        {
            return CenterPos;
        }

        /// <summary>
        ///  Set radius value
        /// </summary>
        /// <param name="radius"></param>
        public void SetObserverRadius(float radius)
        {
            Radius = radius;
        }

        /// <summary>
        ///  Focus the center to a point
        /// </summary>
        /// <param name="center"></param>
        public void FocusCenter(Vector3 center)
        {
            if (ObCamera == null) return;
            CenterPos = center;
            Vector3 dir = -ObCamera.transform.forward * Radius;
            Vector3 targetPos = center + dir;
            ObCamera.transform.position = targetPos;
            ObCamera.transform.forward = -dir;
            Vector3 angle = ObCamera.transform.eulerAngles;
            CurrentX = angle.y;
            CurrentY = angle.x;
            IsNeedLerp = false;
        }

        public void LerpFocusCenter(Vector3 center)
        {
            if (ObCamera == null) return;
            CenterPos = center;
            Vector3 dir = -ObCamera.transform.forward * Radius;
            Vector3 targetPos = center + dir;
            Vector3 angle = ObCamera.transform.eulerAngles;
            CurrentX = angle.y;
            CurrentY = angle.x;
            TargetRotation = Quaternion.LookRotation(-dir);
            TargetPosition = targetPos;
            IsNeedLerp = true;
        }

        /// <summary>
        ///  Rotate the camera rotation
        /// </summary>
        /// <param name="detlaValue"></param>
        public void RotateWithScreenPixl(Vector3 detlaValue)
        {
            //**************************************************************
            //             摄像机旋转问，需要进一步优化
            //              主要问题是摄像机的来回抖动
            //**************************************************************
            //Vector3 worldDir = ObCamera.transform.TransformDirection(detlaValue);
            //Vector3 targetDir = (worldDir - ObCamera.transform.forward * Radius).normalized * Radius;
            //Vector3 targePos = CenterPos + targetDir;
            ////Quaternion nowStatus = ObCamera.transform.rotation;
            //Debug.DrawLine(ObCamera.transform.position, targePos, Color.green, 10f);
            //ObCamera.transform.position = targePos;    
            //Quaternion transQuater = Quaternion.FromToRotation(ObCamera.transform.forward, -targetDir);
            //ObCamera.transform.forward = -targetDir;
            //ObCamera.transform.rotation *= transQuater;
            if (ObCamera == null) return;
            CurrentX += detlaValue.x * speedX * Time.deltaTime;
            CurrentY -= detlaValue.y * speedY * Time.deltaTime;
            CurrentY = ClampAngle(CurrentY, minLimitY, maxLimitY);
            Quaternion rotation = Quaternion.Euler(CurrentY, CurrentX, 0);
            Vector3 position = rotation * new Vector3(0, 0, -Radius) + CenterPos;
            ObCamera.transform.rotation = rotation;
            ObCamera.transform.position = position;
            IsNeedLerp = false;
        }

        public void LerpRotateWithScreenPixl(Vector3 detlaValue)
        {
            if (ObCamera == null) return;
            CurrentX += detlaValue.x * speedX * Time.deltaTime;
            CurrentY -= detlaValue.y * speedY * Time.deltaTime;
            CurrentY = ClampAngle(CurrentY, minLimitY, maxLimitY);
            Quaternion rotation = Quaternion.Euler(CurrentY, CurrentX, 0);
            Vector3 position = rotation * new Vector3(0, 0, -Radius) + CenterPos;
            TargetRotation = rotation;
            TargetPosition = position;
            IsNeedLerp = true;
        }

        /// <summary>
        /// move the CenterPos 
        /// </summary>
        /// <param name="deltaValue"></param>
        public void MoveWithScreenPixl(Vector3 screendeltaValue)
        {
            if (ObCamera == null) return;
            Vector3 centerScreenPos = ObCamera.WorldToScreenPoint(CenterPos);
            Vector3 moveTargetPos = ObCamera.ScreenToWorldPoint(centerScreenPos - screendeltaValue);
            FocusCenter(moveTargetPos);
        }

        public void LerpMoveWithScreenPixl(Vector3 screendeltaValue)
        {
            if (ObCamera == null) return;
            Vector3 centerScreenPos = ObCamera.WorldToScreenPoint(CenterPos);
            Vector3 moveTargetPos = ObCamera.ScreenToWorldPoint(centerScreenPos - screendeltaValue);
            LerpFocusCenter(moveTargetPos);
            IsNeedLerp = true;
        }

        /// <summary>
        ///  Change the Camera distance to the CenterPos
        /// </summary>
        /// <param name="value"></param>
        public void Scale(float value)
        {
            if (ObCamera == null) return;
            Radius += value;
            Vector3 dir = -ObCamera.transform.forward * Radius;
            Vector3 targetPos = CenterPos + dir;
            ObCamera.transform.position = targetPos;
        }

        public void LerpScale(float value)
        {
            if (ObCamera == null) return;
            Radius += value;
            Vector3 dir = -ObCamera.transform.forward * Radius;
            Vector3 targetPos = CenterPos + dir;
            TargetPosition = targetPos;
            IsNeedLerp = true;
        }

        public void RestCamera(Vector3 position, Quaternion rotation)
        {
            if (ObCamera == null) return;
            IsNeedLerp = false;
            ObCamera.transform.position = position;
            ObCamera.transform.rotation = rotation;
        }

        public void LerpRestCamera(Vector3 position, Quaternion rotation)
        {
            if (ObCamera == null) return;
            IsNeedLerp = true;
            TargetPosition = position;
            TargetRotation = rotation;
        }

        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360) angle += 360;
            if (angle > 360) angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }

        public void BAwake(MonoBehaviour main)
        {
            LifeCycleTool tool = this.GetLifeCycleTool();
            tool.SetLifeCycle(LifeCycleTool.LifeType.LateUpdate, true);
        }

        public void BStart(MonoBehaviour main) { }

        public void BOnEnable(MonoBehaviour main) { }

        public void BDisable(MonoBehaviour main) { }

        public void BOnDestory(MonoBehaviour main) { }

        public void BFixedUpdate(MonoBehaviour main) { }

        public void BLateUpdate(MonoBehaviour main)
        {
            if (IsNeedLerp)
            {
                ObCamera.transform.rotation = Quaternion.Lerp(ObCamera.transform.rotation, TargetRotation, Time.deltaTime * LerpValue);
                ObCamera.transform.position = Vector3.Lerp(ObCamera.transform.position, TargetPosition, Time.deltaTime * LerpValue);
                if (ObCamera.transform.rotation == TargetRotation && ObCamera.transform.position == TargetPosition)
                    IsNeedLerp = false;
            }
        }

        public void BOnApplicationFocus(MonoBehaviour main) { }
        public void BOnApplicationPause(MonoBehaviour main) { }
        public void BOnApplicationQuit(MonoBehaviour main) { }
        public void BOnDestroy(MonoBehaviour main) { }

        public void BUpdate(MonoBehaviour main) { }
    }
}
