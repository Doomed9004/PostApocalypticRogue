using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]public class EnemyConfigs
    {
        public string name;
        public int count;//要生成的数量
        public int spawnedCount;//已经生成的数量
        public float weight;
        public GameObject prefab;
        public int defaultPoolSize=50;
        public int maxPoolSize=1000;
    }
    [System.Serializable]public class Wave
    {
        public string waveName;
        public List<EnemyConfigs> configs;
        public int waveQuota;//敌人总数
        public float spawnInterval;//生成间隔
        public int spawnedCount;//已经生成的数量
        public float totalWeight;//显示总权重
    }
    [Header("敌人相关配置")]
    [SerializeField] List<Wave> waves = new List<Wave>();
    [SerializeField] int currentWaveCount = 0;//index
    
    [Header("生成相关参数")]
    [SerializeField] Transform player;
    [SerializeField] float spawnORadius = 25f;
    [SerializeField] float spawnIRadius = 25f;
    [SerializeField] int maxIterations = 5;
    [SerializeField] float rayHeight = 2;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Transform poolTrans;
    private Vector3 RandomPosInConcentric {
        get
        {
            var dir = Random.insideUnitCircle.normalized;
            var radius = Random.Range(spawnIRadius, spawnORadius);
            var v3 = new Vector3(dir.x, 0, dir.y)*radius;
            var final = new Vector3(v3.x, rayHeight, v3.z)+player.position;
            Debug.DrawLine(player.position, final, Color.blue,1);
            return final;
        }
    }
    
    [SerializeField]
    Dictionary<string, Pool<GameObject>> poolDict = new Dictionary<string, Pool<GameObject>>();
    private void OnValidate()
    {
        if (spawnORadius < spawnIRadius)
            spawnORadius=spawnIRadius;
        
        if (spawnIRadius < 0)
            spawnIRadius = 0;
        
        if (spawnORadius < 0)
            spawnORadius = 0;

        if(waves.Count == 0)return;

        CalculateWaveQuota();
        CalculateTotalWeight();
    }

    private void Awake()
    {
        //初始化字典
        foreach (Wave wave in waves)
        {
            foreach (EnemyConfigs config in wave.configs)
            {
                if (poolDict.Count == 0)
                {
                    GameObject pre = config.prefab;
                    Pool<GameObject> p= new Pool<GameObject>(20,1000,pre,SpawnFunc,ActionOnGet,ActionOnRelease, ActionOnDestroy);
                    poolDict.Add(config.name,p);
                    continue;
                }

                //如果存在这种键 那么跳过
                if (poolDict.ContainsKey(config.name)) continue;
                
                //创建一个新的池子
                GameObject prefab = config.prefab;
                //Pool尺寸只会识别列表中第一个
                Pool<GameObject> newPool= new Pool<GameObject>(config.defaultPoolSize,config.maxPoolSize,prefab,SpawnFunc,ActionOnGet,ActionOnRelease, ActionOnDestroy);
                poolDict.Add(config.name,newPool);
            }
        }

        CalculateWaveQuota();
        CalculateTotalWeight();
    }

    private Coroutine spawnCoroutine;
    private void Start()
    {
        spawnCoroutine = StartCoroutine(SpawnEnemiesCoroutine());
        
        foreach (var volume in VolumeManager.Volumes)
        {
            if (volume.vType == Volume.VolumeType.Safe)
            {
                volume.PlayerEnter+= PlayerEnterHandler;
                volume.PlayerExit += PlayerExitHandler;
            }
        }
    }

    IEnumerator SpawnEnemiesCoroutine()
    {
        while (true)
        {
            if (currentWaveCount>=waves.Count)
            {
                yield break;
            }

            //Debug.Log(currentWaveCount);
            yield return new WaitForSeconds(waves[currentWaveCount].spawnInterval);
            SpawnEnemies();
        }
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var wave in waves)
        {
            foreach (var config in wave.configs)
            {
                currentWaveQuota+=config.count;
            }
            wave.waveQuota = currentWaveQuota;
            currentWaveQuota = 0;
        }
        //Debug.LogWarning(currentWaveQuota);
    }

    void CalculateTotalWeight()
    {
        float t = 0;
        foreach (var wave in waves)
        {
            foreach (var config in wave.configs)
            {
                t+=config.weight;
            }
            wave.totalWeight = t;
            t = 0;
        }
    }
    
    void SpawnEnemies()
    {
        if (FindSpawnPoint(out Vector3 pos))
        {
            //从对象池中获取对象并设置位置
            EnemyConfigs ec = RandomEnemyConfig();
            Pool<GameObject> pool = poolDict[ec.name];
            GameObject go = pool.Get(pos);
            //go.transform.position = pos;
            ec.spawnedCount++;
            waves[currentWaveCount].spawnedCount++;
        }
        
        if (waves[currentWaveCount].spawnedCount >= waves[currentWaveCount].waveQuota)
        {
            currentWaveCount++;
        }
    }

    Vector3 offset = new Vector3(0,2f,0);
    //在生成范围内寻找可以生成的位置
    bool FindSpawnPoint(out Vector3 pos)
    {
        //寻找生成位置
        for (int i = 0; i < maxIterations; i++)
        {
            Vector3 v = RandomPosInConcentric;
            if (Physics.Raycast(v, Vector3.down, out RaycastHit hit, rayHeight + 5, layerMask))
            {
                Debug.DrawRay(v, Vector3.down * hit.distance, Color.red);
                pos = hit.point+offset;
                return true;
            }
        }
        pos = Vector3.zero;
        return false;
    }

    //根据权重随机一种敌人
    EnemyConfigs RandomEnemyConfig()
    {
        float r = Random.Range(0,waves[currentWaveCount].totalWeight);
        float cumulative = 0;
        foreach (EnemyConfigs config in waves[currentWaveCount].configs)//遍历当前波次的配置表
        {
            cumulative += config.weight;
            
            if(config.spawnedCount>=config.count)continue;
            
            if (r < cumulative)
            {
                return config;
            }
        }
        return waves[currentWaveCount].configs[0];
    }
    
    void PlayerEnterHandler(Volume.VolumeType vType)
    {
        StopCoroutine(spawnCoroutine);
    }

    void PlayerExitHandler(Volume.VolumeType vType)
    {
        spawnCoroutine = StartCoroutine(SpawnEnemiesCoroutine());
    }

    private void OnDrawGizmos()
    {
        if(player == null)return;
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(player.position, spawnORadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(player.position, spawnIRadius);
    }

    #region 对象池方法
    GameObject SpawnFunc(GameObject obj)
    {
        GameObject go = Instantiate(obj);
        go.GetComponent<EnemyBase>().Init(player);//初始化敌人
        go.SetActive(false);
        return go;
    }
    void ActionOnGet(GameObject obj,Vector3 pos)
    {
        obj.transform.position = pos;
        obj.SetActive(true);
    }
    void ActionOnRelease(GameObject obj)
    {
        obj.SetActive(false);
    }
    void ActionOnDestroy(GameObject obj)
    {
        Destroy(obj);
    }
    #endregion
}
