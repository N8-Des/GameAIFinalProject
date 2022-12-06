using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : State
{
    public State attack;
    public State pursuit;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager animationManager)
    {
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        enemyManager.navAgent.transform.localPosition = Vector3.zero;
        enemyManager.navAgent.transform.localRotation = Quaternion.identity;
        if (enemyManager.currRecoveryTime <= 0 && enemyManager.distanceFromTarget <= enemyManager.stopDistance)
        {
            return attack;
        }else if (enemyManager.distanceFromTarget > enemyManager.stopDistance)
        {
            return pursuit;
        }
        else
        {
            return this;
        }
    }
}
