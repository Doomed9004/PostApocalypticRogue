using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour,IInjury
{
    [SerializeField]float hpMax=100;
    [SerializeField]float hp=100;
    [SerializeField]float maxHp=100;
    [SerializeField]float atk=10;
    [SerializeField]float def=0.1f;
    public bool Inject(float dmg,GameObject obj)
    {
        float realDmg=dmg - (dmg * def);
        hp -= realDmg >= 0 ? realDmg : 0;
        Debug.Log($"玩家受到伤害：{realDmg}  当前玩家血量：{hp}");
        
        return hp <= 0;
    }
}
