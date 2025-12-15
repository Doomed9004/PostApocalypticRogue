using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ModuleSO", menuName = "Module/ModuleSOList")]
public class ModuleDataSO : ScriptableObject
{
    public List<ModuleData> data = new List<ModuleData>();
}