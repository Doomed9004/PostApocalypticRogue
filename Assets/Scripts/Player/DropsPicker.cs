using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsPicker : MonoBehaviour
{
    public event Action<IDrop> OnEnergyDropChecked;
    public event Action<IDrop> OnModuleDropChecked;
    void Start()
    {
        StartCoroutine(CheckDrops());
    }

    Collider[] drops=new Collider[100];
    [SerializeField] private float checkRadius = 2f;
    [SerializeField]LayerMask dropMask;
    IEnumerator CheckDrops()
    {
        WaitForSeconds waitSeconds = new WaitForSeconds(0.16f);
        while (true)
        {
            yield return waitSeconds;
            
            Physics.OverlapSphereNonAlloc(transform.position, checkRadius, drops,dropMask);
            
            foreach (Collider col in drops)
            {
                if (col == null) break;
                //Debug.Log(col.gameObject.name);
            }
            //Debug.Log(i);
            foreach (Collider col in drops)
            {
                //Debug.Log(col.gameObject.name+"第二循环");
                if (col == null)
                {
                    break;
                }
                
                if (col.TryGetComponent<IDrop>(out IDrop id))
                {
                    if(id is EnergyDrop)OnEnergyDropChecked?.Invoke(id);
                    if(id is ModuleDrop)OnModuleDropChecked?.Invoke(id);
                }
            }
            Array.Clear(drops, 0, drops.Length);
        }
    }
}
