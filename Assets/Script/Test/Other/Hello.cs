using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT;
using THEDARKKNIGHT.Interface;
using UnityEngine;

public class Hello : ILifeCycle {

    private LifeCycleTool tool;

    public int proporty;

    public void SetProporty(int i) {
        this.proporty = i;
    }

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
        Debug.Log("Hello BUpdate "+proporty);
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    this.Disable(tool);
        //}
        //if(Input.GetKeyDown(KeyCode.O)) {
            
        //}
    }

    public void OpenAble() {
        //if (tool == null)
        //{
        //    tool = this.Enable(new LifeCycleTool()
        //    {
        //        priority = 0,
        //        Icycle = this
        //    }.SetLifeCycle(LifeCycleTool.LifeType.Update,true));
        //}
        //else {
        //    this.Enable(tool);
        //}

    }

    public Hello(int i) {
        //if (tool == null)
        //{
        //    tool = this.Enable(new LifeCycleTool()
        //    {
        //        priority = i,
        //        Icycle = this
        //    }.SetLifeCycle(LifeCycleTool.LifeType.Update, true));
        //    SetProporty(i);
        //}
        //else {
        //    this.Enable(tool);
        //}

    }
}
