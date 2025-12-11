using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrop : MonoBehaviour,IDrop
{
    [SerializeField] private float t=0.3f;
    [SerializeField] private float duration=0.3f;
    [SerializeField] private float energyDrop=0.4f;
    public float Value { get => energyDrop; }

    public void PickUp(IPicker picker,Transform trans)
    {
        gameObject.layer = 0;
        StartCoroutine(PickingCoroutine(picker,trans));
    }

    IEnumerator PickingCoroutine(IPicker picker,Transform trans)
    {
        WaitForEndOfFrame wf = new WaitForEndOfFrame();

        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer >= duration)break;
            this.transform.position = Vector3.Lerp(this.transform.position,trans.position,t);
            yield return wf;
        }
        picker.PickedUp(Value);
        Destroy(gameObject);
    }
}
