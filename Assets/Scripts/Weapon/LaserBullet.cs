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
    float timer = 0;
    
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
        //Debug.Log("Triggered");
        IInjury injury = other.gameObject.GetComponent<IInjury>();
        if (injury != null)
        {
            injury.Inject(damage,this.gameObject);
            Destroy(gameObject);
        }
    }
}
