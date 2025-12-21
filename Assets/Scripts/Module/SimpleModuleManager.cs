using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 模块管理器 - 控制模块的装备和卸下
/// 挂载到玩家或武器上
/// </summary>
public class SimpleModuleManager : MonoBehaviour,IPicker
{
    [Header("槽位设置")]
    public int maxBodySlots = 3;      // 机体槽位数量
    public int maxWeaponSlots = 2;    // 武器槽位数量
    
    [Header("调试")]
    public bool showDebugInfo = true; // 是否显示调试信息
    
    // 已装备的模块
    private List<ModuleItem> equippedBodyModules = new List<ModuleItem>();
    private List<ModuleItem> equippedWeaponModules = new List<ModuleItem>();
    
    // 模块背包（未装备的模块）
    private List<ModuleItem> moduleInventory = new List<ModuleItem>();
    
    public event Action<ModuleItem> EquippedBodyModule;
    public event Action<ModuleItem> EquippedWeaponModule;
    public event Action<ModuleItem> PickedUpBodyModule;
    public event Action<ModuleItem> PickUpWeaponModule;
    
    // 属性引用（需要挂载到同一个GameObject）
    private PlayerStats playerStats;
    private WeaponBase weapon;
    [SerializeField] private DropsPicker dropsPicker;
    
    public ModuleItem testModuleItem;
    public ModuleDataSO dataSO;

    [ContextMenu("测试添加一个模组")]
    public void AddModuleTest()
    {
        testModuleItem = new ModuleItem(dataSO.data[0]);
        bool b = EquipModule(testModuleItem);
        Debug.Log(b);
    }

    private ModuleItem curPickedupModule;
    public void PickedUp(EventArgs e)
    {
        ModuleEventArgs args = e as ModuleEventArgs;
        
        //测试用：直接装备
        //EquipModule(args?.item);
        //TODO:展示装备UI界面
        curPickedupModule = args.item;
        switch (curPickedupModule.data.moduleType)
        {
            case ModuleData.ModuleType.Body:PickedUpBodyModule?.Invoke(curPickedupModule);
                break;
            case ModuleData.ModuleType.Weapon:PickUpWeaponModule?.Invoke(curPickedupModule);
                break;
        }
    }

    public void ChangeEquipModule(ModuleItem module)
    {
        if (module != null )
        {
            UnequipModule(module);
            EquipModule(curPickedupModule);
        }
        else
        {
            EquipModule(curPickedupModule);
        }
    }
    
    
    void Start()
    {
        // 获取组件引用
        playerStats = GetComponent<PlayerStats>();
        weapon = GetComponent<WeaponBase>();
        dropsPicker = GetComponent<DropsPicker>();
        dropsPicker.OnModuleDropChecked += (t) => { t.PickUp(this, transform); };
        
        if (playerStats == null)
            Debug.LogWarning("未找到PlayerStats组件！");
        if (weapon == null)
            Debug.LogWarning("未找到Weapon组件！");
        
        Log("模块管理器初始化完成");
    }
    
    
    /// <summary>
    /// 装备模块
    /// </summary>
    public bool EquipModule(ModuleItem module)
    {
        if (module == null || module.data == null)
        {
            LogWarning("尝试装备无效的模块");
            return false;
        }
        
        switch (module.data.moduleType)
        {
            case ModuleData.ModuleType.Body:
                return EquipBodyModule(module);
                
            case ModuleData.ModuleType.Weapon:
                return EquipWeaponModule(module);
                
            default:
                LogWarning($"未知模块类型: {module.data.moduleType}");
                return false;
        }
    }
    
    /// <summary>
    /// 装备机体模块
    /// </summary>
    private bool EquipBodyModule(ModuleItem module)
    {
        // 检查槽位是否已满
        if (equippedBodyModules.Count >= maxBodySlots)
        {
            LogWarning($"机体槽位已满 (最大: {maxBodySlots})");
            return false;
        }
        
        // 添加到装备列表
        equippedBodyModules.Add(module);
        module.isEquipped = true;
        
        // 从背包移除
        moduleInventory.Remove(module);
        
        //触发装备事件
        EquippedBodyModule?.Invoke(module);
        
        // 应用效果到玩家
        ApplyBodyModuleEffects(module);
        
        Log($"装备机体模块: {module.GetDisplayName()}");
        return true;
    }
    
