﻿using UnityEngine;

public abstract class SingletonPersistant<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {

                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    var obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    instance = obj.AddComponent<T>();
                }
            }

            return instance;

        }
        set
        {
            instance = value;
        }
    }



    public virtual void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

