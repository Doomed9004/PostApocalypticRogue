using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]UIView view;
    [SerializeField]PlayerStats data;

    private void Start()
    {
        view.Init();
        view.UpdateDisplay();
        data.EnergyValueChange += EnergyValueChangeHandler;
    }

    private void EnergyValueChangeHandler(float value)
    {
        Debug.Log("EnergyValueChangeHandler");
        view.UpdateDisplay(value);
    }
}
