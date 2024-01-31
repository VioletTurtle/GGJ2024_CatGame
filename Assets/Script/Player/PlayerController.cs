using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
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

    public Transform orientation;

    //player control
    private float horizontalInput = 0;
    private float verticalInput = 0;
    //Attack Vars
    private bool attacking = false;

    [Header("Other")]
    public CatAttackHitBox attackBox;

    //Alaina's code
    [SerializeField] GameObject player;
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

    public void attackStart() { attackBox.enableBoxes(); }
    public void attackEnd() { attackBox.disableBoxes(); }

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
        DoChaosDecay();
    }

    private void FixedUpdate()
    {
        //Debug.Log(targetDirection);
        if (!attacking)
        {
            FpsLook();
            Move();
        }
    }
    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0) && !attacking)
        {
            StartCoroutine(Attack());
        }

        if (verticalInput < 0)
        {
            verticalInput = 0;
        }
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            //Destroy(gameObject);
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
                Debug.Log("Running");
            }
            else
            {
                speed = 1.4f;
                _isRunning = false;
            }
        }
    }

    public void Move()
    {
        Vector3 movDirection = gameObject.transform.forward * verticalInput + gameObject.transform.right * horizontalInput;
        rb.AddForce(movDirection.normalized * speed * 10f * Time.deltaTime, ForceMode.VelocityChange);
        //Debug.Log(Mathf.Clamp(rb.velocity.magnitude, 0, 5)/5);
        
        /*
        Vector3 viewDir = transform.position - 
            new Vector3(virtualCamera.transform.position.x, 
            virtualCamera.transform.position.y, 
            virtualCamera.transform.position.z);
        orientation.forward = viewDir.normalized;
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero) 
        {
            transform.forward = Vector3.Slerp(transform.forward, inputDir.normalized, Time.deltaTime * 30f);
        }
        */

        animator.SetFloat("Blend", Mathf.Clamp(rb.velocity.magnitude, 0, 5) / 5);
    }


    private void FpsLook()
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
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(1f);
        attacking = false;
        animator.SetBool("isAttacking", false);
    }
}
