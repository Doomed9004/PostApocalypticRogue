using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Nest : MonoBehaviour
{
    //生成规则(暂时)：持续生成
    
    
    [SerializeField] private float generateRadius;//生成半径
    Vector3 GeneratePosition=>transform.position+Random.insideUnitSphere*generateRadius;

    [SerializeField] private float checkRadius=4;//检测建筑半径
    [SerializeField] private LayerMask checkMask;//建筑层级
    [SerializeField] List<GameObject> enemiesPrefabs = new List<GameObject>();
    Vector3 checkPosition;
    [SerializeField] private float generateDelay=0.5f;//生成间隔
    [SerializeField] private int threatenedCount = 10;//受到威胁时的生成量（周围存在建筑）
    List<GameObject> threatenedEnemies = new List<GameObject>();
    int curThreatenedCount;
    
    bool canGenerate;
    private void Start()
    {
        checkPosition = transform.position;
        StartCoroutine(GenerateCoroutine(generateDelay, enemiesPrefabs[0],null));
    }

    // //建筑建造时 检测周围是否存在建筑 如果存在建筑那么将canGenerate设置为true
    // [SerializeField]private Collider[] colliders = new Collider[1];
    // private void OnCheckBuild(object sender, EventArgs eventArgs)
    // {
    //     Transform targetBuild = (sender as GameObject).transform;
    //     if (targetBuild == null)//如果这里转换失败了，那么targetBuild为null，打印错误信息并返回
    //     {
    //         Debug.LogError("没有检测到BuildBase实例");
    //         return;
    //     }
    //     
    //     colliders[0] = null;
    //     Physics.OverlapSphereNonAlloc(checkPosition, checkRadius, colliders, checkMask);
    //     if (colliders[0] != null)//在周围检测到建筑后，调用Generate方法
    //     {
    //         Debug.Log("生成敌人");
    //         Generate(threatenedCount-threatenedEnemies.Count, generateDelay, enemiesPrefabs[0],targetBuild);
    //     }
    // }

    // IEnumerator CheckAroundTarget()
    // {
    //     while (true)
    //     {
    //         colliders[0] = null;
    //         Physics.OverlapSphereNonAlloc(checkPosition, checkRadius, colliders, checkMask);
    //         if (colliders[0] != null)//在周围检测到目标后，调用Generate方法
    //         {
    //             Debug.Log("生成敌人");
    //             StartCoroutine(GenerateCoroutine(threatenedCount - threatenedEnemies.Count, generateDelay,
    //                 enemiesPrefabs[0], null));
    //         }
    //         yield return new WaitForSeconds(0.5f);
    //     }
    // }

    // void Generate(int count, float delay,GameObject enemiesPrefab,Transform target)
    // {
    //     if (count == 0)
    //     {
    //         StartCoroutine(GenerateCoroutine(delay,enemiesPrefab,target));
    //     }
    //     else
    //     {
    //         StartCoroutine(GenerateCoroutine(count, delay, enemiesPrefab,target));
    //     }
    //     
    //     curThreatenedCount += count;
    // }

    IEnumerator GenerateCoroutine( float delay, GameObject enemiesPrefab,Transform target)
    {
        while (true)
        {
            if (threatenedEnemies.Count > threatenedCount)
            {
                yield return new WaitForSeconds(delay);
                continue;
            }
            
            //生成并初始化敌人实例
            GameObject obj = Instantiate(enemiesPrefab, GeneratePosition, Quaternion.identity);
            obj.GetComponent<EnemyBase>().Init(target);
            threatenedEnemies.Add(obj);
            
            yield return new WaitForSeconds(delay); 
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
