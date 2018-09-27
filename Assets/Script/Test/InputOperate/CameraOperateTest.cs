using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.EventDefine;
using THEDARKKNIGHT.EventSystem;
using THEDARKKNIGHT.InputOperate;
using THEDARKKNIGHT.InputOperate.DataStruct;
using THEDARKKNIGHT.Log;
using UnityEngine;

public class CameraOperateTest : MonoBehaviour {

    private GameObject CurrentGB;


	// Use this for initialization
	void Start () {
        BInputOperateEngine.Instance();
        BEventManager.Instance().AddListener(BatEventDefine.LEFTPRESSEVENT, LeftPressCallback);
        BEventManager.Instance().AddListener(BatEventDefine.LEFTDRAGEVENT, LeftDragCallback);
        BEventManager.Instance().AddListener(BatEventDefine.LEFTRELEASEVENT, LeftReleaseCallback);
        BEventManager.Instance().AddListener(BatEventDefine.RIGHTPRESSEVENT, RightPressCallback);
        BEventManager.Instance().AddListener(BatEventDefine.RIGHTDRAGEVENT, RightDragCallback);
        BEventManager.Instance().AddListener(BatEventDefine.RIGHTPRESSEVENT, RightReleaseCallback);
        BEventManager.Instance().AddListener(BatEventDefine.ZOOMEVENT, ZoomCallback);
        BEventManager.Instance().AddListener(BatEventDefine.SINGLETOUCHEVENT, SingleTouchEvent);
        BEventManager.Instance().AddListener(BatEventDefine.SINGLEDRAGEVENT, SingleDragEvent);
        BEventManager.Instance().AddListener(BatEventDefine.SINGLERELEASEVENT, SingleReleasEvent);
        BEventManager.Instance().AddListener(BatEventDefine.MULTITOUCHEVENT, MultiTouchEvent);
        BEventManager.Instance().AddListener(BatEventDefine.MULTIDRAGEVENT, MultiDragEvent);
        BEventManager.Instance().AddListener(BatEventDefine.MULTIRELEASEVENT, MultiReleasEvent);
        CameraCtrl.Instance().SetCamera(Camera.main);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private object MultiReleasEvent(object data)
    {
        Debug.Log("MultiReleasEvent");
        return null;
    }

    private object MultiDragEvent(object data)
    {
        Debug.Log("MultiDragEvent");

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
        return null;
    }

    private object SingleTouchEvent(object data)
    {
        Debug.Log("SingleTouchEvent");
        return null;
    }

    private object ZoomCallback(object data)
    {
        BLog.Instance().Log("ZoomCallback");
        InputDataPacket<float> packet = (InputDataPacket<float>)data;
        //CameraCtrl.Instance().Scale(packet.Value[0]);
        CameraCtrl.Instance().LerpScale(packet.Value[0]);
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
        //BLog.Instance().Log("RightDragCallback"+ packet.DeltaValue[0]);
        //CameraCtrl.Instance().RotateWithScreenPixl(packet.DeltaValue[0]);
        CameraCtrl.Instance().LerpRotateWithScreenPixl(packet.DeltaValue[0]);
        return null;
    }

    private object RightPressCallback(object data)
    {
        InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;
        BLog.Instance().Log("RightPressCallback");
        return null;
    }

    private object LeftReleaseCallback(object data)
    {
        return null;
    }

    private object LeftDragCallback(object data)
    {
        InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;
        //CameraCtrl.Instance().MoveWithScreenPixl(packet.DeltaValue[0]);
        CameraCtrl.Instance().LerpMoveWithScreenPixl(packet.DeltaValue[0]);
        BLog.Instance().Log("LeftDragCallback :");
        return null;
    }

    private object LeftPressCallback(object data)
    {
        InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;
        BLog.Instance().Log("LeftReleaseCallback");
        if (packet.Info.CastGameObject != null && packet.Info.CastGameObject != CurrentGB)
        {
            CameraCtrl.Instance().LerpFocusCenter(packet.Info.CastGameObject.transform.position);
            //CameraCtrl.Instance().FocusCenter(packet.Info.CastGameObject.transform.position);
            CurrentGB = packet.Info.CastGameObject;
        }
        return null;
    }
}
