﻿using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.InputOperate.DataStruct;
using THEDARKKNIGHT.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.InputOperate {

    public class BGestureInputParser : IInputParser
    {
        public bool IsInputOperating()
        {
            if (Input.touchCount > 0)
                return true;
            else
                return false;
        }

        public InputDataPacket<Vector3> GetLeftDragEvent(Vector3 MousePos, Vector3 MousePfore, RaycastHit hit)
        {
            InputDataPacket<Vector3> packet = new InputDataPacket<Vector3>();
            return packet;
        }

        public InputDataPacket<Vector3> GetLeftPressEvent(Vector3 MousePos, RaycastHit hit)
        {
            InputDataPacket<Vector3> packet = new InputDataPacket<Vector3>();
            return packet;
        }

        public InputDataPacket<Vector3> GetLeftReleasEvent(Vector3 MousePos, Vector3 MousePfore, RaycastHit hit)
        {
            InputDataPacket<Vector3> packet = new InputDataPacket<Vector3>();
            return packet;
        }

        public InputDataPacket<Vector3> GetMultiDragEvent(Vector3[] fingerPos, Vector3[] fingerbefore, RaycastHit hit)
        {
            InputDataPacket<Vector3> packet = new InputDataPacket<Vector3>();
            packet.Value = fingerPos;
            for (int i = 0; i < fingerPos.Length; i++)
            {
                fingerPos[i] = fingerPos[i] - fingerbefore[i];
            }
            packet.DeltaValue = fingerPos;
            packet.OperateType = InputOperateType.MultiReleas;
            packet.Info = hit.transform == null ? new RayInfo() : new RayInfo(hit.transform.gameObject, hit.point, hit.normal);
            packet.IsOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            return packet;
        }

        public InputDataPacket<Vector3> GetMultiReleasEvent(Vector3[] fingerPos, Vector3[] fingerbefore, RaycastHit hit)
        {
            InputDataPacket<Vector3> packet = new InputDataPacket<Vector3>();
            packet.Value = fingerPos;
            for (int i =0;i<fingerPos.Length; i++) {
                fingerPos[i] = fingerPos[i] - fingerbefore[i];
            }
            packet.DeltaValue = fingerPos;
            packet.OperateType = InputOperateType.MultiReleas;
            packet.Info = hit.transform == null ? new RayInfo() : new RayInfo(hit.transform.gameObject, hit.point, hit.normal);
            packet.IsOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            return packet;
        }

        public InputDataPacket<Vector3> GetMultiTouchsEvent(Vector3[] fingerPos, RaycastHit hit)
        {
            InputDataPacket<Vector3> packet = new InputDataPacket<Vector3>();
            packet.Value = fingerPos;
            packet.OperateType = InputOperateType.MultiTouchs;
            packet.Info = hit.transform == null ? new RayInfo() : new RayInfo(hit.transform.gameObject, hit.point, hit.normal);
            packet.IsOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            return packet;
        }

        public InputDataPacket<Vector3> GetRightDragEvent(Vector3 MousePos, Vector3 MousePfore, RaycastHit hit)
        {
            InputDataPacket<Vector3> packet = new InputDataPacket<Vector3>();
            return packet;
        }

        public InputDataPacket<Vector3> GetRightPressEvent(Vector3 MousePos, RaycastHit hit)
        {
            InputDataPacket<Vector3> packet = new InputDataPacket<Vector3>();
            return packet;
        }

        public InputDataPacket<Vector3> GetRightReleasEvent(Vector3 MousePos, Vector3 MousePfore, RaycastHit hit)
        {
            InputDataPacket<Vector3> packet = new InputDataPacket<Vector3>();
            return packet;
        }

        public InputDataPacket<Vector3> GetSingleDragEvent(Vector3 fingerPos, Vector3 fingerbefore, RaycastHit hit)
        {
            InputDataPacket<Vector3> packet = new InputDataPacket<Vector3>();
            packet.Value = new Vector3[] { fingerPos };
            packet.DeltaValue = new Vector3[] { fingerPos - fingerbefore };
            packet.OperateType = InputOperateType.SingleDrag;
            packet.Info = hit.transform == null ? new RayInfo() : new RayInfo(hit.transform.gameObject, hit.point, hit.normal);
            packet.IsOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            return packet;
        }

        public InputDataPacket<Vector3> GetSingleReleasEvent(Vector3 fingerPos, Vector3 fingerbefore, RaycastHit hit)
        {
            InputDataPacket<Vector3> packet = new InputDataPacket<Vector3>();
            packet.Value = new Vector3[] { fingerPos };
            packet.DeltaValue = new Vector3[] { fingerPos - fingerbefore };
            packet.OperateType = InputOperateType.SingleReleas;
            packet.Info = hit.transform == null ? new RayInfo() : new RayInfo(hit.transform.gameObject, hit.point, hit.normal);
            packet.IsOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            return packet;
        }

        public InputDataPacket<Vector3> GetSingleTouchEvent(Vector3 fingerPos, RaycastHit hit)
        {
            InputDataPacket<Vector3> packet = new InputDataPacket<Vector3>();
            packet.Value = new Vector3[] { fingerPos };
            packet.OperateType = InputOperateType.SingleTouch;
            packet.Info = hit.transform == null ? new RayInfo() : new RayInfo(hit.transform.gameObject, hit.point, hit.normal);
            packet.IsOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            return packet;
        }

        public InputDataPacket<float> GetZoomEvent(float value, RaycastHit hit)
        {
            InputDataPacket<float> packet = new InputDataPacket<float>();
            packet.Value = new float[] { value };
            packet.DeltaValue = new float[] { float.NegativeInfinity };
            packet.OperateType = InputOperateType.Zoom;
            packet.Info = hit.transform == null ? new RayInfo() : new RayInfo(hit.transform.gameObject, hit.point, hit.normal);
            packet.IsOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            return packet;
        }
    }
}
