using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class ModuleDrop : DropBase
{
    [SerializeField] ModuleDataSO moduleDataSO;
    ModuleItem moduleData;
    private ModuleEventArgs ea;

    private void Start()
    {
        int r = Random.Range(0,moduleDataSO.data.Count-1);
        moduleData = new ModuleItem(moduleDataSO.data[r]);
        ea = new ModuleEventArgs(moduleData);
    }

    protected override IEnumerator PickingCoroutine(IPicker picker, Transform trans)
    {
        yield return base.PickingCoroutine(picker, trans);
        picker.PickedUp(ea);
        
    }
}
