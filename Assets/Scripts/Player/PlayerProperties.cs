using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour,IInjury,IPicker
{
    [SerializeField]float hpMax=100;
    [SerializeField]float hp=100;
    [SerializeField]float maxHp=100;
    [SerializeField]float atk=10;
    [SerializeField]float def=0.1f;
    [SerializeField]float maxEnergy = 100;
    [SerializeField]float curEnergy = 100;
    [SerializeField]float energyConsumption = 1f;

    public float CurrentEnergy {
        get => curEnergy;
        private set
        {
            EnergyValueChange?.Invoke(curEnergy/maxEnergy);
            curEnergy = value;
            curEnergy = Mathf.Clamp(curEnergy, 0, maxEnergy);
        }
    }
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

    private void Start()
    {
        StartCoroutine(ConsumeEnergy());
        StartCoroutine(CheckDrops());
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
            Debug.Log($"当前能量为：{curEnergy}");
        }
    }

    Collider[] drops=new Collider[100];
    [SerializeField] private float checkRadius = 2f;
    [SerializeField]LayerMask dropMask;
    IEnumerator CheckDrops()
    {
        WaitForSeconds waitSeconds = new WaitForSeconds(0.16f);
        while (true)
        {
            yield return waitSeconds;
            
            Physics.OverlapSphereNonAlloc(transform.position, checkRadius, drops,dropMask);
            
            foreach (Collider col in drops)
            {
                if (col == null) break;
                //Debug.Log(col.gameObject.name);
            }
            //Debug.Log(i);
            foreach (Collider col in drops)
            {
                //Debug.Log(col.gameObject.name+"第二循环");
                if (col == null)
                {
                    break;
                }
                
                if (col.TryGetComponent<IDrop>(out IDrop id))
                {
                    id.PickUp(this,transform);
                }
            }
            Array.Clear(drops, 0, drops.Length);
            //Debug.Log($"当前能量为：{curEnergy}");
        }
    }

    public void PickedUp(float value)
    {
        CurrentEnergy+=value;
    }
}
