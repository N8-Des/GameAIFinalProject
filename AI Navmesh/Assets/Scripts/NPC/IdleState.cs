using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public LayerMask detectionLayer;
    public State pursueState;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager animationManager)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);
        foreach (Collider coll in colliders)
        {
            CharacterStats characterStats = coll.GetComponent<CharacterStats>();
            if (characterStats != null)
            {
                //Team ID??
                Vector3 targetDirection = characterStats.transform.position - transform.position;
                float viewAngle = Vector3.Angle(targetDirection, transform.forward);
                if (viewAngle > enemyManager.minDetectionAngle && viewAngle < enemyManager.maxDetectionAngle)
                {
                    enemyManager.currentTarget = characterStats;
                }
            }
        }
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
        if(enemyManager.currentState != null)
        {
            return pursueState;
        }
        else
        {
            return this;
        }
    }
}
