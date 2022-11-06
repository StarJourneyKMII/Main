using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

namespace MiProduction.Scene
{
    public abstract class SceneConfigAsset<T> : ScriptableObject
    {
        public SceneConfig<T>[] SceneConfigs;

        public bool TryGetSceneData(out T data)
        {
            if(SceneConfigs == null || SceneConfigs.Length <= 0)
            {
                Debug.LogError("There is no data in SceneConfig!");
                data = default;
                return false;
            }
            
            foreach(var configData in SceneConfigs)
            {
                if(configData.Scene == SceneManager.GetActiveScene().path)
                {
                    data = configData.Data;
                    return true;
                }
            }

            Debug.LogError($"Can't get {nameof(T)} in Scene:{SceneManager.GetActiveScene().name}, please check SceneConfig asset");
            data = default;
            return false;
        }

        //public object GetSceneData()
        //{
        //    return SceneConfigs.Where(x => x.Scene == SceneManager.GetActiveScene().path).Select(x => x.Data).First();
        //}

        
        
    } 
}
