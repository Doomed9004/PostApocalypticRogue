using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : WeaponBase, IWeapon
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] LineRenderer laser;
    [SerializeField] private float holdTime = 3f;
    private Reader reader;

    private void Start()
    {
        reader = FindObjectOfType<Reader>();
    }

    float timer = 0;
    float hitTime = 0;
    public void Attack()
    {
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }

    RaycastHit hit;
    public void ChargedAttack()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            Debug.Log("长按攻击");
            laser.SetPosition(0, transform.position);
            laser.SetPosition(1, hit.point);
        }
        else
        {
            laser.SetPosition(0, transform.position);
            laser.SetPosition(1, transform.position + transform.forward * 1000f);
        }

    }

    public void AttackFinish()
    {
        timer = 0;
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, transform.position);
    }
}
