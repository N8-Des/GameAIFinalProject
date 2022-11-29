using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PursuitMovement : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Rigidbody rb;
    public Transform targetToFollow;
    public float speed = 1;
    void Update()
    {
        agent.enabled = true;
        agent.SetDestination(targetToFollow.position);
        rb.velocity = transform.forward * speed;
        rb.AddForce(0, -4000, 0);
    }
}
