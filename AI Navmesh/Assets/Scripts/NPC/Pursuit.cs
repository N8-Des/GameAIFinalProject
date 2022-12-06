using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursuit : State
{
    public State combatState;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager animationManager)
    {
        if (enemyManager.isInAction)
        {
            return this;
        }
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
        float viewAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        if (enemyManager.distanceFromTarget > enemyManager.stopDistance && !enemyManager.animManager.anim.GetBool("IsInteracting") 
            || (viewAngle < enemyManager.minDetectionAngle || viewAngle > enemyManager.maxDetectionAngle))
        {
            enemyManager.animManager.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            enemyManager.rb.velocity = enemyManager.transform.forward * enemyManager.moveSpeed;
        }

        ManageRotation(enemyManager);
        enemyManager.navAgent.transform.localPosition = Vector3.zero;
        enemyManager.navAgent.transform.localRotation = Quaternion.identity;
        if (enemyManager.distanceFromTarget <= enemyManager.stopDistance && viewAngle > enemyManager.minDetectionAngle && viewAngle < enemyManager.maxDetectionAngle)
        {
            return combatState;
        }
        else
        {
            return this;
        }
    }
    private void ManageRotation(EnemyManager enemyManager)
    {
        //wacky
        if (enemyManager.isInAction)
        {
            Vector3 dir = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            dir.y = 0;
            dir.Normalize();
            if (dir == Vector3.zero)
            {
                dir = enemyManager.transform.forward;
            }
            Quaternion targetRot = Quaternion.LookRotation(dir);
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRot, enemyManager.rotationSpeed / Time.deltaTime);
        }
        else
        {
            Vector3 relativeDir = enemyManager.transform.InverseTransformDirection(enemyManager.navAgent.desiredVelocity);
            Vector3 targetVel = enemyManager.rb.velocity;
            enemyManager.navAgent.enabled = true;
            enemyManager.navAgent.SetDestination(enemyManager.currentTarget.transform.position);
            enemyManager.rb.velocity = targetVel;
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
    }
}
