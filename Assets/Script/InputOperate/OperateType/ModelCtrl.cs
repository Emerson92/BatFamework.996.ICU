using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.BatCore;
using THEDARKKNIGHT.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.InputOperate {
    public class ModelCtrl : BatSingletion<ModelCtrl>, ILifeCycle
    {
        private GameObject CurrentGB;
        private bool IsNeedLerp = false;
        private float ScaleFactor = (1 / 5f);
        private Quaternion TargetRotation;
        private Vector3 TargetPosition;
        public Vector3 TargetScale;
        private float LerpValue = 5f;

        private ModelCtrl() {
            this.Enable();
        }

        public void SetOperateModel(GameObject ob) {
            CurrentGB = ob;
            TargetScale = CurrentGB.transform.localScale;
            TargetPosition = CurrentGB.transform.position;
            TargetRotation = CurrentGB.transform.rotation;
            IsNeedLerp = false;
        }

        public void MoveWithScreenPixl(Vector3 detalValue, Camera camera) {
            if (CurrentGB == null) return;
            Vector3 obScreenPos = camera.WorldToScreenPoint(CurrentGB.transform.position);
            Vector3 finalPos = obScreenPos + detalValue;
            CurrentGB.transform.position = camera.ScreenToWorldPoint(finalPos);
            IsNeedLerp = false;
        }

        public void MoveWithWorldVector(Vector3 detalValue) {
            if (CurrentGB == null) return;
            CurrentGB.transform.position += detalValue;
            IsNeedLerp = false;
        }

        public void LerpMoveWithWorldVector(Vector3 detalValue) {
            if (CurrentGB == null) return;
            TargetPosition += detalValue;
            IsNeedLerp = true;
        }


        public void RotationWithScreenPixl(Vector3 detalValue) {
            if (CurrentGB == null) return;
            float angleX = 0;
            float angleY = 0;
            float ratioY = detalValue.x / Screen.width;
            angleY = ratioY * 180f;
            angleY %= 360;
            float ratioX = detalValue.y / Screen.height;
            angleX = ratioX * 180f;
            angleX %= 360;
            Quaternion preRotation = CurrentGB.transform.rotation;
            Vector3 localVecX = CurrentGB.transform.InverseTransformDirection(Vector3.right);      
            Quaternion final = preRotation;
            Quaternion quaternionX = Quaternion.AngleAxis(angleX, localVecX.normalized);
            Quaternion quaternionY = Quaternion.AngleAxis(-angleY, Vector3.up);
            CurrentGB.transform.rotation = preRotation* quaternionX* quaternionY;
            IsNeedLerp = false;
        }

        public void Scale(float value) {
            if (CurrentGB == null) return;
            CurrentGB.transform.localScale = CurrentGB.transform.localScale + CurrentGB.transform.localScale * value * ScaleFactor;
            IsNeedLerp = false;
        }



        public void LerpMoveWithScreenPixl(Vector3 detalValue, Camera camera) {
            if (CurrentGB == null) return;
            Vector3 obScreenPos = camera.WorldToScreenPoint(CurrentGB.transform.position);
            Vector3 finalPos = obScreenPos + detalValue;
            TargetPosition = camera.ScreenToWorldPoint(finalPos);
            TargetScale = CurrentGB.transform.localScale;
            TargetRotation = CurrentGB.transform.rotation;
            IsNeedLerp = true;
        }

        public void LerpRotationWithScreenPixl(Vector3 detalValue)
        {
            if (CurrentGB == null) return;
            float angleX = 0;
            float angleY = 0;
            float ratioY = detalValue.x / Screen.width;
            angleY = ratioY * 180f;
            angleY %= 360;
            float ratioX = detalValue.y / Screen.height;
            angleX = ratioX * 180f;
            angleX %= 360;
            Quaternion preRotation = CurrentGB.transform.rotation;
            Vector3 localVecX = CurrentGB.transform.InverseTransformDirection(Vector3.right);
            Quaternion final = preRotation;
            Quaternion quaternionX = Quaternion.AngleAxis(angleX, localVecX.normalized);
            Quaternion quaternionY = Quaternion.AngleAxis(-angleY, Vector3.up);
            TargetRotation = preRotation * quaternionX * quaternionY;
            IsNeedLerp = true;
        }

        public void LerpScale(float value)
        {
            if (CurrentGB == null) return;
            TargetPosition = CurrentGB.transform.position;
            TargetRotation = CurrentGB.transform.rotation;
            TargetScale = CurrentGB.transform.localScale + CurrentGB.transform.localScale * value * ScaleFactor;
            IsNeedLerp = true;
        }
        public void BAwake(MonoBehaviour main)
        {
            LifeCycleTool tool = this.GetLifeCycleTool();
            tool.SetLifeCycle(LifeCycleTool.LifeType.Update, true);
        }

        public void BStart(MonoBehaviour main){}
        public void BOnEnable(MonoBehaviour main){}
        public void BDisable(MonoBehaviour main){}
        public void BOnDestory(MonoBehaviour main){}
        public void BFixedUpdate(MonoBehaviour main){}
        public void BLateUpdate(MonoBehaviour main){}
        public void BOnApplicationFocus(MonoBehaviour main){}
        public void BOnApplicationPause(MonoBehaviour main){}
        public void BOnApplicationQuit(MonoBehaviour main){}
        public void BOnDestroy(MonoBehaviour main){}
        public void BUpdate(MonoBehaviour main)
        {
            if (IsNeedLerp)
            {
                CurrentGB.transform.rotation = Quaternion.Lerp(CurrentGB.transform.rotation, TargetRotation, Time.deltaTime * LerpValue);
                CurrentGB.transform.position = Vector3.Lerp(CurrentGB.transform.position, TargetPosition, Time.deltaTime * LerpValue*10f);
                CurrentGB.transform.localScale = Vector3.Lerp(CurrentGB.transform.localScale,TargetScale,Time.deltaTime*LerpValue*5f);
                if (CurrentGB.transform.rotation == TargetRotation && CurrentGB.transform.position == TargetPosition && CurrentGB.transform.localScale == TargetScale)
                    IsNeedLerp = false;
            }
        }
    }
}

