using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LadyBehavior : MonoBehaviour
{
    public GameObject Target;
    public float detectionRadius = 10f;
    public float detectionAngle = 45f; // Half of the total cone angle
    public float patrolSpeed = 2f;
    public Transform[] patrolPoints; // Define patrol points in the editor

    private Transform catTransform;
    private NavMeshAgent navMeshAgent;
    private int currentPatrolIndex = 0;

    private Animator animator;

    void Start()
    {
        catTransform = Target.transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetNextPatrolDestination();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (IsCatInSight())
        {
            // Cat detected, start pursuit
            navMeshAgent.speed = patrolSpeed;
            navMeshAgent.SetDestination(catTransform.position);
            animator.SetBool("IsRunning", true);
        }
        else
        {
            // Cat not detected, patrol the area
            navMeshAgent.speed = patrolSpeed;
            animator.SetBool("IsWalking", true);
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
            {
                animator.SetBool("IsIdle", true);
                SetNextPatrolDestination();
            }
        }
    }

    bool IsCatInSight()
    {
        Vector3 directionToCat = catTransform.position - transform.position;
        float angleToCat = Vector3.Angle(transform.forward, directionToCat);

        if (angleToCat < detectionAngle && directionToCat.magnitude < detectionRadius)
        {
            // Cat is within the cone of sight and detection radius
            return true;
        }

        return false;
    }

    void SetNextPatrolDestination()
    {
        // Set the next patrol destination point
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }
}
