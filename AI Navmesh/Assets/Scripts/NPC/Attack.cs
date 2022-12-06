using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{
    public State combat;
    public State pursuit;
    public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager animationManager)
    {
        enemyManager.navAgent.transform.localPosition = Vector3.zero;
        enemyManager.navAgent.transform.localRotation = Quaternion.identity;
        if (enemyManager.isInAction)
        {
            return combat;
        }
        if (currentAttack != null)
        {
            if(enemyManager.distanceFromTarget < currentAttack.minDistance)
            {
                return this;
            }
            else if (enemyManager.distanceFromTarget < currentAttack.maxDistance)
            {
                if(enemyManager.viewableAngle <= currentAttack.maximumAttackAngle && enemyManager.viewableAngle >= currentAttack.minimumAttackAngle)
                {
                    if(enemyManager.currRecoveryTime <= 0 && !enemyManager.isInAction)
                    {
                        animationManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                        animationManager.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                        animationManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                        enemyManager.isInAction = true;
                        enemyManager.currRecoveryTime = currentAttack.recoveryTime;
                        currentAttack = null;
                        return combat;
                    }
                }
            }
            float viewAngle = Vector3.Angle(enemyManager.currentTarget.transform.position - enemyManager.transform.position, enemyManager.transform.forward);
            if (viewAngle > currentAttack.maximumAttackAngle && viewAngle < currentAttack.minimumAttackAngle)
            {
                return pursuit;
            }
        }

        if (enemyManager.distanceFromTarget > enemyManager.stopDistance)
        {
            return pursuit;
        }
        else if(!GetAttack(enemyManager))
        {
           return pursuit;
        }

        return this;

    }


    public virtual bool GetAttack(EnemyManager enemyManager)
    {
        Vector3 targetDir = enemyManager.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDir, transform.forward);
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
        int maxScore = 0;
        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction attackAction = enemyAttacks[i];
            if (enemyManager.distanceFromTarget <= attackAction.maxDistance && enemyManager.distanceFromTarget >= attackAction.minDistance)
            {
                if (viewableAngle <= attackAction.maximumAttackAngle && viewableAngle >= attackAction.minimumAttackAngle)
                {
                    maxScore += attackAction.attackScore;
                }
            }
        }
        if (maxScore == 0)
        {
            return false;
        }
        int randomValue = Random.Range(0, maxScore + 1);
        int tempScore = 0;
        for (int i = 0; i < enemyAttacks.Length; i++)
        {

            EnemyAttackAction attackAction = enemyAttacks[i];
            if (enemyManager.distanceFromTarget <= attackAction.maxDistance && enemyManager.distanceFromTarget >= attackAction.minDistance)
            {
                if (viewableAngle <= attackAction.maximumAttackAngle && viewableAngle >= attackAction.minimumAttackAngle)
                {
                    if (currentAttack != null)
                    {
                        return true;
                    }
                    tempScore += attackAction.attackScore;
                    if (tempScore > randomValue)
                    {
                        currentAttack = attackAction;
                    }
                }
            }
        }
        return true;
    }
}

