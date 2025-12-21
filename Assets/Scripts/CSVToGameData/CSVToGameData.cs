using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVToGameData : MonoBehaviour
{
    [SerializeField] TextAsset CSVFile;
    [SerializeField] private ModuleDataSO dataSO;

    [ContextMenu("读取CSV")]
    public void CSV2ModuleData()
    {
        dataSO.data.Clear();
        
        //按换行符分成行 一处为空的行
        string[] lines = CSVFile.text.Split('\n',StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < lines.Length; i++)//跳过标题行
        {
            //按逗号分隔成字段 移出为空的字段
            string[] fields = lines[i].Split(',',StringSplitOptions.RemoveEmptyEntries);

            ModuleData data = new ModuleData
            {
                moduleID = fields[0].Trim(),
                displayName = fields[1].Trim(),
                description = fields[2].Trim(),
                moduleType = ModuleData.ModuleType.Body,
                speedBonus = float.Parse(fields[3].Trim()),
                energyEfficiencyBonus = float.Parse(fields[4].Trim()),
                energyUpperBonus = float.Parse(fields[5].Trim()),
                defenseBonus = float.Parse(fields[6].Trim()),
                purchasePrice = int.Parse(fields[7].Trim()),
                salePrice = int.Parse(fields[8].Trim())
            };
            dataSO.data.Add(data);
        }  
        
        Debug.Log("导入完成");
    }
}