    /// <summary>
    /// 装备武器模块
    /// </summary>
    private bool EquipWeaponModule(ModuleItem module)
    {
        if (equippedWeaponModules.Count >= maxWeaponSlots)
        {
            LogWarning($"武器槽位已满 (最大: {maxWeaponSlots})");
            return false;
        }
        
        equippedWeaponModules.Add(module);
        module.isEquipped = true;
        moduleInventory.Remove(module);
        
        
        //ApplyWeaponModuleEffects(module);
        
        Log($"装备武器模块: {module.GetDisplayName()}");
        return true;
    }
    
    /// <summary>
    /// 卸下模块
    /// </summary>
    public bool UnequipModule(ModuleItem module)
    {
        if (module == null || !module.isEquipped)
        {
            LogWarning("尝试卸下未装备的模块");
            return false;
        }
        
        bool removed = false;
        
        // 从对应列表移除
        switch (module.data.moduleType)
        {
            case ModuleData.ModuleType.Body:
                removed = equippedBodyModules.Remove(module);
                RemoveBodyModuleEffects(module);
                break;
                
            case ModuleData.ModuleType.Weapon:
                removed = equippedWeaponModules.Remove(module);
                //RemoveWeaponModuleEffects(module);
                break;
        }
        
        if (removed)
        {
            module.isEquipped = false;
            moduleInventory.Add(module); // 放回背包
            Log($"卸下模块: {module.GetDisplayName()}");
        }
        
        return removed;
    }
    public bool UnequipModule(int index)
    {
        if (equippedBodyModules.Count <=index)
        {
            LogWarning("尝试卸下未装备的模块");
            return false;
        }
        
        bool removed = false;
        
        // 从对应列表移除
        ModuleItem m = equippedBodyModules[index];
        equippedBodyModules.Remove(m);
        RemoveBodyModuleEffects(m);
        
        return removed;
    }
    
    /// <summary>
    /// 应用机体模块效果
    /// </summary>
    private void ApplyBodyModuleEffects(ModuleItem module)
    {
        if (playerStats == null) return;
        
        // 简单直接地修改属性
        ModuleData data=module.GetData();
        playerStats.MoveSpeed += data.speedBonus;
        playerStats.EnergyConsumption += data.energyEfficiencyBonus;
        playerStats.MaxEnergy += data.energyUpperBonus;
        playerStats.Def += data.defenseBonus;

        
        Log($"应用机体模块效果: +{module.GetTotalHealthBonus()}生命, +{module.GetTotalSpeedBonus()}速度");
    }
    
    /// <summary>
    /// 移除机体模块效果
    /// </summary>
    private void RemoveBodyModuleEffects(ModuleItem module)
    {
        if (playerStats == null) return;
        
        // 简单直接地修改属性
        ModuleData data=module.GetData();
        playerStats.MoveSpeed -= data.speedBonus;
        playerStats.EnergyConsumption -= data.energyEfficiencyBonus;
        playerStats.MaxEnergy -= data.energyUpperBonus;
        playerStats.Def -= data.defenseBonus;

    }
    
    /// <summary>
    /// 应用武器模块效果
    /// </summary>
    // private void ApplyWeaponModuleEffects(ModuleItem module)
    // {
    //     if (weapon == null) return;
    //     
    //     weapon.damage += module.GetTotalDamageBonus();
    //     weapon.fireRate += module.data.fireRateBonus * module.currentStack;
    //     
    //     Log($"应用武器模块效果: +{module.GetTotalDamageBonus()}伤害, +{module.data.fireRateBonus * module.currentStack}射速");
    // }
    
