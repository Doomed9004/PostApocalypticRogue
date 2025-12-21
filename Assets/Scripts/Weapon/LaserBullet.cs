using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LaserBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float lifeTime=7;
    [SerializeField]LayerMask targetMask;
    float timer = 0;

    private void Start()
    {
        Debug.Log("子弹生成");
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            Destroy(gameObject);
        }
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if((1<<other.gameObject.layer)==targetMask)
        {
            Debug.Log("层级正确");
            if (other.gameObject.GetComponent<IInjury>() is IInjury injury)
            {
                Debug.Log("injury不为空");
                injury.Inject(damage, this.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
