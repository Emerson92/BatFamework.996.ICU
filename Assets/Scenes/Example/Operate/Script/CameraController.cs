using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float distance = 5f;

    private float speedX = 240;
    private float speedY = 120;

    private float minLimitY = -360f;
    private float maxLimitY = 360f;

    private float mX = 0.0f;
    private float mY = 0.0f;

    private float maxDistance = 10;
    private float minDistance = 1.5f;

    private float zoomSpeed = 2f;
    public bool isNeedDamping = true;

    public float damping = 2.5f;

    void Start()
    {
        mX = transform.eulerAngles.x;
        mY = transform.eulerAngles.y;
    }

    void LateUpdate()
    {
        if (target != null && Input.GetMouseButton(1))
        {
            mX += Input.GetAxis("Mouse X") * speedX * Time.deltaTime;
            mY -= Input.GetAxis("Mouse Y") * speedY * Time.deltaTime;

            mY = ClampAngle(mY, minLimitY, maxLimitY);
        }

        distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
       
        Quaternion mRotation = Quaternion.Euler(mY, mX, 0);
        Vector3 mPosition = mRotation * new Vector3(0, 0, -distance) + target.position;
        Debug.DrawLine(transform.position, mPosition, Color.yellow, 2f);
        //是否平滑过渡
        if (isNeedDamping)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, mRotation, Time.deltaTime * damping);
            transform.position = Vector3.Lerp(transform.position, mPosition, Time.deltaTime * damping);
        }
        else
        {
            transform.rotation = mRotation;
            transform.position = mPosition;
        }
    }

    //private void Update()
    //{
    //    if (target != null && Input.GetMouseButton(1))
    //    {
    //        mX += Input.GetAxis("Mouse X") * speedX * Time.deltaTime;
    //        mY -= Input.GetAxis("Mouse Y") * speedY * Time.deltaTime;

    //        mY = ClampAngle(mY, minLimitY, maxLimitY);
    //    }

    //    distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
    //    distance = Mathf.Clamp(distance, minDistance, maxDistance);

    //    Quaternion mRotation = Quaternion.Euler(mY, mX, 0);
    //    Vector3 mPosition = mRotation * new Vector3(0, 0, -distance) + target.position;
    //    //是否平滑过渡
    //    if (isNeedDamping)
    //    {
    //        transform.rotation = Quaternion.Lerp(transform.rotation, mRotation, Time.deltaTime * damping);
    //        transform.position = Vector3.Lerp(transform.position, mPosition, Time.deltaTime * damping);
    //    }
    //    else
    //    {
    //        transform.rotation = mRotation;
    //        transform.position = mPosition;
    //    }
    //}

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}