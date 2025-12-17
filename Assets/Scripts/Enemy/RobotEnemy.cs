using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotEnemy : EnemyBase
{
    
    protected override IEnumerator Attack()
    {
        yield return base.Attack();
    }

    protected override void Dying()
    {
        base.Dying();
    }
    
}
