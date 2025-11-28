using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : WeaponBase, IWeapon
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] LineRenderer laser;
    [SerializeField] private float laserDelay;
    [SerializeField] private float chargedDamage = 10f;
    [SerializeField] private float holdTime = 3f;
    private Reader reader;
    Camera mainCamera;
    [SerializeField]LayerMask layerMask;
    RaycastHit hit;
    bool isChargingShooted = false;
    
    Vector3 ShootPoint
    {
        get
        {
            Ray ray = mainCamera.ScreenPointToRay(reader.InputMousePos);
            
            if (Physics.Raycast(ray, out hit, 1000,layerMask))
            {
                return hit.point;
            }
            else
            {
                return Vector3.zero;
            }
        }
    } 
    
    

    private void Start()
    {
        reader = FindObjectOfType<Reader>();
        mainCamera = FindObjectOfType<Camera>();
        
    }

    private Coroutine chargingCoroutine;
    private void OnEnable()
    {
        chargingCoroutine = StartCoroutine(ChargedAttackDamage());
    }

    private void OnDisable()
    {
        StopCoroutine(chargingCoroutine);
    }

    float timer = 0;
    float hitTime = 0;
    public void Attack()
    {
        if (ShootPoint!=Vector3.zero)
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(ShootPoint - transform.position));
        }
        else
        {
            Instantiate(bulletPrefab, transform.position, transform.rotation);
        }
    }


    public void ChargedAttack()
    {
        //检测面朝方向 
        // if (Physics.Raycast(transform.position, transform.forward, out hit))
        // {
        //     //Debug.Log("长按攻击");
        //     laser.SetPosition(0, transform.position);
        //     laser.SetPosition(1, hit.point);
        // }
        // else
        // {
        //     laser.SetPosition(0, transform.position);
        //     laser.SetPosition(1, transform.position + transform.forward * 1000f);
        // }
        
        if (ShootPoint!=Vector3.zero)//视觉效果需要修改
        {
            //Debug.Log("长按攻击");
            laser.SetPosition(0, transform.position);
            laser.SetPosition(1, ShootPoint);
            isChargingShooted = true;
        }
        else
        {
            laser.SetPosition(0, transform.position);
            laser.SetPosition(1, transform.position + transform.forward * 1000f);
            isChargingShooted = false;
        }
    }

    IEnumerator ChargedAttackDamage()
    {
        WaitForSeconds wait = new WaitForSeconds(laserDelay);
        RaycastHit laserHit;
        Vector3 dir;
        while (true)
        {
            if (!isChargingShooted)//没射中就跳过
            {
                yield return wait;
                continue;
            }
            dir = hit.point - transform.position;
            if (Physics.Raycast(transform.position, dir, out laserHit, 1000, layerMask))
            {
                laserHit.collider.GetComponent<IInjury>()?.Inject(chargedDamage);
                //Debug.Log(hit.collider.gameObject.name);
            }
            
            yield return wait;
        }
    }

    public void AttackFinish()
    {
        timer = 0;
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, transform.position);
    }
}
