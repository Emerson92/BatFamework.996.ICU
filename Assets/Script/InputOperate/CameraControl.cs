using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT;
using THEDARKKNIGHT.BatCore;
using UnityEngine;
/// <summary>
///  Camera control Script 
///  this is script main to control the Camera status on the scene
///  such as the distance and rotation
/// </summary>
public class CameraControl: BatSingletion<CameraControl>
{

    private float Radius = 5f;

    private Camera ObCamera;

    private Vector3 CameraLookDir;

    private Vector3 CenterPos;

    private CameraControl() { }

    public void SetCamera(Camera camera) {
        ObCamera = camera;
    }

    public Vector3 GetCenterPos() {
        return CenterPos;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="radius"></param>
    public void SetObserverRadius(float radius) {
        Radius = radius;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="center"></param>
    public void FocusCenter(Vector3 center) {
        CenterPos = center;
        Vector3 dir = -ObCamera.transform.forward * Radius;
        Vector3 targetPos = center + dir;
        ObCamera.transform.position = targetPos;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="detlaValue"></param>
    public void RotateWithScreenPixl(Vector3 detlaValue) {
        if (ObCamera != null) {
            // Debug.DrawLine(ObCamera.transform.position, ObCamera.transform.position + detlaValue,Color.blue,10f);
            //Debug.DrawLine(ObCamera.transform.position, CenterPos + detlaValue, Color.black, 10f);
            //Vector3 detla = ObCamera.transform.TransformDirection(detlaValue*0.05f);
            //Vector3 targetDir = (ObCamera.transform.forward * Radius + detla).normalized * Radius;
            //Debug.DrawLine(CenterPos, CenterPos - targetDir,Color.yellow,10f);
            //Vector3 targePos = CenterPos - targetDir;
            //ObCamera.transform.position = targePos;
            // Debug.DrawLine(targePos, CenterPos , Color.yellow, 10f);
            //ObCamera.transform.LookAt(CenterPos);
            Vector3 axis = Vector3.Cross(detlaValue, ObCamera.transform.forward);
            ObCamera.transform.RotateAround(CenterPos, axis, detlaValue.magnitude);
        }
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="deltaValue"></param>
    public void MoveWithScreenPixl(Vector3 screendeltaValue) {
        if (ObCamera != null) {
            Vector3 centerScreenPos = ObCamera.WorldToScreenPoint(CenterPos);
            Vector3 moveTargetPos = ObCamera.ScreenToWorldPoint(centerScreenPos - screendeltaValue);
            FocusCenter(moveTargetPos);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public void Scale(float value) {

    }
}