    /// <summary>
    /// 移除武器模块效果
    /// </summary>
    private void RemoveWeaponModuleEffects(ModuleItem module)
    {
        if (weapon == null) return;
        
        //weapon.damage -= module.GetTotalDamageBonus();
        //weapon.fireRate -= module.data.fireRateBonus * module.currentStack;
    }
    
    /// <summary>
    /// 添加模块到背包
    /// </summary>
    public void AddModuleToInventory(ModuleData moduleData)
    {
        ModuleItem newModule = new ModuleItem(moduleData);
        moduleInventory.Add(newModule);
        
        Log($"获得新模块: {newModule.GetDisplayName()} (添加到背包)");
    }
    
    /// <summary>
    /// 自动装备背包中的模块
    /// </summary>
    public void AutoEquipFromInventory()
    {
        // 尝试装备所有背包中的模块
        for (int i = moduleInventory.Count - 1; i >= 0; i--)
        {
            EquipModule(moduleInventory[i]);
        }
    }
    
    /// <summary>
    /// 计算总加成
    /// </summary>
    public ModuleData CalculateTotalBonuses()
    {
        ModuleData md = new ModuleData();
        
        // 计算机体模块加成
        foreach (var module in equippedBodyModules)
        {
            md.speedBonus += module.data.speedBonus;
            md.energyEfficiencyBonus += module.data.energyEfficiencyBonus;
            md.energyUpperBonus += module.data.energyUpperBonus;
            md.defenseBonus += module.data.defenseBonus;
        }
        
        // 计算武器模块加成
        foreach (var module in equippedWeaponModules)
        {
            //damage += module.GetTotalDamageBonus();
        }
        
        return md;
    }
    
    /// <summary>
    /// 获取所有已装备的模块
    /// </summary>
    public List<ModuleItem> GetAllEquippedModules()
    {
        List<ModuleItem> allModules = new List<ModuleItem>();
        allModules.AddRange(equippedBodyModules);
        allModules.AddRange(equippedWeaponModules);
        return allModules;
    }

    public List<ModuleItem> GetBodyEquippedModules()
    {
        return equippedBodyModules;
    }

    public List<ModuleItem> GetWeaponEquippedModules()
    {
        return equippedWeaponModules;
    }
    
    /// <summary>
    /// 获取背包中的模块
    /// </summary>
    public List<ModuleItem> GetInventoryModules()
    {
        return new List<ModuleItem>(moduleInventory);
    }
    
    /// <summary>
    /// 获取可装备的空槽位数量
    /// </summary>
    public int GetAvailableSlots(ModuleData.ModuleType type)
    {
        switch (type)
        {
            case ModuleData.ModuleType.Body:
                return maxBodySlots - equippedBodyModules.Count;
            case ModuleData.ModuleType.Weapon:
                return maxWeaponSlots - equippedWeaponModules.Count;
            default:
                return 0;
        }
    }
    
    /// <summary>
    /// 调试信息
    /// </summary>
    void OnGUI()
    {
        if (!showDebugInfo) return;
        
        GUILayout.BeginArea(new Rect(10, 10, 500, 500));
        GUILayout.Label("=== 模块系统调试 ===");
        
        GUILayout.Label($"机体槽位: {equippedBodyModules.Count}/{maxBodySlots}");
        GUILayout.Label($"武器槽位: {equippedWeaponModules.Count}/{maxWeaponSlots}");
        //GUILayout.Label($"背包模块: {moduleInventory.Count}");
        
        var bonuses = CalculateTotalBonuses();
        GUILayout.Label($"总加成: 速度{bonuses.speedBonus}, 能量效率{bonuses.energyEfficiencyBonus}, 能量上限{bonuses.energyUpperBonus},防御力{bonuses.defenseBonus}");
        
        GUILayout.EndArea();
    }
    
    /// <summary>
    /// 日志方法
    /// </summary>
    private void Log(string message)
    {
        if (showDebugInfo)
            Debug.Log($"[ModuleManager] {message}");
    }
    
    private void LogWarning(string message)
    {
        Debug.LogWarning($"[ModuleManager] {message}");
    }


}