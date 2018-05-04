using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT;
using UnityEngine;

public class Hello : ILifeCycle {
    public void BAwake(MonoBehaviour main)
    {
        Debug.Log("Hello BAwake");
    }

    public void BDisable(MonoBehaviour main)
    {
    }

    public void BFixedUpdate(MonoBehaviour main)
    {
    }

    public void BLateUpdate(MonoBehaviour main)
    {
    }

    public void BOnApplicationFocus(MonoBehaviour main)
    {
    }

    public void BOnApplicationPause(MonoBehaviour main)
    {
    }

    public void BOnApplicationQuit(MonoBehaviour main)
    {
    }

    public void BOnDestory(MonoBehaviour main)
    {
    }

    public void BOnDestroy(MonoBehaviour main)
    {
    }

    public void BOnEnable(MonoBehaviour main)
    {
    }

    public void BStart(MonoBehaviour main)
    {
        Debug.Log("Hello BStart");
    }

    public void BUpdate(MonoBehaviour main)
    {
        Debug.Log("Hello BUpdate");
        if (Input.GetKeyDown(KeyCode.P))
        {
            this.Disable();
        }
        if(Input.GetKeyDown(KeyCode.O)) {
            
        }
    }

    public void OpenAble() {
        this.Enable();
    }

    public Hello() {
        this.Enable();
    }
}
