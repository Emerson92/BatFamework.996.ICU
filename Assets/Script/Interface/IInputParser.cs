using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.InputOperate.DataStruct;
using UnityEngine;
namespace THEDARKKNIGHT.Interface
{
    public interface IInputParser
    {

        /// <summary>
        /// 鼠标左键事件
        /// </summary>
        InputDataPacket<Vector3> GetLeftPressEvent();

        /// <summary>
        /// 鼠标右键事件
        /// </summary>
        InputDataPacket<Vector3> GetRightPressEvent();

        /// <summary>
        /// 鼠标左键拖拽事件
        /// </summary>
        InputDataPacket<Vector3> GetLeftDragEvent();

        /// <summary>
        /// 鼠标右键拖拽事件
        /// </summary>
        InputDataPacket<Vector3> GetRightDragEvent();

        /// <summary>
        /// 鼠标右键释放事件
        /// </summary>
        InputDataPacket<Vector3> GetRightReleasEvent();

        /// <summary>
        /// 鼠标左键释放事件
        /// </summary>
        InputDataPacket<Vector3> GetLeftReleasEvent();


        /// <summary>
        /// 缩放事件
        /// </summary>
        InputDataPacket<float> GetZoomEvent();

        /// <summary>
        /// 单手指触碰事件
        /// </summary>
        InputDataPacket<Vector3> GetSingleTouchEvent();

        /// <summary>
        /// 多手指触碰事件
        /// </summary>
        InputDataPacket<Vector3> GetMultiTouchsEvent();

        /// <summary>
        /// 单手指拖拽事件
        /// </summary>
        InputDataPacket<Vector3> GetSingleDragEvent();


        /// <summary>
        /// 多手指拖拽事件
        /// </summary>
        InputDataPacket<Vector3> GetMultiDragEvent();

        /// <summary>
        /// 单手指释放事件
        /// </summary>
        InputDataPacket<Vector3> GetSingleReleasEvent();


        /// <summary>
        /// 多手指释放事件
        /// </summary>
        InputDataPacket<Vector3> GetMultiReleasEvent();
    }

}

