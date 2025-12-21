using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField]protected float atkRate = 0.75f;
    
}

public interface IWeapon
{
    public void Attack();//普通攻击
    public void ChargedAttack();//蓄力攻击
    public void AttackFinish();//攻击结束
}
