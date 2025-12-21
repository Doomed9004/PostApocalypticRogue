using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerStats : MonoBehaviour,IInjury,IPicker
{
    [SerializeField]float hp=100;
    [SerializeField]float maxHp=100;
    [SerializeField]float atk=10;
    
    [SerializeField]float def=0.1f;
    [SerializeField]float maxEnergy = 100;
    [SerializeField]float energyConsumption = 1f;
    [SerializeField]float moveSpeed=10f;
    
    [SerializeField]float curEnergy = 100;

    DropsPicker dropsPicker;
    #region 属性
    public float Hp
    {
        get => hp;
        set => hp = value;
    }

    public float MaxHp
    {
        get => maxHp;
        set => maxHp = value;
    }

    public float Atk
    {
        get => atk;
        set => atk = value;
    }

    public float Def
    {
        get => def;
        set => def = value;
    }

    public float MaxEnergy
    {
        get => maxEnergy;
        set => maxEnergy = value;
    }

    public float CurEnergy
    {
        get => curEnergy;
        set => curEnergy = value;
    }

    public float EnergyConsumption
    {
        get => energyConsumption;
        set => energyConsumption = value;
    }

    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }

    public float CurrentEnergy {
        get => curEnergy;
        private set
        {
            EnergyValueChange?.Invoke(curEnergy/maxEnergy);
            curEnergy = value;
            curEnergy = Mathf.Clamp(curEnergy, 0, maxEnergy);
        }
    }
    #endregion
    
    public event Action<float> EnergyValueChange;
    public bool Inject(float dmg,GameObject obj)
    {
        //减少能量
        float realDmg = dmg - (dmg * def);
        CurrentEnergy -= realDmg;
        
        return CurrentEnergy <= 0;
        // //减少血量
        // float realDmg=dmg - (dmg * def);
        // hp -= realDmg >= 0 ? realDmg : 0;
        // Debug.Log($"玩家受到伤害：{realDmg}  当前玩家血量：{hp}");
        //
        // return hp <= 0;
    }

    Coroutine consumeEnergyCoroutine;
    private void Start()
    {
        consumeEnergyCoroutine = StartCoroutine(ConsumeEnergy());
        dropsPicker = GetComponent<DropsPicker>();
        dropsPicker.OnEnergyDropChecked += (t) => { t.PickUp(this, transform); };
        
        foreach (var volume in VolumeManager.Volumes)
        {
            if (volume.vType == Volume.VolumeType.Safe)
            {
                volume.PlayerEnter+= PlayerEnterHandler;
                volume.PlayerExit += PlayerExitHandler;
            }
        }
    }

    /// <summary>
    /// 每隔1秒消耗一次能量
    /// </summary>
    /// <returns></returns>
    IEnumerator ConsumeEnergy()
    {
        WaitForSeconds waitSeconds = new WaitForSeconds(1f);
        while (true)
        {
            yield return waitSeconds;

            if (CurrentEnergy <= 0) continue;
            CurrentEnergy -= energyConsumption;
            //Debug.Log($"当前能量为：{curEnergy}");
        }
    }

    public void PickedUp(EventArgs e)
    {
        EnergyEventArgs args = e as EnergyEventArgs;
        float temp = CurrentEnergy;
        temp+=args.energy;
        temp = Mathf.Clamp(temp, 0, MaxEnergy);
        CurrentEnergy = temp;
    }

    void PlayerEnterHandler(Volume.VolumeType vType)
    {
        StopCoroutine(consumeEnergyCoroutine);
    }

    void PlayerExitHandler(Volume.VolumeType vType)
    {
        consumeEnergyCoroutine = StartCoroutine(ConsumeEnergy());
    }
}
