using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatLady : MonoBehaviour
{
    public GameObject Target;
    public float detectionRadius = 10f;
    public float detectionAngle = 45f; // Half of the total cone angle
    public float patrolSpeed = 2f;
    public float minIdleTime = 2f;
    public float maxIdleTime = 5f;

    private Transform catTransform;
    private NavMeshAgent navMeshAgent;
    private float idleTime;
    private float currentIdleTime;

    void Start()
    {
        catTransform = Target.transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetRandomIdleTime();
        MoveToRandomPosition();
    }

    void Update()
    {
        if (IsCatInSight())
        {
            // Cat detected, start pursuit
            navMeshAgent.speed = patrolSpeed;
            navMeshAgent.SetDestination(catTransform.position);
        }
        else
        {
            // Cat not detected, move aimlessly or idle
            navMeshAgent.speed = patrolSpeed;

            if (navMeshAgent.remainingDistance < 0.5f)
            {
                // If close to the destination, idle for a random time
                currentIdleTime -= Time.deltaTime;
                if (currentIdleTime <= 0f)
                {
                    MoveToRandomPosition();
                    SetRandomIdleTime();
                }
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

    void MoveToRandomPosition()
    {
        // Move the Crazy Cat Lady to a random position within the navmesh
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas);
        navMeshAgent.SetDestination(hit.position);
    }

    void SetRandomIdleTime()
    {
        // Set a random idle time between minIdleTime and maxIdleTime
        currentIdleTime = Random.Range(minIdleTime, maxIdleTime);
    }
}
