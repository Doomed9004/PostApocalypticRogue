using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField]private ModuleDataSO moduleDataSO;
    [SerializeField] private Transform[] points;
    [SerializeField]GameObject shopPrefab;

    private void Start()
    {
        for (int i = 0; i < points.Length; i++)
        {
            GameObject obj = Instantiate(shopPrefab, points[i].position, Quaternion.identity, points[i]);
            
        }
    }
}
