using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleDrop : DropBase
{
    [SerializeField] ModuleDataSO moduleDataSO;
    ModuleData moduleData;
}

public interface IModule
{
    ModuleData data { get; set; }
}