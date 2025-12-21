using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]UIView view;
    [SerializeField]PlayerStats data;
    [SerializeField]SimpleModuleManager moduleManager;

    private void Start()
    {
        view.Init(OnEquipButtonClicked);
        view.UpdateDisplay();
        data.EnergyValueChange += EnergyValueChangeHandler;
        moduleManager.EquippedBodyModule += EquippedBodyModuleEventHandler;//装备机体模组时触发
        moduleManager.PickedUpBodyModule += PickedUpBodyModuleEventHandler;//捡起机体模组时触发
    }

    private void PickedUpBodyModuleEventHandler(ModuleItem obj)
    {
        //捡起模组时展示模组 并暂停游戏
        view.ShowPickedUp(obj);
        PauseManager.Pause();
    }

    private EquipmentBar curEB;
    public void OnEquipButtonClicked(ModuleItem obj,EquipmentBar bar)
    {
        //点击装备栏 调用更换模组方法 解除暂停
        curEB = bar;
        moduleManager.ChangeEquipModule(obj);
        
    }

    private void EquippedBodyModuleEventHandler(ModuleItem obj)
    {
        //装备模组 更新UI信息
        view.UpdateEquippedBodyModule(obj,curEB);
        curEB = null;
    }

    private void EnergyValueChangeHandler(float value)
    {
        Debug.Log("EnergyValueChangeHandler");
        view.UpdateDisplay(value);
    }
    
}
