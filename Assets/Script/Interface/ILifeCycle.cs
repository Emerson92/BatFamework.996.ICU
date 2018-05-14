using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILifeCycle {

    void BAwake(MonoBehaviour main);

    void BStart(MonoBehaviour main);

    void BOnEnable(MonoBehaviour main);

    void BDisable(MonoBehaviour main);

    void BOnDestory(MonoBehaviour main);

    void BFixedUpdate(MonoBehaviour main);

    void BLateUpdate(MonoBehaviour main);

    void BOnApplicationFocus(MonoBehaviour main);

    void BOnApplicationPause(MonoBehaviour main);

    void BOnApplicationQuit(MonoBehaviour main);

    void BOnDestroy(MonoBehaviour main);

    void BUpdate(MonoBehaviour main);
}
