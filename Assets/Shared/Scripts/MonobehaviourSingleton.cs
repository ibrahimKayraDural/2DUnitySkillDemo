using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonobehaviourSingleton<T> : MonoBehaviour where T : Component
{
    public static T Instance;

    internal virtual void Awake()
    {
        if(Instance == null)
        {
            Instance = this as T;
        }
        else if (Instance != this as T)
        {
            Destroy(this);
        }
    }
}
