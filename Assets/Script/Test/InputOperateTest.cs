using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using THEDARKKNIGHT.EventSystem;
using THEDARKKNIGHT.EventDefine;
using System;
using THEDARKKNIGHT.Log;
using THEDARKKNIGHT.InputOperate;
using THEDARKKNIGHT.InputOperate.DataStruct;

public class InputOperateTest : MonoBehaviour {

    GameObject Operater = null;


	// Use this for initialization
	void Start () {
        BInputOperateEngine.Instance();
        BLog.Instance().Log("InputOperateTest");
        BEventManager.Instance().AddListener(BatEventDefine.LEFTPRESSEVENT,LeftPressCallback);
        BEventManager.Instance().AddListener(BatEventDefine.LEFTDRAGEVENT, LeftDragCallback);
        BEventManager.Instance().AddListener(BatEventDefine.LEFTRELEASEVENT, LeftReleaseCallback);
        BEventManager.Instance().AddListener(BatEventDefine.RIGHTPRESSEVENT,RightPressCallback);
        BEventManager.Instance().AddListener(BatEventDefine.RIGHTDRAGEVENT, RightDragCallback);
        BEventManager.Instance().AddListener(BatEventDefine.RIGHTPRESSEVENT, RightReleaseCallback);
        BEventManager.Instance().AddListener(BatEventDefine.ZOOMEVENT, ZoomCallback);
        BEventManager.Instance().AddListener(BatEventDefine.SINGLETOUCHEVENT, SingleTouchEvent);
        BEventManager.Instance().AddListener(BatEventDefine.SINGLEDRAGEVENT,SingleDragEvent);
        BEventManager.Instance().AddListener(BatEventDefine.SINGLERELEASEVENT, SingleReleasEvent);
        BEventManager.Instance().AddListener(BatEventDefine.MULTITOUCHEVENT,MultiTouchEvent);
        BEventManager.Instance().AddListener(BatEventDefine.MULTIDRAGEVENT, MultiDragEvent);
        BEventManager.Instance().AddListener(BatEventDefine.MULTIRELEASEVENT, MultiReleasEvent);
    }

    private object MultiReleasEvent(object data)
    {
        Debug.Log("MultiReleasEvent");
        return null;
    }

    private object MultiDragEvent(object data)
    {
        Debug.Log("MultiDragEvent");
        InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;
        if (packet.Value.Length == 2 ) {
            BLog.Instance().Log("MultiDragEvent" + packet.DeltaValue[0].magnitude);
            if (Operater != null && packet.DeltaValue[0].magnitude > 1)
            {
                float angleX = 0;
                float angleY = 0;
                float ratioY = packet.DeltaValue[1].x / Screen.width;
                angleY = ratioY * 180f;
                angleY %= 360;
                float ratioX = packet.DeltaValue[1].y / Screen.height;
                angleX = ratioX * 180f;
                angleX %= 360;
                Quaternion preRotation = Operater.transform.rotation;
                //世界x轴投影到本地坐标，世界坐标x轴 换算 到本地旋转轴
                //Matrix4x4 localPointMartrix = new Matrix4x4();
                //localPointMartrix.SetTRS(Vector3.zero, preRotation, Vector3.one);
                Vector3 localVecX = new Vector3();
                //localVecX = localPointMartrix.inverse.MultiplyPoint3x4(new Vector3(1, 0, 0)); 
                localVecX = Operater.transform.InverseTransformDirection(new Vector3(1, 0, 0));
                Quaternion final = preRotation;
                Quaternion qua = Quaternion.AngleAxis(angleX, localVecX.normalized);
                preRotation *= qua;
                Quaternion qua1 = Quaternion.AngleAxis(-angleY, Vector3.up);
                preRotation *= qua1;
                Operater.transform.rotation = preRotation;
            }
        }
        return null;
    }

    private object MultiTouchEvent(object data)
    {
        Debug.Log("MultiTouchEvent");
        return null;
    }

    private object SingleReleasEvent(object data)
    {
        Debug.Log("SingleReleasEvent");
        return null;
    }

