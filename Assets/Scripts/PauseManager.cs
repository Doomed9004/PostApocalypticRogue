using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Ins;
    public static bool IsPaused {get; private set;}
    private void Awake()
    {
        if (Ins == null)
        {
            Ins = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static event Action<bool> OnPauseChanged;
    [MenuItem("Pause Manager/Pause")]
    static void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0;
        OnPauseChanged?.Invoke(IsPaused);
    }
    
    [MenuItem("Pause Manager/Unpause")]
    static void Unpause()
    {
        IsPaused = false;
        Time.timeScale = 1;
        OnPauseChanged?.Invoke(IsPaused);
    }
    
}
