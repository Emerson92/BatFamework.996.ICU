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

    private GameObject CurrenGB = null;

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
        ModelCtrl.Instance();
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
            ModelCtrl.Instance().RotationWithScreenPixl(packet.DeltaValue[0]);
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
        ModelCtrl.Instance().LerpMoveWithScreenPixl(packet.DeltaValue[0], packet.camera);
        return null;
    }

    private object SingleTouchEvent(object data)
    {
        Debug.Log("SingleTouchEvent");
        InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;
        if (packet.Info.CastGameObject != null)
        {
            BLog.Instance().Log(packet.Info.CastGameObject.name);
            ModelCtrl.Instance().SetOperateModel(packet.Info.CastGameObject);
            CurrenGB = packet.Info.CastGameObject;
        }
        return null;
    }

    private object ZoomCallback(object data)
    {
        InputDataPacket<float> packet = (InputDataPacket<float>)data;
        BLog.Instance().Log("ZoomCallback"+ packet.Value[0]);
        ModelCtrl.Instance().LerpScale(packet.Value[0]);
        //ModelCtrl.Instance().Scale(packet.Value[0]);
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
        ModelCtrl.Instance().RotationWithScreenPixl(packet.DeltaValue[0]);
        //ModelCtrl.Instance().LerpRotationWithScreenPixl(packet.DeltaValue[0]);
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
        //ModelCtrl.Instance().MoveWithScreenPixl(packet.DeltaValue[0],packet.camera);
        ModelCtrl.Instance().LerpMoveWithScreenPixl(packet.DeltaValue[0], packet.camera);
        BLog.Instance().Log("LeftDragCallback");
        return null;
    }

    private object LeftPressCallback(object data)
    {
        InputDataPacket<Vector3> packet = (InputDataPacket<Vector3>)data;
        if (packet.Info.CastGameObject != null && packet.Info.CastGameObject != CurrenGB) {
            BLog.Instance().Log(packet.Info.CastGameObject.name);
            ModelCtrl.Instance().SetOperateModel(packet.Info.CastGameObject);
            CurrenGB = packet.Info.CastGameObject;
        }
        BLog.Instance().Log("LeftPressCallback");
        return null;
    }

    // Update is called once per frame
    void Update () {

    }
}
