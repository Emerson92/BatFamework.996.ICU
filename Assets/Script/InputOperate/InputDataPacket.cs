using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.InputOperate.DataStruct {
    public enum InputOperateType
    {
        LeftPress,
        RightPress,
        LeftDrag,
        RightDrag,
        RightReleas,
        LeftReleas,
        Zoom,
        SingleTouch,
        MultiTouchs,
        SingleDrag,
        MultiDrag,
        SingleReleas,
        MultiReleas,
    }

    public struct InputDataPacket<T>
    {

        public InputOperateType OperateType;

        public T Value;

        public T DeltaValue;

        public bool IsOverUI;

        public RayInfo Info;

        public InputDataPacket(InputOperateType type, T value, T deltaValue, bool isOverUI, RayInfo info)
        {
            this.OperateType = type;
            this.Value = value;
            this.DeltaValue = deltaValue;
            this.IsOverUI = isOverUI;
            this.Info = info;
        }
    }

    public struct RayInfo
    {

        public GameObject CastGameObject;

        public Vector3 RayPoistion;

        public Vector3 normal;

        public RayInfo(GameObject ob, Vector3 position, Vector3 normal)
        {
            this.CastGameObject = ob;
            this.RayPoistion = position;
            this.normal = normal;
        }
    }
}
