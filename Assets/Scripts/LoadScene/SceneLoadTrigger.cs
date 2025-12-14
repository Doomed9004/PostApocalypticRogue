using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTrigger : MonoBehaviour
{
    [SerializeField]SceneAsset[] scenesToLoad;
    [SerializeField]SceneAsset[] scenesToUnload;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            LoadScene();
            UnloadScene();
        }
    }

    private void LoadScene()
    {
        for (int i = 0; i < scenesToLoad.Length; i++)
        {
            bool isSceneLoaded = false;
            //如果需要被加载的场景没被加载过
            for (int j = 0; j < SceneManager.sceneCount; j++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(j);
                if (loadedScene.name == scenesToLoad[i].name)
                {
                    isSceneLoaded = true;
                    break;
                }
            }
            
            //那么加载场景
            if (!isSceneLoaded)
            {
                Debug.Log($"加载场景{scenesToLoad[i].name}");
                SceneManager.LoadSceneAsync(scenesToLoad[i].name, LoadSceneMode.Additive);
            }
        }
    }
    private void UnloadScene()
    {
        for (int i = 0; i < scenesToUnload.Length; i++)
        {
            //如果需要被卸载的场景正在被加载 那么卸载
            for (int j = 0; j < SceneManager.sceneCount; j++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(j);
                if (loadedScene.name == scenesToUnload[i].name)
                {
                    Debug.Log($"卸载场景{loadedScene.name}");
                    SceneManager.UnloadSceneAsync(scenesToUnload[i].name);
                }
            }
        }
    }

}
