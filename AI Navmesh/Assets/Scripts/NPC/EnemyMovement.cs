using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    EnemyManager enemyManager;
    EnemyAnimationManager enemyAnim;
    public LayerMask detectionLayer;
    EnemyStats thisEnemy;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        detectionLayer = LayerMask.GetMask("EnemyDetection");
        thisEnemy = GetComponent<EnemyStats>();
        enemyAnim = GetComponent<EnemyAnimationManager>();

    }
    private void Start()
    {

    }
    private void Update()
    {
    }

    public void MoveToTarget()
    {
      
    }

}
