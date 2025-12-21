using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    public static List<Volume> Volumes;
    private void Awake()
    {
        Volumes = FindObjectsByType<Volume>(FindObjectsSortMode.None).ToList();
    }
}
