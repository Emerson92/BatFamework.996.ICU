using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using THEDARKKNIGHT.EventDefine;
using THEDARKKNIGHT.EventSystem;
using THEDARKKNIGHT.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.InputOperate
{
    public class BInputOperateEngine : BatSingletion<BInputOperateEngine>, ILifeCycle
    {
#if UNITY_STANDALONE_WIN||UNITY_EDITOR
        private IInputParser InputParser = new BMouseInputParser();
#elif UNITY_IOS || UNITY_ANDROID
          private IInputParser InputParser = new BGestureInputParser();
#endif

        private readonly float DiagonalLenght = Mathf.Sqrt(Mathf.Pow(Screen.width, 2) + Mathf.Pow(Screen.height, 2));

        /// <summary>
        /// 临时变量存储区域
        /// </summary>
        Vector3[] TempInputValue = new Vector3[5] { Vector3.negativeInfinity, Vector3.negativeInfinity, Vector3.negativeInfinity, Vector3.negativeInfinity, Vector3.negativeInfinity };

        private BInputOperateEngine()
        {
            this.Enable();
        }

        public void BAwake(MonoBehaviour main)
        {
            LifeCycleTool tool = this.GetLifeCycleTool();
            tool.SetLifeCycle(LifeCycleTool.LifeType.Start, true)
                .SetLifeCycle(LifeCycleTool.LifeType.Update, true);
        }

        public void BDisable(MonoBehaviour main) { }
        public void BFixedUpdate(MonoBehaviour main) { }
        public void BLateUpdate(MonoBehaviour main) { }
        public void BOnApplicationFocus(MonoBehaviour main) { }
        public void BOnApplicationPause(MonoBehaviour main) { }
        public void BOnApplicationQuit(MonoBehaviour main) { }
        public void BOnDestory(MonoBehaviour main) { }
        public void BOnDestroy(MonoBehaviour main) { }
        public void BOnEnable(MonoBehaviour main) { }
        public void BStart(MonoBehaviour main) { }
        public void BUpdate(MonoBehaviour main)
        {
            OperatorCheck();
        }

        private void OperatorCheck()
        {
            if (InputParser.IsInputOperating())
            {
                RaycastHit hit = BCameraRaycast.Instance().ShootRaycast(Input.mousePosition);
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
                MouseOperate(hit);
#elif UNITY_IOS || UNITY_ANDROID
                FingerOperate(hit);
#endif
            }
        }

        private void FingerOperate(RaycastHit hit)
        {
            int touchCount = Input.touchCount;
            if (touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    DataStruct.InputDataPacket<Vector3> data = InputParser.GetSingleTouchEvent(Input.GetTouch(0).position, hit);
                    BEventManager.Instance().DispatchEvent(BatEventDefine.SINGLETOUCHEVENT, data);
                    SaveInputValue(touchCount);
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    DataStruct.InputDataPacket<Vector3> data = InputParser.GetSingleDragEvent(Input.GetTouch(0).position, TempInputValue[0], hit);
                    BEventManager.Instance().DispatchEvent(BatEventDefine.SINGLEDRAGEVENT, data);
                    SaveInputValue(touchCount);
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    DataStruct.InputDataPacket<Vector3> data = InputParser.GetSingleDragEvent(Input.GetTouch(0).position, TempInputValue[0], hit);
                    BEventManager.Instance().DispatchEvent(BatEventDefine.SINGLERELEASEVENT, data);
                    SaveInputValue(touchCount);
                }
            }
            else if (touchCount > 1)
            {
                if (Input.GetTouch(touchCount - 1).phase == TouchPhase.Began && touchCount < 5)// MultiFinger Begin Event
                {
                    SaveInputValue(touchCount);
                    DataStruct.InputDataPacket<Vector3> data = InputParser.GetMultiTouchsEvent(TempInputValue, hit);
                    BEventManager.Instance().DispatchEvent(BatEventDefine.MULTITOUCHEVENT, data);
                }
                else if (Input.GetTouch(touchCount - 1).phase == TouchPhase.Moved && touchCount < 5)
                {
                    Vector3 deltaVectorOne = new Vector3(Input.GetTouch(0).position.x - TempInputValue[0].x, Input.GetTouch(0).position.y - TempInputValue[0].y, 0);
                    Vector3 deltaVectorTwo = new Vector3(Input.GetTouch(1).position.x - TempInputValue[1].x, Input.GetTouch(1).position.y - TempInputValue[1].y, 0);
                    if (CheckIsZoom(deltaVectorOne, deltaVectorTwo))//Zoom function callback;
                    {
                        float distance = deltaVectorOne.magnitude < deltaVectorTwo.magnitude ? deltaVectorOne.magnitude : deltaVectorTwo.magnitude;
                        DataStruct.InputDataPacket<float> zoomData = InputParser.GetZoomEvent(distance/ DiagonalLenght, hit);
                        BEventManager.Instance().DispatchEvent(BatEventDefine.ZOOMEVENT, zoomData);
                    }
                    //MultiFinger Drag Event;
                    Vector3[] pos = GetLastPosValue(touchCount);
                    DataStruct.InputDataPacket<Vector3> data = InputParser.GetMultiDragEvent(pos, TempInputValue, hit);
                    BEventManager.Instance().DispatchEvent(BatEventDefine.MULTIDRAGEVENT, data);
                    
                    SaveInputValue(touchCount);
                }
                else if (Input.GetTouch(touchCount - 1).phase == TouchPhase.Ended && touchCount < 5)//MultiFinger Releas Event
                {
                    Vector3[] pos = GetLastPosValue(touchCount);
                    DataStruct.InputDataPacket<Vector3> data = InputParser.GetMultiReleasEvent(pos, TempInputValue, hit);
                    BEventManager.Instance().DispatchEvent(BatEventDefine.MULTIRELEASEVENT, data);
                    SaveInputValue(touchCount);
                }
            }
        }

        private static Vector3[] GetLastPosValue(int touchCount)
        {
            Vector3[] pos = new Vector3[touchCount];
            for (int i = 0; i < touchCount; i++)
            {
                pos[i] = Input.GetTouch(i).position;
            }
            return pos;
        }

        private void MouseOperate(RaycastHit hit)
        {
            if (Input.GetMouseButtonDown(0))
            {
                TempInputValue[0] = Input.mousePosition;
                DataStruct.InputDataPacket<Vector3> data = InputParser.GetLeftPressEvent(Input.mousePosition,hit);
                BEventManager.Instance().DispatchEvent(BatEventDefine.LEFTPRESSEVENT, data);
            }
            if (Input.GetMouseButton(0))
            {
                DataStruct.InputDataPacket<Vector3> data = InputParser.GetLeftDragEvent(Input.mousePosition, TempInputValue[0], hit);
                BEventManager.Instance().DispatchEvent(BatEventDefine.LEFTDRAGEVENT, data);
            }
            if (Input.GetMouseButtonUp(0))
            {
                DataStruct.InputDataPacket<Vector3> data = InputParser.GetLeftReleasEvent(Input.mousePosition, TempInputValue[0], hit);
                BEventManager.Instance().DispatchEvent(BatEventDefine.LEFTRELEASEVENT, data);
            }
            if (Input.GetMouseButtonDown(1))
            {
                TempInputValue[0] = Input.mousePosition;
                DataStruct.InputDataPacket<Vector3> data = InputParser.GetRightPressEvent(Input.mousePosition, hit);
                BEventManager.Instance().DispatchEvent(BatEventDefine.RIGHTPRESSEVENT, data);
            }
            if (Input.GetMouseButton(1))
            {
                DataStruct.InputDataPacket<Vector3> data = InputParser.GetRightDragEvent(Input.mousePosition, TempInputValue[0], hit);
                BEventManager.Instance().DispatchEvent(BatEventDefine.RIGHTDRAGEVENT, data);
            }
            if (Input.GetMouseButtonUp(1))
            {
                DataStruct.InputDataPacket<Vector3> data = InputParser.GetRightReleasEvent(Input.mousePosition, TempInputValue[0], hit);
                BEventManager.Instance().DispatchEvent(BatEventDefine.RIGHTRELEASEVENT, data);
            }
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                DataStruct.InputDataPacket<float> data = InputParser.GetZoomEvent(Input.GetAxis("Mouse ScrollWheel"), hit);
                BEventManager.Instance().DispatchEvent(BatEventDefine.ZOOMEVENT, data);
            }
            TempInputValue[0] = Input.mousePosition;
        }

        private bool CheckIsZoom(Vector3 vectorOne, Vector3 vectorTwo)
        {
            if (Vector3.Dot(vectorOne, vectorTwo) < 0)
                return true;
            else
                return false;
        }

        private void SaveInputValue(int touchCount)
        {
            for (int i = 0; i < touchCount; i++)
            {
                TempInputValue[i] = Input.GetTouch(i).position;
            }
        }
    }
}

