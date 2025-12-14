using UnityEngine;

/// <summary>
/// 模块实例 - 运行时使用的模块对象
/// 比ScriptableObject更轻量，适合运行时创建和修改
/// </summary>
[System.Serializable]
public class ModuleItem
{
    // 基础数据引用
    public ModuleData data;           // 模块配置数据
    
    // 运行时状态
    public int currentStack = 1;      // 当前堆叠数
    public bool isEquipped = false;   // 是否已装备
    
    /// <summary>
    /// 构造函数
    /// </summary>
    public ModuleItem(ModuleData data)
    {
        this.data = data;
        currentStack = 1;
    }
    
    /// <summary>
    /// 获取模块名称
    /// </summary>
    public string GetDisplayName()
    {
        return data.displayName;
    }
    
    /// <summary>
    /// 获取模块图标
    /// </summary>
    public Sprite GetIcon()
    {
        return data.icon;
    }
    
    /// <summary>
    /// 尝试增加堆叠
    /// </summary>
    public bool TryAddStack()
    {
        if (currentStack < data.maxStack)
        {
            currentStack++;
            return true;
        }
        return false;
    }
    
    /// <summary>
    /// 获取总加成值（考虑堆叠）
    /// </summary>
    public float GetTotalHealthBonus()
    {
        return data.healthBonus * currentStack;
    }
    
    public float GetTotalSpeedBonus()
    {
        return data.speedBonus * currentStack;
    }

    //TODO:增加其他堆叠
    public ModuleData GetTotalData()
    {
        ModuleData totalData = new ModuleData();
        totalData.speedBonus=data.speedBonus*currentStack;
        totalData.defenseBonus=data.defenseBonus*currentStack;
        totalData.energyEfficiencyBonus=data.energyEfficiencyBonus*currentStack;
        totalData.energyUpperBonus=data.energyUpperBonus*currentStack;
        
        return totalData;
    }

    public ModuleData GetData()
    {
        return data;
    }
    
    
    
    /// <summary>
    /// 模块描述（包含堆叠信息）
    /// </summary>
    public string GetFullDescription()
    {
        string stackInfo = data.maxStack > 1 ? 
            $"\n当前堆叠: {currentStack}/{data.maxStack}" : "";
        
        return data.description + stackInfo;
    }
}