using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.InputOperate.DataStruct;
using UnityEngine;
namespace THEDARKKNIGHT.Interface
{

    public interface IInputParser
    {
        bool IsInputOperating();

        /// <summary>
        /// 鼠标左键事件
        /// </summary>
        InputDataPacket<Vector3> GetLeftPressEvent(Vector3 MousePos, RaycastHit hit);

        /// <summary>
        /// 鼠标右键事件
        /// </summary>
        InputDataPacket<Vector3> GetRightPressEvent(Vector3 MousePos, RaycastHit hit);

        /// <summary>
        /// 鼠标左键拖拽事件
        /// </summary>
        InputDataPacket<Vector3> GetLeftDragEvent(Vector3 MousePos, Vector3 MousePfore, RaycastHit hit);

        /// <summary>
        /// 鼠标右键拖拽事件
        /// </summary>
        InputDataPacket<Vector3> GetRightDragEvent(Vector3 MousePos, Vector3 MousePfore, RaycastHit hit);

        /// <summary>
        /// 鼠标右键释放事件
        /// </summary>
        InputDataPacket<Vector3> GetRightReleasEvent(Vector3 MousePos, Vector3 MousePfore, RaycastHit hit);

        /// <summary>
        /// 鼠标左键释放事件
        /// </summary>
        InputDataPacket<Vector3> GetLeftReleasEvent(Vector3 MousePos, Vector3 MousePfore, RaycastHit hit);


        /// <summary>
        /// 缩放事件
        /// </summary>
        InputDataPacket<float> GetZoomEvent(float value, RaycastHit hit);

        /// <summary>
        /// 单手指触碰事件
        /// </summary>
        InputDataPacket<Vector3> GetSingleTouchEvent(Vector3 fingerPos, RaycastHit hit);

        /// <summary>
        /// 多手指触碰事件
        /// </summary>
        InputDataPacket<Vector3> GetMultiTouchsEvent(Vector3[] fingerPos, RaycastHit hit);

        /// <summary>
        /// 单手指拖拽事件
        /// </summary>
        InputDataPacket<Vector3> GetSingleDragEvent(Vector3 fingerPos, Vector3 fingerbefore, RaycastHit hit);


        /// <summary>
        /// 多手指拖拽事件
        /// </summary>
        InputDataPacket<Vector3> GetMultiDragEvent(Vector3[] fingerPos, Vector3[] fingerbefore, RaycastHit hit);

        /// <summary>
        /// 单手指释放事件
        /// </summary>
        InputDataPacket<Vector3> GetSingleReleasEvent(Vector3 fingerPos, Vector3 fingerbefore, RaycastHit hit);


        /// <summary>
        /// 多手指释放事件
        /// </summary>
        InputDataPacket<Vector3> GetMultiReleasEvent(Vector3[] fingerPos, Vector3[] fingerbefore, RaycastHit hit);
    }

}

