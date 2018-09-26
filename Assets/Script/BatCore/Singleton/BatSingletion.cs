using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
namespace THEDARKKNIGHT.BatCore
{
    public abstract class BatSingletion<T> where T : BatSingletion<T>
    {
        protected static T instance = null;

        protected BatSingletion(){}

        public static T Instance()
        {
            if (instance == null)
            {
                ConstructorInfo[] ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                ConstructorInfo ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
                if (ctor == null)
                    throw new Exception("Non-public ctor() not found!");
                instance = ctor.Invoke(null) as T;
            }

            return instance;
        }
    }

    public abstract class BatMonoSingletion<T> : MonoBehaviour where T : BatMonoSingletion<T> {

        protected static T instance = null;

        public static T Instance()
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if (FindObjectsOfType<T>().Length > 1)
                {
                    Debug.Log ("当前场景中已经有"+ typeof(T).Name);
                    return instance;
                }

                if (instance == null)
                {
                    string instanceName = typeof(T).Name;
                    GameObject instanceGO = GameObject.Find(instanceName);

                    if (instanceGO == null)
                        instanceGO = new GameObject(instanceName);
                    instance = instanceGO.AddComponent<T>();
                    DontDestroyOnLoad(instanceGO);
                }
                else
                {
                    Debug.Log("当前脚本" + typeof(T).Name+"单例已经存在");
                }
            }

            return instance;
        }


        public virtual void OnDestroy()
        {
            instance = null;
        }
    }
}
