using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [MenuItem("Game Manager/TimePause")]
    static void TimePause()
    {
        Time.timeScale = 0;
    }
    [MenuItem("Game Manager/TimeRestore")]
    static void TimeRestore()
    {
        Time.timeScale = 1;
    }
}
