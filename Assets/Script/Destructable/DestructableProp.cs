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
    public float forceMagnitude = 1;

    [Header("Components")]
    public Fracture fractureScript;
    public ParticleSystem breakParticles;
    //public Rigidbody[] pieces;
    public Rigidbody[] fragments;
    public Rigidbody rigidBody;
    public MMF_Player breakFeedbacks;
    private BoxCollider collider;

    //Motion and Force
    private bool _falling;
    private bool _destroyed;
    private float _distanceFallen;
    private Vector2 _dirLastCollision;
    private Vector2 _lastFramePosition;

    [Header("Debug and Gizmos")]
    public bool debug;
    public bool drawGizmos;
    public bool drawHeatRadius;

    private void Awake()
    {
        fractureScript = GetComponent<Fracture>();
        fragments = GetComponentsInChildren<Rigidbody>();
        rigidBody = GetComponent<Rigidbody>();
        //parentObject = transform.parent.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleFalls();
    }
    
    void Destruct()
    {
        breakFeedbacks.transform.SetParent(parentObject.transform);
        parentObject.transform.position = transform.position;
        breakFeedbacks.PlayFeedbacks();
        fractureScript.CauseFracture();
        //ApplyForceToFragments();
    }

    //Adds force to debris props with respect
    //to the direction of the last collision
    public void ApplyForceToFragments()
    {
        fragments = parentObject.GetComponentsInChildren<Rigidbody>();

        Debug.Log("ApplyForceToFragments called.");

        float torque;
        float forceMultiplier;

        //Unused for now. Maybe will be used
        //to add angular offset to each piece
        //of debris?
        float randomX;
        float randomY;

        foreach (Rigidbody piece in fragments)
        {
            //Randomize torque & force for more visual intrigue
            torque = Random.Range(-10f, 10f);
            forceMultiplier = Random.Range(1, 5);

            // Apply force & torque to child objects
            //piece.AddForce(_dirLastCollision * (forceMagnitude * forceMultiplier), ForceMode.Impulse);
            
            //Needs to be converted to 3D before this is reenabled
            //piece.AddTorque(torque, ForceMode.Impulse);

            piece.transform.SetParent(null);

            //GameManager.Instance.SendToGarigidBodyage(piece.gameObject);
        }
    }

    private void HandleFalls()
    {
        if (rigidBody.velocity.y < 0) _falling = true;
        else _falling = false;

        //Debug.Log("Distance fallen: " + _distanceFallen);

        if (_falling)
        {
            _distanceFallen += Vector2.Distance(transform.position, _lastFramePosition);
        }
        _lastFramePosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            // Get the contact normal (direction of collision)
            _dirLastCollision = contact.normal;
            forceMagnitude = collision.relativeVelocity.magnitude;

            if (!debug) break;
            Debug.Log("last collision direction: " + _dirLastCollision + " | Force Mag: " + forceMagnitude);
        }

        //Any super strong impact destroys instantly regardless of material
        if (forceMagnitude > forceToBreak) { Destruct(); return; }

        //Any long fall destroys instantly
        if (_distanceFallen >= fallDistanceToBreak) { Destruct(); return; }

        /* - - - - - Instant Destruction checks by material - - - - - */

        //Debris
        if (collision.gameObject.layer == 9 && forceMagnitude > debrisForceToBreak) { Destruct(); return; }

        //Metal
        else if (collision.gameObject.layer == 11 && forceMagnitude > metalForceToBreak) { Destruct(); return; }

        //Wood
        else if (collision.gameObject.layer == 10 && forceMagnitude > woodForceToBreak) { Destruct(); return; }

        /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */
        _distanceFallen = 0;

        if (health <= 0) { Destruct(); return; }
    }
}
