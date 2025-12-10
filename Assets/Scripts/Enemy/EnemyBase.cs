using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour,IInjury
{
    //敌人 移动 攻击
    [SerializeField]NavMeshAgent _navMeshAgent;
    [SerializeField]float speed = 1f;
    [SerializeField]float stoppingDistance = 1f;
    [SerializeField]float hp = 10f;
    [SerializeField]float damage = 5f;
    [SerializeField]float atkDelay = 1f;
    [SerializeField]float attackRadius = 0.4f;
    [SerializeField]float checkDelay = 0.5f;
    [SerializeField]float checkRadius = 1.5f;
    [SerializeField] private LayerMask atkMask;
    private Transform _target;

    public event EventHandler Dead;
    
    Transform Target
    {
        get { return _target; }
        set
        {
            _target = value;
            if (value != null)
            {
                MoveTowardsTarget(value.transform);
            }
        }
    }

    private IInjury injuryTarget;

    public void Init(Transform currentTarget)
    {
        //Target = currentTarget;
        _navMeshAgent.speed = speed;
        _navMeshAgent.stoppingDistance = stoppingDistance;
    }

    private void Start()
    {
        StartCoroutine(Move());
        StartCoroutine(Attack());
        StartCoroutine(CheckTarget());
    }
    
    protected virtual void MoveTowardsTarget(Transform target)
    {
        _navMeshAgent.SetDestination(target.position);
    }
    

    public bool Inject(float dmg,GameObject obj)
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

    void Dying()
    {
        //TODO:优化死亡逻辑
        Destroy(gameObject);
    }

    IEnumerator Attack()
    {
        while (true)
        {
            if (injuryTarget != null&& Vector3.Distance(transform.position,Target.position) <= attackRadius)
            {
                Debug.Log($"{gameObject.name}攻击");
                injuryTarget.Inject(damage,this.gameObject);
            }
            
            yield return new WaitForSeconds(atkDelay);
        }
    }

    IEnumerator Move()
    {
        while (true)
        {
            if (Target != null)
            {
                MoveTowardsTarget(Target.transform);
            }
            
            yield return new WaitForSeconds(0.2f);
        }
    }

    //检测周围目标
    public Collider[] colliders = new Collider[4];
    IEnumerator CheckTarget()
    {
        
        int i = 0;
        while (true)
        {
            yield return new WaitForSeconds(checkDelay);
            
            Physics.OverlapSphereNonAlloc(transform.position, checkRadius, colliders,atkMask);
            i++;
            foreach (Collider col in colliders)
            {
                if (col == null) break;
                Debug.Log(col.gameObject.name);
            }
            Debug.Log(i);
            foreach (Collider col in colliders)
            {
				//Debug.Log(col.gameObject.name+"第二循环");
                if (col == null)
                {
                    Target=this.transform;
                    injuryTarget=null;
                    break;
                }
                
                if (col.TryGetComponent<IInjury>(out IInjury ij))
                {
                    
                    Target = col.transform;
                    injuryTarget = ij;
                    break;
                }
                else
				{
                    Debug.Log("没有IInjury");
					Target=null;
 					injuryTarget=null;
				}
            }
            Array.Clear(colliders, 0, colliders.Length);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
