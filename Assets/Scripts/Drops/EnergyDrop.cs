using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrop : DropBase,IDrop
{

    [SerializeField] private float energyDrop=0.4f;
    public override float Value { get => energyDrop; }
    
    protected override IEnumerator PickingCoroutine(IPicker picker, Transform trans)
    {
        yield return base.PickingCoroutine(picker, trans);
        picker.PickedUp(Value);
        Debug.Log("Picked up Ani");
        yield break;
    }
}
