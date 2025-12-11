using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    [SerializeField]Slider energySlider;

    public void Init()
    {
        
    }
    
    public void UpdateDisplay(float energyValue=1)
    {
        energySlider.value = energyValue;
    }
}
