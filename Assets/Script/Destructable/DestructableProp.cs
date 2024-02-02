using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestructableProp : MonoBehaviour
{
    [Header("Core Stats")]
    [SerializeField]
    private float health;
    public GameObject parentObject;
    public bool containsItem;
    [Range(1, 2)]
    public int itemCategory;    //Specifies the category of items to be spawned in the crate:
                                //small items, large items, melee items, etc.
    public GameObject itemInContainer;

    [Header("Force Limits")]
    //Configurable force limits per material
    public float forceToBreak;
    public float fallDistanceToBreak;
    public float playerForceToBreak;
    public float metalForceToBreak;
    public float woodForceToBreak;
    public float debrisForceToBreak;

    [Header("Debris Stats")]
    public int debrisLifetime;
    //Chance for each piece of debris to despawn
    [Range(0, 100)]
    public int debrisDecayChance;


    [Header("Components")]
    public Prefracture fractureScript;
    public ParticleSystem breakParticles;
    //public Rigidbody[] pieces;
    public DebrisProp[] fragments;
    public Rigidbody rigidBody;
    public MMF_Player breakFeedbacks;
    private BoxCollider collider;

    //Motion and Force
    private bool _falling;
    private bool _destroyed;
    private float _distanceFallen;

    private Collision _lastCollision;
    private Vector3 _lastVelocity;
    private Vector3 _dirLastCollision;
    private Vector3 _lastFramePosition;

    private float _collisionForceMagnitude;
    private Vector3 _collisionVelocity;

    [Header("Debug and Gizmos")]
    public bool debug;
    public bool drawGizmos;
    public bool drawHeatRadius;
    public bool disableDistruction;


    private void Awake()
    {
        //Initialize Default Values if nothing
        //is set in the inspector
        if (forceToBreak == 0) forceToBreak = 1000;
        if (fallDistanceToBreak == 0) fallDistanceToBreak = 1000;
        if (playerForceToBreak == 0) playerForceToBreak = 1000;
        if (metalForceToBreak == 0) metalForceToBreak = 1000;
        if (woodForceToBreak == 0) woodForceToBreak = 1000;
        if (debrisForceToBreak == 0) debrisForceToBreak = 1000;


    //fractureScript = GetComponent<Prefracture>();
    rigidBody = GetComponent<Rigidbody>();
        fragments = GetComponentsInChildren<DebrisProp>();
        
        foreach (DebrisProp fragment in fragments)
        {
            fragment.gameObject.SetActive(false);
        }
        //parentObject = transform.parent.gameObject;
    }

    void FixedUpdate()
    {
        _lastVelocity = rigidBody.velocity;

        HandleFalls();
    }
    
    public bool IsDestroyed()
    {
        return _destroyed;
    }

    public float GetHealth()
    {
        return health;
    }

    //Public call for the Destruct() method
    public void DoDestruct()
    {
        Destruct();
    }

    private void Destruct()
    {
        if (disableDistruction) return;

        //breakFeedbacks.transform.SetParent(parentObject.transform);
        breakFeedbacks.PlayFeedbacks();
        //parentObject.transform.position = transform.position;

        rigidBody.isKinematic = true;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<MeshCollider>().enabled = false;

        foreach (DebrisProp fragment in fragments)
        {
            fragment.gameObject.SetActive(true);
        }
        ApplyForceToFragments();
    }

    //Adds force to debris props with respect
    //to the direction of the last collision
    public void ApplyForceToFragments()
    {
        //Skip this method if we haven't
        //made fragments for this object yet
        if (fragments == null) return;

        Vector3 torque;
        float movingForceMultiplier;
        float stationaryForceMultiplier;

        //Unused for now. Maybe will be used
        //to add angular offset to each piece
        //of debris?
        float randomX;
        float randomY;

        foreach (DebrisProp piece in fragments)
        {
            //Randomize torque & force for more visual intrigue
            torque = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            movingForceMultiplier = Random.Range(1, 5);
            stationaryForceMultiplier = Random.Range(0.3f, 1f);

            Rigidbody rbody = piece.gameObject.GetComponent<Rigidbody>();

            // Apply force & torque to child objects...


            //If hit by a moving object
            if (_lastCollision.rigidbody != null && _lastCollision.rigidbody.velocity.magnitude > 0)
            {
                if (debug) Debug.Log("Hit by moving object");
                rbody.AddForce(_dirLastCollision * (_collisionForceMagnitude * movingForceMultiplier), ForceMode.Impulse);
            } 
            
            else //If hit by a static, stationary object
            {
                if (debug) Debug.Log("Broke on stationary object.");
                rbody.AddForce(_lastVelocity * (_collisionForceMagnitude * stationaryForceMultiplier), ForceMode.Impulse);
            }
            
            rbody.AddTorque(torque, ForceMode.Impulse);

            rbody.transform.SetParent(null);

            GameManager.Instance.SendToGarbage(piece.gameObject);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health < 0) Destruct();
    }

    private void HandleFalls()
    {
        if (rigidBody.velocity.y < 0) _falling = true;
        else _falling = false;

        //Debug.Log("Distance fallen: " + _distanceFallen);

        if (_falling)
        {
            _distanceFallen += Vector3.Distance(transform.position, _lastFramePosition);
        }
        _lastFramePosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        _lastCollision = collision;

        foreach (ContactPoint contact in collision.contacts)
        {
            // Get the contact normal (direction of collision)
            _dirLastCollision = contact.normal;
            _collisionForceMagnitude = collision.relativeVelocity.magnitude;

            if (!debug) break;
            //Debug.Log("last collision direction: " + _dirLastCollision + " | Force Mag: " + _collisionForceMagnitude);
        }

        //Any super strong impact destroys instantly regardless of material
        if (_collisionForceMagnitude > forceToBreak) { Destruct(); return; }

        //Any long fall destroys instantly
        if (_distanceFallen >= fallDistanceToBreak) { Destruct(); return; }

        /* - - - - - Instant Destruction checks by material - - - - - */

        //Debris
        if (collision.gameObject.layer == 9 && _collisionForceMagnitude > debrisForceToBreak) { Destruct(); return; }

        //Metal
        else if (collision.gameObject.layer == 11 && _collisionForceMagnitude > metalForceToBreak) { Destruct(); return; }

        //Wood
        else if (collision.gameObject.layer == 10 && _collisionForceMagnitude > woodForceToBreak) { Destruct(); return; }

        /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */
        _distanceFallen = 0;

        if (health <= 0) { Destruct(); return; }
    }
}
