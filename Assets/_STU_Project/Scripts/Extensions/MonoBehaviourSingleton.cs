﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unity的Mono單例泛型
/// </summary>
/// <typeparam name="T">泛型</typeparam>
public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// 泛型單例實體
    /// </summary>
    private static T _instance = null;

    /// <summary>
    /// 單例外部接口
    /// </summary>
    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
                return null;
            if (_instance == null)
            {//未建立過，先尋找是否有同類物件
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {//未找到同類物件，新建該類物件(用類別名稱命名)
                    GameObject obj = new GameObject(typeof(T).Name);
                    //(選項)Object在新Scene中會保留，但不顯示在Hierarchy中
                    //obj.hideFlags = HideFlags.HideAndDontSave;
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
        set { }
    }

    private static bool applicationIsQuitting = false;

    /// <summary>
    /// 腳本建立時，檢查場景中是否已經存在對象的副本；
    /// 如果有將其摧毀以確保物件唯一性(單例)。
    /// </summary>
    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if(ShouldDestroyOnLoad() == false)
        {
            DontDestroyOnLoad(gameObject);
        }

        DidAwake();
    }

    protected virtual bool ShouldDestroyOnLoad()
    {
        return false;
    }

    protected virtual void DidAwake()
    {

    }

    private void OnDestroy()
    {
        applicationIsQuitting = true;
    }
}