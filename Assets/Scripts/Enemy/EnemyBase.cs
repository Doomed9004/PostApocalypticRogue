using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour,IInjury
{
    //敌人 移动 攻击
    [SerializeField]NavMeshAgent _navMeshAgent;
    [SerializeField]protected float speed = 1f;
    [SerializeField]protected float stoppingDistance = 1f;
    [SerializeField]protected float hp = 10f;
    [SerializeField]protected float damage = 5f;
    [SerializeField]protected float atkDelay = 1f;
    [SerializeField]protected float attackRadius = 0.4f;
    [SerializeField]protected float checkDelay = 0.5f;
    [SerializeField]protected float checkRadius = 1.5f;
    [SerializeField]LayerMask atkMask;
    protected Transform _target;

    [SerializeField]GameObject drops;//掉落物预制体

    public event EventHandler Dead;
    
    protected virtual Transform Target
    {
        get => _target;
        set => _target = value;
    }

    protected IInjury injuryTarget;

    public virtual void Init(Transform target)
    {
        Target = target;
        _navMeshAgent.speed = speed;
        _navMeshAgent.stoppingDistance = stoppingDistance;
    }

    private void OnEnable()
    {
        _navMeshAgent.Warp(transform.position);
    }

    protected virtual void Start()
    {
        StartCoroutine(Move());
        StartCoroutine(Attack());
        //StartCoroutine(CheckTarget());
    }
    
    protected virtual void MoveTowardsTarget(Transform target)
    {
        _navMeshAgent.SetDestination(target.position);
    }
    

    public virtual bool Inject(float dmg,GameObject obj)
    {
        hp-=dmg;
        Debug.Log($"被{obj.name}攻击");
        if(hp<=0)
        {
            Dead?.Invoke(this, null);
            Dying();
            return false;
        }
        return true;
    }

    protected virtual void Dying()
    {
        //TODO:优化死亡逻辑
        Debug.Log("enemy死亡");
        Instantiate(drops, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    protected virtual IEnumerator Attack()
    {
        WaitForSeconds wait = new WaitForSeconds(atkDelay);
        while (true)
        {
            if (injuryTarget != null&& Vector3.Distance(transform.position,Target.position) <= attackRadius)
            {
                Debug.Log($"{gameObject.name}攻击");
                injuryTarget.Inject(damage,this.gameObject);
            }
            
            yield return wait;
        }
    }

    protected virtual IEnumerator Move()
    {
        WaitForSeconds  wait = new WaitForSeconds(0.2f);
        while (true)
        {
            if (Target != null)
            {
                MoveTowardsTarget(Target.transform);
            }

            yield return wait;
        }
    }

    //检测周围目标
    // public Collider[] colliders = new Collider[4];
    // protected virtual IEnumerator CheckTarget()
    // {
    //     int i = 0;
    //     WaitForSeconds wait = new WaitForSeconds(atkDelay);
    //     while (true)
    //     {
    //         yield return wait;
    //         
    //         Physics.OverlapSphereNonAlloc(transform.position, checkRadius, colliders,atkMask);
    //         i++;
    //         foreach (Collider col in colliders)
    //         {
    //             if (col == null) break;
    //             //Debug.Log(col.gameObject.name);
    //         }
    //         //Debug.Log(i);
    //         foreach (Collider col in colliders)
    //         {
				// //Debug.Log(col.gameObject.name+"第二循环");
    //             if (col == null)
    //             {
    //                 Target=this.transform;
    //                 injuryTarget=null;
    //                 break;
    //             }
    //             
    //             if (col.TryGetComponent<IInjury>(out IInjury ij))
    //             {
    //                 
    //                 Target = col.transform;
    //                 injuryTarget = ij;
    //                 break;
    //             }
    //             else
				// {
    //                 //Debug.Log("没有IInjury");
				// 	Target=null;
 			// 		injuryTarget=null;
				// }
    //         }
    //         Array.Clear(colliders, 0, colliders.Length);
    //     }
    // }


    [Header("显示调试信息")]
    public bool debug=false;
    private void OnDrawGizmos()
    {
        if (!debug)return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
