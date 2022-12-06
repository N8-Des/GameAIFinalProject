using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationManager : AnimationManager
{
    EnemyMovement enemyMovement;
    EnemyManager enemyManager;
    private void Awake()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        enemyManager = GetComponent<EnemyManager>();
    }
    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyManager.rb.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyManager.rb.velocity = velocity;
    }
    public void CanMoveAgain()
    {

    }
}
