using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Audio")]
    public ManyLoopPlayer dynamicPlayer;
    public FootstepPlayer footstepPlayer;
    //controls audio

    //Camera Control

    [Header("Camera")]
    public GameObject cameraTarget;
    public Camera mainCamera;
    public CinemachineFreeLook virtualCamera;
    private float yCamRot;
    private float xCamRot;
    private float sensitivityX = 100f;
    private float sensitivityY = 100f;

    private Animator animator;
    private float speed = 1f; //Old value 0.05f
    private Rigidbody rb;
    private float jump = 6f;

    //player control
    private float horizontalInput = 0;
    private float verticalInput = 0;
    //Attack Vars
    private bool attacking = false;

    //Alaina's code

    [SerializeField] Material redMaterial;
    [SerializeField] Material originalMaterial;

    //Audio Scripts
    public FootstepPlayer stepController;

    [Header("Chaos Meter")]
    private int chaosMeter = 0;
    private bool chaosAdded = false;
    private int chaosMin = 0;
    private int chaosMax = 8;
    private bool _canAddChaos;

    private bool _isWalking;
    private bool _isRunning;

    [Header("Visual Feedback")]
    public MMF_Player jumpFeedbacks;
    public MMF_Player runFeedbacks;
    public ParticleSystem runParticles;

    [Header("Other")]
    public CatAttackHitBox attackBox;
    public TrailRenderer attackTrail;

    [Header("Looking")]
    public Transform player;
    public Transform playerObj;
    public Transform orientation;

    public float rotationSpeed;
    public GameObject thirdPersonCam;

    public CameraStyle currentStyle;
    public enum CameraStyle
    {
        Basic,
        Combat,
        Topdown
    }

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    Vector3 moveDirection;

    public void attackStart() { attackBox.enableBoxes(); }
    public void attackEnd() { attackBox.disableBoxes(); }

    private void Start()
    {
        //For the new movement
        rb.freezeRotation = true;
        readyToJump = true;
    }

    public void AddChaos()
    {
        if (_isRunning) chaosMin = 1;
            chaosMeter++;
        MusicController();
    }
    private async void DoChaosDecay()
    {
        while (true)
        {
            await Task.Delay(10000);
            if (chaosMeter > chaosMin)
            {
                chaosMeter--;
            }
        }
    }

    public void MusicController()
    {
        dynamicPlayer.setSongId(Mathf.Clamp(chaosMeter, chaosMin, chaosMax));
    }

    bool IsGrounded()
    {
        speed = 30f;
        float groundCheckDist = 0.3f;
        float groundBuffer = 0.2f;
        RaycastHit hit;

        Debug.DrawRay(transform.position + new Vector3(0, groundBuffer, 0.2f), new Vector3(0, -groundCheckDist, 0), Color.blue);
        Debug.DrawRay(transform.position + new Vector3(0, groundBuffer, -0.2f), new Vector3(0, -groundCheckDist, 0), Color.green);
        bool jointBool 
            = (Physics.Raycast(transform.position + new Vector3(0, groundBuffer, -0.2f), Vector3.down, out hit, groundCheckDist) 
            || Physics.Raycast(transform.position + new Vector3(0, groundBuffer, 0.2f), Vector3.down, out hit, groundCheckDist)) ||
            Physics.Raycast(transform.position + new Vector3(-0.2f, groundBuffer, 0), Vector3.down, out hit, groundCheckDist) ||
            Physics.Raycast(transform.position + new Vector3(0.2f, groundBuffer, 0), Vector3.down, out hit, groundCheckDist); 
        return jointBool;
    }

    private void Awake()
    {
        //playerAnimator.gameObject.GetComponent<Animator>().enabled = false;
        //player.GetComponent<SkinnedMeshRenderer>().material = player.GetComponent<SkinnedMeshRenderer>().materials[0];
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        footstepPlayer = GetComponentInChildren<FootstepPlayer>();
        attackTrail.emitting = false;
        DoChaosDecay();
    }

    private void FixedUpdate()
    {
        //Debug.Log(targetDirection);
        if (!attacking)
        {
            //MoveLegacy();
            //FpsLookLegacy();
            //HandleLooking();
            HandleMovement();
        }
    }
    private void Update()
    {
        /*
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (verticalInput < 0)
        {
            verticalInput = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            //Destroy(gameObject);
            jumpFeedbacks.PlayFeedbacks();
            rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
        }
        animator.SetBool("Grounded", IsGrounded());
        //Debug.Log(IsGrounded());

        if (!IsGrounded())
        {
            speed = Input.GetKey(KeyCode.LeftShift) ? 1f : 0.5f;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = 2f;
                _isRunning = true;
                chaosMax = 1;
                AddChaos();
                //Debug.Log("Running");
            }
            else
            {
                speed = 1.4f;
                _isRunning = false;
            }
        }
        */

        if (Input.GetMouseButtonDown(0) && !attacking)
        {
            StartCoroutine(Attack());
        }

        // ground check
        Vector3 raycastStart = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);
        grounded = Physics.Raycast(raycastStart, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        GatherInput();
        SpeedControl();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    void GatherInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && IsGrounded())
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void HandleMovement()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (IsGrounded())
        {
            //animator.SetBool("Grounded", true);
            animator.SetFloat("Blend", Mathf.Clamp(rb.velocity.magnitude, 0, 5) / 5);
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

            if (moveDirection.magnitude > 0 && !runParticles.isPlaying) runParticles.Play();
            else if (runParticles.isPlaying && moveDirection.magnitude == 0) runParticles.Stop();
        }  

        // in air
        else if (!IsGrounded())
        {
            if (runParticles.isPlaying) runParticles.Stop();
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        animator.SetBool("Grounded", IsGrounded());
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        jumpFeedbacks.PlayFeedbacks();
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    public void MoveLegacy()
    {
        Vector3 movDirection = gameObject.transform.forward * verticalInput + gameObject.transform.right * horizontalInput;
        rb.AddForce(movDirection.normalized * speed * 10f * Time.deltaTime, ForceMode.Force);
        //Debug.Log(Mathf.Clamp(rb.velocity.magnitude, 0, 5)/5);

        animator.SetFloat("Blend", Mathf.Clamp(rb.velocity.magnitude, 0, 5) / 5);
    }

    private void FpsLookLegacy()
    {
        //Calculate Camera Rots
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

        
        yCamRot += mouseX;
        //xCamRot -= mouseY;

        xCamRot = Mathf.Clamp(xCamRot, -30f, 60f);
        
        //Apply Camera Rots
        //cameraTarget.transform.rotation = Quaternion.Euler(xCamRot, yCamRot, 0);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yCamRot, 0), Time.deltaTime);

        Vector3 camRotation = mainCamera.transform.rotation.eulerAngles;
        Vector3 targetRotation = new Vector3(xCamRot, yCamRot, 0);
        cameraTarget.transform.DORotate(targetRotation, 0.3f);
        transform.DORotate(new Vector3(transform.rotation.x, yCamRot, transform.rotation.z), 0.3f, RotateMode.Fast);
    }

    public void BurnPlayer()
    {
        player.GetComponent<SkinnedMeshRenderer>().material = redMaterial;
    }
    public void StopBurnPlayer()
    {
        player.GetComponent<SkinnedMeshRenderer>().material = originalMaterial;
    }
    public void ChimneyEscape()
    {
        //playerAnimator.gameObject.GetComponent<Animator>().enabled = true;
        //playerAnimator.SetTrigger("ChimneyEscape");
    }
    void OnTriggerExit(Collider other)
    {
        Debug.Log("I work");
        StopBurnPlayer();
    }

    IEnumerator Attack()
    {
        attacking = true;
        if (IsGrounded()) attackTrail.emitting = true;
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.5f);
        attacking = false;
        attackTrail.emitting = false;
        animator.SetBool("isAttacking", false);
    }
}
