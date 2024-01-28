using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatLady : MonoBehaviour
{
    public enum CatLadyState
    {
        Patrol,
        Pursuit,
        Cleaning,
        Special,
        nah,
        // Add more states as needed
    }

    private GameObject Target;
    public Transform cleanStart;
    public Transform cleanMid;
    public Transform cleanEnd;
    public Transform start;
    public float detectionRadius = 10f;
    public float detectionAngle = 45f;
    public float patrolSpeed = 2f;
    public float runSpeed = 4f;
    public float minIdleTime = 2f;
    public float maxIdleTime = 5f;
    private CharacterManager cm;

    private NavMeshAgent navMeshAgent;
    private float currentIdleTime;

    public Animator animator;
    public CatLadyState currentState;
    public nightStand nightStand;
    public GameObject nightyStan;

    private bool isCleaning;

    //special task shit
    private Vector3 specialTarget;


    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetRandomIdleTime();
        cm = GetComponent<CharacterManager>();
        currentState = CatLadyState.Patrol;
        isCleaning = false;
        nightyStan = nightStand.gameObject;
    }

    void Update()
    {
        switch (currentState)
        {
            case CatLadyState.Patrol:
                PatrolUpdate();
                break;
            case CatLadyState.Pursuit:
                PursuitUpdate();
                break;
            case CatLadyState.Cleaning:
                //CleanUpdate();
                break;
            case CatLadyState.Special:
                MoveTowardsTask();
                break;
            default:
                break;
        }
    }


    private void UpdateAnimator(string input)
    {
        animator.SetBool("IsWalking", input == "IsWalking");
        animator.SetBool("IsIdle", input == "IsIdle");
        animator.SetBool("IsRunning", input == "IsRunning");
    }

    void PatrolUpdate()
    {
        if (IsCatInSight())
        {
            TransitionToState(CatLadyState.Pursuit);
        }
        else
        {
            UpdateAnimator("IsWalking");
            navMeshAgent.speed = patrolSpeed;
            if (Vector3.Distance(transform.position, navMeshAgent.destination) < 0.5f)
            {
                UpdateAnimator("IsIdle");
                currentIdleTime -= Time.deltaTime;
                if (currentIdleTime <= 0f)
                {
                    MoveToRandomPosition();
                    SetRandomIdleTime();
                }
            }
        }
    }

    void PursuitUpdate()
    {
        UpdateAnimator("IsRunning");

        navMeshAgent.speed = runSpeed;
        navMeshAgent.SetDestination(Target.transform.position);

        if (!IsCatInSight())
        {
            TransitionToState(CatLadyState.Patrol);
        }
    }
    //Helpful Meth
    private void NavWalkTowards(Vector3 target)
    {
        UpdateAnimator("IsWalking");
        navMeshAgent.speed = patrolSpeed;
        navMeshAgent.SetDestination(target);
    }
    private void MoveTowardsTask()
    {
        if (specialTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, specialTarget, Time.deltaTime * patrolSpeed);
        }
    }
    //
    public void doClean()
    {
        StartCoroutine(CleanUpdate());
    }

    IEnumerator CleanUpdate()
    {
        NavWalkTowards(new Vector3(start.position.x, gameObject.transform.position.y, start.position.z));
        while ((Vector3.Distance(transform.position, navMeshAgent.destination) < 0.5f)) { }
        navMeshAgent.isStopped = true;
        transform.rotation = cleanStart.rotation;
        // Move towards the cleaning point
        currentState = CatLadyState.Special;
        specialTarget = cleanStart.position;

        yield return new WaitForSeconds(2f);
        currentState = CatLadyState.Cleaning;
        UpdateAnimator("IsIdle");
        nightStand.setMove();
        StartCoroutine(CleanIt());
        //yield return null;
    }
    IEnumerator CleanIt()
    {
        //Clean Task
        // Calculate the rotation towards the cleaning point
        Quaternion targetRotation = Quaternion.LookRotation(cleanMid.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 3f);

        //UpdateAnimator("IsIdle");

        // Move towards the cleaning point
        //yield return new WaitForSeconds(1f);
        currentState = CatLadyState.Special;
        specialTarget = cleanMid.position;

        // Check if cleaning at cleanMid is complete
        //yield return new WaitForSeconds(3f);
        UpdateAnimator("IsWalking");

        yield return new WaitForSeconds(1f);
        navMeshAgent.SetDestination(cleanEnd.position);
        navMeshAgent.isStopped = false;
        TransitionToState(CatLadyState.Patrol);
    }

IEnumerator MoveToCleaningPoint(Vector3 targetPoint)
{
    // Set the target rotation
    Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

    while (Vector3.Distance(transform.position, targetPoint) > 0.1f)
    {
        // Smoothly rotate towards the cleaning point
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 3f);

        // Smoothly move towards the cleaning point
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, Time.deltaTime * patrolSpeed);

        yield return null;
    }

    // Enable NavMeshAgent
    navMeshAgent.isStopped = false;

    // Transition back to patrol
    TransitionToState(CatLadyState.Patrol);
}
    void TransitionToState(CatLadyState newState)
    {
        currentState = newState;

        // Add any state-specific transition logic here
        if (currentState == CatLadyState.Patrol)
        {
            SetRandomIdleTime();
        }
    }

    bool IsCatInSight()
    {
        Vector3 directionToCat = Target.transform.position - transform.position;
        float angleToCat = Vector3.Angle(transform.forward, directionToCat);

        // Check if the cat is within the cone of sight and detection radius
        if (angleToCat < detectionAngle && directionToCat.magnitude < detectionRadius)
        {
            RaycastHit hit;

            // Cast a ray from the cat towards the player
            if (Physics.Raycast(transform.position, directionToCat, out hit, detectionRadius))
            {
                // Check if the ray hits the player (or any other relevant tag)
                if (hit.collider.CompareTag("Player"))
                {
                    // Cat has a direct line of sight to the player
                    return true;
                }
            }
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
            UpdateAnimator("IsIdle");
            //cm.Caught(this.gameObject, Target);
        }
    }
}
