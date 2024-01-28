using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private float speed = 0.05f;
    private Rigidbody rb;
    public GameObject cameraTarget;
    private float jump = 6f;
    //controls music
    private int chaosMeter = 0;
    private bool chaosAdded = false;
    public ManyLoopPlayer dynamicPlayer;

    //Camera Control
    private float sensitivityX = 100f;
    private float sensitivityY = 100f;
    public CinemachineVirtualCamera virtualCamera;
    private float yCamRot;
    private float xCamRot;
    //player control
    private float horizontalInput = 0;
    private float verticalInput = 0;
    //Attack Vars
    private bool attacking = false;
    public CatAttackHitBox attackBox;

    //Alaina's code
    [SerializeField] GameObject player;
    [SerializeField] Material redMaterial;
    [SerializeField] Material originalMaterial;
    //[SerializeField] Animator playerAnimator;

    public void attackStart() { attackBox.enableBoxes(); }
    public void attackEnd() { attackBox.disableBoxes(); }

    public void moarchaos()
    {
        chaosMeter++;
    }
    IEnumerator tooCalm()
    {
        while (true)
        {
            if (!chaosAdded && chaosMeter > 5)
            {
                chaosMeter--;
            }
            yield return new WaitForSeconds(10f);
        }
    }

    public void MusicController()
    {
        dynamicPlayer.setSongId(Mathf.Clamp(chaosMeter, 0, 10));
    }

    bool IsGrounded()
    {
        speed = 30f;
        float groundCheckDist = 0.3f;
        float groundBuffer = 0.2f;
        RaycastHit hit;
        Debug.DrawRay(transform.position + new Vector3(0, groundBuffer, -0.1f), new Vector3(0, -groundCheckDist, 0), Color.red);
        return Physics.Raycast(transform.position + new Vector3(0, groundBuffer, -0.1f), Vector3.down, out hit, groundCheckDist);
    }

    private void Awake()
    {
        //playerAnimator.gameObject.GetComponent<Animator>().enabled = false;
        player.GetComponent<SkinnedMeshRenderer>().material = player.GetComponent<SkinnedMeshRenderer>().materials[0];
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        StartCoroutine(tooCalm());
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
        Debug.Log(IsGrounded());

        if (!IsGrounded())
        {
            speed = Input.GetKey(KeyCode.LeftShift) ? 1f : 0.5f;
        }
        else
        {
            speed = Input.GetKey(KeyCode.LeftShift) ? 2f : 1.3f;
        }
    }

    public void Move()
    {
        Vector3 movDirection = gameObject.transform.forward * verticalInput;//+ gameObject.transform.right * horizontalInput;
        rb.AddForce(movDirection.normalized * speed * 10f * Time.deltaTime, ForceMode.VelocityChange);
        //Debug.Log(Mathf.Clamp(rb.velocity.magnitude, 0, 5)/5);
        animator.SetFloat("Blend", Mathf.Clamp(rb.velocity.magnitude, 0, 5) / 5);
        if (rb.velocity.magnitude > 0)
        {
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yCamRot, 0), Time.deltaTime * 1f);
        }

    }
    private void FpsLook()
    {
        //Calculate Camera Rots
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

        yCamRot += mouseX;
        xCamRot -= mouseY;

        xCamRot = Mathf.Clamp(xCamRot, -30f, 60f);
        //Apply Camera Rots
        cameraTarget.transform.rotation = Quaternion.Euler(xCamRot, yCamRot, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yCamRot, 0), Time.deltaTime);
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
