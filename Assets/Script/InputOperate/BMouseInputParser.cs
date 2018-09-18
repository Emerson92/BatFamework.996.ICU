using System;
using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.InputOperate.DataStruct;
using THEDARKKNIGHT.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.InputOperate {
    public class BMouseInputParser : IInputParser
    {
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
            return packet;
        }

        public InputDataPacket<Vector3> GetMultiReleasEvent(Vector3[] fingerPos, Vector3[] fingerbefore, RaycastHit hit)
        {
            InputDataPacket<Vector3> packet = new InputDataPacket<Vector3>();
            return packet;
        }

        public InputDataPacket<Vector3> GetMultiTouchsEvent(Vector3[] fingerPos, RaycastHit hit)
        {
            InputDataPacket<Vector3> packet = new InputDataPacket<Vector3>();
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
            return packet;
        }

        public InputDataPacket<Vector3> GetSingleReleasEvent(Vector3 fingerPos, Vector3 fingerbefore, RaycastHit hit)
        {
            InputDataPacket<Vector3> packet = new InputDataPacket<Vector3>();
            return packet;
        }

        public InputDataPacket<Vector3> GetSingleTouchEvent(Vector3 fingerPos, RaycastHit hit)
        {
            InputDataPacket<Vector3> packet = new InputDataPacket<Vector3>();
            return packet;
        }

        public InputDataPacket<float> GetZoomEvent(float value, RaycastHit hit)
        {
            InputDataPacket<float> packet = new InputDataPacket<float>();
            return packet;
        }

        public bool IsInputOperating()
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
                return true;
            else
                return false;
        }
    }

}

