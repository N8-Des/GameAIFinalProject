using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyManager : CharacterManager
{
    public bool isInAction;
    public DamagePlayer[] hitboxes;
    EnemyMovement enemyMovement;
    EnemyStats enemyStats;
    public bool holdingWeapon;
    [HideInInspector]
    public EnemyAnimationManager animManager;
    public CharacterStats currentTarget;
    public float distanceFromTarget;
    [HideInInspector]
    public NavMeshAgent navAgent;
    [HideInInspector]
    public Rigidbody rb;

    [Header("State stuff")]
    public State currentState;

    [Header("AI Settings")]
    public float detectionRadius = 30f;
    public float maxDetectionAngle = 50f;
    public float minDetectionAngle = -50f;
    public float moveSpeed = 1f;
    public float currRecoveryTime = 0;
    public float stopDistance = 1f;
    public float rotationSpeed = 25f;
    public float viewableAngle;

    private void Awake()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        animManager = GetComponent<EnemyAnimationManager>();
        enemyStats = GetComponent<EnemyStats>();
        navAgent = GetComponentInChildren<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        navAgent.enabled = false;

    }
    private void Start()
    {
        if(holdingWeapon)
        {
            animManager.PlayTargetAnimation("HoldingWeaponRight", false);
        }
        rb.isKinematic = false;
    }
    private void Update()
    {
        if (!isDead)
        {
            RecoveryTimer();
        }
    }
    private void FixedUpdate()
    {
        if (!isDead) 
        {
            StateMachine();
        }
    }
    private void StateMachine()
    {
        if (currentState != null)
        {
            State nextState = currentState.Tick(this, enemyStats, animManager);
            if (nextState != null)
            {
                SwitchToState(nextState);
            }
        }
    }
    void SwitchToState(State state)
    {
        currentState = state;
    }
    void RecoveryTimer()
    {
        if (currRecoveryTime > 0)
        {
            currRecoveryTime -= Time.deltaTime;
        }
        if (currRecoveryTime <= 0)
        {
            animManager.anim.SetBool("IsInteracting", false);
            isInAction = false;
        }
    }
    public void KillMe()
    {
        Destroy(this.gameObject);
    }
    public void PushAttack()
    {
        Vector3 moveDir = transform.forward;
        moveDir.Normalize();
        Vector3 pushVelocity = Vector3.ProjectOnPlane(moveDir, Vector3.zero);
        pushVelocity *= 2.5f * 100;
        rb.AddForce(pushVelocity);
    }
    public void EndInteracting()
    {
        animManager.anim.SetBool("IsInteracting", true);
    }
    public void SetRecovery(int rec)
    {
        currRecoveryTime = rec;
    }
}
