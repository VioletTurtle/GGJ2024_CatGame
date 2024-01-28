using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatLady : MonoBehaviour
{
    private GameObject Target;
    public float detectionRadius = 10f;
    public float detectionAngle = 45f; // Half of the total cone angle
    public float patrolSpeed = 2f;
    public float runSpeed = 4f;
    public float minIdleTime = 2f;
    public float maxIdleTime = 5f;
    private bool patrol;
    private CharacterManager cm;

    private NavMeshAgent navMeshAgent;
    private float currentIdleTime;

    public Animator animator;

    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetRandomIdleTime();
        MoveToRandomPosition();
        //animator = GetComponent<Animator>();
        patrol = true;
        cm = GetComponent<CharacterManager>();
    }

    void Update()
    {
        if (IsCatInSight())
        {
            animator.SetBool("IsRunning", true);
            // Cat detected, start pursuit
            navMeshAgent.speed = runSpeed;
            navMeshAgent.SetDestination(Target.transform.position);
            if (navMeshAgent.remainingDistance < 0.5f)
            {
                // If close to the destination, idle for a random time
                animator.SetBool("IsIdle", true);
                patrol = false;
            }
        }
        else if (patrol)
        {
            // Cat not detected, move aimlessly or idle
            navMeshAgent.speed = patrolSpeed;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsWalking", true);

            if (navMeshAgent.remainingDistance < 0.5f)
            {
                // If close to the destination, idle for a random time
                animator.SetBool("IsWalking", false);
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
        Vector3 directionToCat = Target.transform.position - transform.position;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cm.Caught(this.gameObject, Target);
        }
    }
}
