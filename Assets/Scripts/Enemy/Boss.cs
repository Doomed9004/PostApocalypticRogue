using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour,IInjury
{
    [SerializeField]float hp=20;

    public bool Inject(float dmg,GameObject obj)
    {
        hp -= dmg;
        Debug.Log($"Boss受到伤害：{dmg}  当前Boss血量：{hp}");
        
        return hp <= 0;
    }
}
