using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModuleData
{
    [Header("基础信息")]
    public string moduleID;           // 模块唯一ID
    public string displayName;        // 显示名称
    public Sprite icon;               // 模块图标
    [TextArea] public string description; // 模块描述
    
    [Header("模块类型")]
    public ModuleType moduleType;     // 机体或武器
    
    [Header("效果数值")]
    public float speedBonus;          // 速度加成
    public float energyEfficiencyBonus;// 能量效率加成
    public float energyUpperBonus;    // 能量上限加成
    public float healthBonus;         // 生命加成
    public float defenseBonus;        // 防御加成
    public float purchasePrice;       // 购买价格
    public float salePrice;           // 出售价格
    
    [Header("其他设置")]
    public int maxStack = 1;          // 最大堆叠数
    //public Rarity rarity = Rarity.Common; // 稀有度
    
    /// <summary>
    /// 模块类型枚举
    /// </summary>
    public enum ModuleType
    {
        Body,    // 机体模组
        Weapon   // 武器模组
    }
    
    // /// <summary>
    // /// 稀有度枚举
    // /// </summary>
    // public enum Rarity
    // {
    //     Common,      // 普通 - 白色
    //     Uncommon,    // 稀有 - 绿色
    //     Rare,        // 罕见 - 蓝色
    //     Epic,        // 史诗 - 紫色
    //     Legendary    // 传说 - 橙色
    // }
}