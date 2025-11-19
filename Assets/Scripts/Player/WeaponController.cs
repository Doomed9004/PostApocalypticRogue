using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private List<IWeapon> weapons = new List<IWeapon>();
    public WeaponBase weapon;
    private IWeapon iweapon;
    private Reader reader;

    private void Start()
    {
        reader = FindObjectOfType<Reader>();
        iweapon = weapon as IWeapon;
    }


    private void Update()
    {
        if(reader.HoldShoot)
        {
            Debug.Log("1");
            iweapon.ChargedAttack();
            return;
        }
        else
        {
            Debug.Log("2");
            iweapon.AttackFinish();
        }
        
        if(reader.Shoot)
        {
            Debug.Log("3");
            iweapon.Attack();
        }
    }
}
