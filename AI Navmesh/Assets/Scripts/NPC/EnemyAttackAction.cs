using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/EnemyActions")]
public class EnemyAttackAction : EnemyActions
{
    public int attackScore = 3;
    public float recoveryTime = 2;
    public float maximumAttackAngle = 35;
    public float minimumAttackAngle = -35;

    public float minDistance = 0;
    public float maxDistance = 3;
    public float pushAmount;
}
