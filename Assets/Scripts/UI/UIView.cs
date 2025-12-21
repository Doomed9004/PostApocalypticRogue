using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    [SerializeField]Slider energySlider;
    [SerializeField]Transform equipNode;
    [SerializeField]List<EquipmentBar> bodyEquipBar;

    public void Init(Action<ModuleItem,EquipmentBar> onEquipBtnClick)
    {
        foreach (var body in bodyEquipBar)
        {
            //延迟一秒然后关关闭
            EquipmentBar bar = body;
            body.equipButton.onClick.AddListener(() =>
            {
                StartCoroutine(OnEquipBtnClick(onEquipBtnClick, bar));
                Debug.Log(bar.moduleItem);
            });
        }
    }

    private void Start()
    {
        //throw new NotImplementedException();
        equipNode.gameObject.SetActive(false);
    }

    public void UpdateDisplay(float energyValue=1)
    {
        energySlider.value = energyValue;
    }

    public void ShowPickedUp(ModuleItem obj)
    {
        //显示物品 装备按钮可以交互
        equipNode.gameObject.SetActive(true);
        
    }
    public void UpdateEquippedBodyModule(ModuleItem obj,EquipmentBar bar)
    {
        //更新装备物品 装备按钮不可以交互
        bar.SetModuleItem(obj);
    }

    IEnumerator OnEquipBtnClick(Action<ModuleItem,EquipmentBar> onEquipBtnClick, EquipmentBar bar)
    {
        float startTime = Time.unscaledTime; 
        onEquipBtnClick?.Invoke(bar.moduleItem,bar);
        while (true)
        {
            yield return null;
            if (Time.unscaledTime - startTime > 1f)
            {
                equipNode.gameObject.SetActive(false);
                PauseManager.Unpause();
                yield break;
            }
        }
    }
}