    private object SingleDragEvent(object data)
    {
        Debug.Log("SingleDragEvent");
        InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;
        if (packet.Info.CastGameObject != null)
        {
            Vector3 obScreenPos = BCameraRaycast.Instance().GetCurrentCamera().WorldToScreenPoint(packet.Info.CastGameObject.transform.position);
            Vector3 finalPos = obScreenPos + packet.DeltaValue[0];
            packet.Info.CastGameObject.transform.position = BCameraRaycast.Instance().GetCurrentCamera().ScreenToWorldPoint(finalPos);
        }
        return null;
    }

    private object SingleTouchEvent(object data)
    {
        Debug.Log("SingleTouchEvent");
        InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;
        if (packet.Info.CastGameObject != null)
        {
            BLog.Instance().Log(packet.Info.CastGameObject.name);
            Operater = packet.Info.CastGameObject;
        }
        return null;
    }

    private object ZoomCallback(object data)
    {
        InputDataPacket<float> packet = (InputDataPacket<float>)data;
        BLog.Instance().Log("ZoomCallback"+ packet.Value[0]);
        if (packet.Info.CastGameObject != null)
        {
            packet.Info.CastGameObject.transform.localScale = packet.Info.CastGameObject.transform.localScale+packet.Info.CastGameObject.transform.localScale*packet.Value[0]*(1/5f);
        }
        return null;
    }

    private object RightReleaseCallback(object data)
    {
        BLog.Instance().Log("RightReleaseCallback");
        return null;
    }

    private object RightDragCallback(object data)
    {
        InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;
        BLog.Instance().Log("RightDragCallback" + packet.DeltaValue[0].magnitude);
        if (Operater != null && packet.DeltaValue[0].magnitude>1)
        {
            float angleX = 0;
            float angleY = 0;
            float ratioY = packet.DeltaValue[0].x / Screen.width;
            angleY = ratioY * 180f;
            angleY %= 360;
            float ratioX = packet.DeltaValue[0].y / Screen.height;
            angleX = ratioX * 180f;
            angleX %= 360;
            Quaternion preRotation = Operater.transform.rotation;
            //世界x轴投影到本地坐标，世界坐标x轴 换算 到本地旋转轴
            //Matrix4x4 localPointMartrix = new Matrix4x4();
            //localPointMartrix.SetTRS(Vector3.zero, preRotation, Vector3.one);
            Vector3 localVecX = new Vector3();
            //localVecX = localPointMartrix.inverse.MultiplyPoint3x4(new Vector3(1, 0, 0)); 
            localVecX = Operater.transform.InverseTransformDirection(new Vector3(1, 0, 0));
            Quaternion final = preRotation;
            Quaternion qua = Quaternion.AngleAxis(angleX, localVecX.normalized);
            preRotation *= qua;
            Quaternion qua1 = Quaternion.AngleAxis(-angleY, Vector3.up);
            preRotation *= qua1;
            Operater.transform.rotation = preRotation;
        }

        BLog.Instance().Log("RightDragCallback");
        return null;
    }

    private object RightPressCallback(object data)
    {
        BLog.Instance().Log("RightPressCallback");
        return null;
    }

    private object LeftReleaseCallback(object data)
    {
        BLog.Instance().Log("LeftReleaseCallback");
        return null;
    }

    private object LeftDragCallback(object data)
    {
        InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;
        if (packet.Info.CastGameObject != null) {
            Vector3 obScreenPos = BCameraRaycast.Instance().GetCurrentCamera().WorldToScreenPoint(packet.Info.CastGameObject.transform.position);
            Vector3 finalPos = obScreenPos + packet.DeltaValue[0];
            packet.Info.CastGameObject.transform.position = BCameraRaycast.Instance().GetCurrentCamera().ScreenToWorldPoint(finalPos);
        }
        BLog.Instance().Log("LeftDragCallback");
        return null;
    }

    private object LeftPressCallback(object data)
    {
        InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;
        if (packet.Info.CastGameObject != null) {
            BLog.Instance().Log(packet.Info.CastGameObject.name);
            Operater = packet.Info.CastGameObject;
        }
           
        BLog.Instance().Log("LeftPressCallback");
        return null;
    }

    // Update is called once per frame
    void Update () {

    }
}
