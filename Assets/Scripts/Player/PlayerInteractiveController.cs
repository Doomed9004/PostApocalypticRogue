using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractiveController : MonoBehaviour
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private Vector3 offset;
    [SerializeField] LayerMask  interactiveLayer;//暂时没用，以后可以设置检测层级

    private Reader reader;
    
    Collider[] colliders=new Collider[10];
    IInteractive iobj;//当前可交互物体

    private void Start()
    {
        reader = FindObjectOfType<Reader>();
    }

    private void FixedUpdate()
    {
        if(Physics.OverlapSphereNonAlloc(transform.position + offset, radius, colliders)==0) return;//如果检测后不存在物体，那么返回
        
        foreach (var col in colliders)
        {
            if(col==null)break;
            IInteractive i = col.GetComponent<IInteractive>();
            if(i!=null)
            {
                //interactiveObjs.Add(col.GetComponent<IInteractive>());
                iobj = i;
                return;
            }
        }
        
        iobj = null;
    }

    private void Update()
    {
        if (reader.Interactive && iobj != null)
        {
            iobj.Interaction();
            iobj = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + offset, radius);
    }
}
