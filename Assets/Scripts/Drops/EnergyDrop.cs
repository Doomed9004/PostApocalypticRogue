using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrop : DropBase,IDrop
{

    [SerializeField] private float energyDrop=0.4f;
    EnergyEventArgs ea { get; set; }

    private void Start()
    {
        ea=new EnergyEventArgs(energyDrop);
    }

    protected override IEnumerator PickingCoroutine(IPicker picker, Transform trans)
    {
        yield return base.PickingCoroutine(picker, trans);
        picker.PickedUp(ea);
        Debug.Log("Picked up Ani");
        yield break;
    }
}
