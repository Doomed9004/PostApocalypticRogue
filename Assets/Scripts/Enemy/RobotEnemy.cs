using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotEnemy : EnemyBase
{
	//增加远程攻击手段
    
    protected override IEnumerator Attack()
    {
        yield return base.Attack();
    }

    protected override void Dying()
    {
        base.Dying();
    }
    
}
