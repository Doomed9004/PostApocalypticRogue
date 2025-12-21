using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : WeaponBase, IWeapon
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] LineRenderer laser;
    [SerializeField] private float laserDelay=0.35f;
    [SerializeField] private float chargedDamage = 10f;
    [SerializeField] private float holdTime = 3f;
    private Reader reader;
    Camera mainCamera;
    [SerializeField]LayerMask enemyLayer;
    RaycastHit hit;
    bool Charging=false;
    bool isChargingShooted = false;
    bool canShoot=false;
    
    Vector3 ShootPoint
    {
        get
        {
            Ray ray = mainCamera.ScreenPointToRay(reader.InputMousePos);
            
            if (Physics.Raycast(ray, out hit, 1000,enemyLayer))
            {
                return hit.point;
            }
            else
            {
                return Vector3.zero;
            }
        }
    }

    Vector3 ShootObjPoint
    {
        get
        {
            RaycastHit lHit;
            Vector3 dir = hit.point - transform.position;
            if (Physics.Raycast(transform.position, dir, out lHit, 1000, enemyLayer))
            {
                return lHit.point;
            }
            else
            {
                return hit.point;
            }
        }
    }
    
    

    private void Start()
    {
        reader = FindObjectOfType<Reader>();
        mainCamera = FindObjectOfType<Camera>();
        
    }

    private Coroutine chargingCoroutine;
    private Coroutine atkCoroutine;
    private void OnEnable()
    {
        chargingCoroutine = StartCoroutine(ChargedAttackDamage());
        atkCoroutine = StartCoroutine(AttackCoroutine());
    }

    private void OnDisable()
    {
        StopCoroutine(chargingCoroutine);
        StopCoroutine(atkCoroutine);
    }

    float timer = 0;
    float hitTime = 0;
    public void Attack()
    {
        if(canShoot)
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(transform.forward) );
            canShoot = false;
        }
    }


	//蓄力攻击
    public void ChargedAttack()
    {
         Charging=true;
         laser.SetPosition(0, transform.position);
         //检测面朝方向 
         if (Physics.Raycast(transform.position, transform.forward, out hit,enemyLayer))
         {
             laser.SetPosition(1, hit.point);
             isChargingShooted = true;
         }
         else
         {
             laser.SetPosition(1, transform.position + transform.forward * 100f);
             isChargingShooted = false;
         }
        
        // if (ShootPoint!=Vector3.zero)//视觉效果需要修改
        // {
        //     //Debug.Log("长按攻击");
        //     laser.SetPosition(0, transform.position);
        //     laser.SetPosition(1, ShootObjPoint);
        //     isChargingShooted = true;
        // }
        // else
        // {
        //     laser.SetPosition(0, transform.position);
        //     laser.SetPosition(1, transform.position + transform.forward * 1000f);
        //     isChargingShooted = false;
        // }
    }
    //普通攻击
    IEnumerator AttackCoroutine()
    {
        float currentTime = Time.time;
        WaitForSeconds wait = new WaitForSeconds(atkRate);
        while (true)
        {
            canShoot = true;
            yield return wait;
            //Debug.Log(Time.time - currentTime);
            currentTime = Time.time;
        }
    }

    //蓄力攻击的伤害判定
    IEnumerator ChargedAttackDamage()
    {
        WaitForSeconds wait = new WaitForSeconds(laserDelay);
        RaycastHit laserHit;
        while (true)
        {
            if (!isChargingShooted)//没射中就跳过
            {
                yield return wait;
                continue;
            }
            if (!Charging)//没射中就跳过
            {
                yield return wait;
                continue;
            }

            
            Vector3 dir = transform.forward;
            //dir = hit.point - transform.position;
            
            Debug.DrawLine(transform.position, transform.position+transform.forward*100,Color.red);
                //Debug.Log(transform.forward);
            if (Physics.Raycast(transform.position, dir, out laserHit, 100, enemyLayer))
            {
                laserHit.collider.GetComponent<IInjury>()?.Inject(chargedDamage,this.gameObject);
                Debug.Log(laserHit.collider.gameObject.name);
            }
            
            yield return wait;
        }
    }

    public void AttackFinish()
    {
        Charging = false;
        isChargingShooted = false;
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, transform.position);
    }
}
