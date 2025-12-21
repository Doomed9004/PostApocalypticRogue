using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class EquipmentBar : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public ModuleItem moduleItem;

    private TextMeshProUGUI displayText;
    public Button  equipButton;

    public event Action<ModuleItem> PointerEnter;
    public event Action PointerExit;
    private void Awake()
    {
        equipButton=GetComponent<Button>();
        displayText=GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetModuleItem(ModuleItem moduleItem)
    {
        this.moduleItem = moduleItem;
        displayText.text = moduleItem.data.displayName;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(moduleItem.data.displayName);
        PointerEnter?.Invoke(moduleItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExit?.Invoke();
    }
}
