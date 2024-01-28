using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    public CharacterController characterController;
    public Camera cam;
    private float speed = 0.05f;
    public Rigidbody rb;
    public GameObject cameraTarget;
    private float jump = 7f;

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

    public void attackStart() { attackBox.enableBoxes(); }
    public void attackEnd() { attackBox.disableBoxes(); }

    bool IsGrounded()
    {
        speed = 30f;
        float groundCheckDist = 0.3f;
        float groundBuffer = 0.2f;
        RaycastHit hit;
        Debug.DrawRay(transform.position + new Vector3(0, groundBuffer, 0), new Vector3(0, -groundCheckDist, 0), Color.red);
        return Physics.Raycast(transform.position + new Vector3(0, groundBuffer, 0), Vector3.down, out hit, groundCheckDist);
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
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

        if (!IsGrounded())
        {
            speed = Input.GetKey(KeyCode.LeftShift) ? 60f : 40f;
        }
        else
        {
            speed = Input.GetKey(KeyCode.LeftShift) ? 200f : 120f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            //Destroy(gameObject);
            rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
        }
        //Debug.Log(targetDirection);
        if (!attacking)
        {
            Move();
            FpsLook();
        }
    }

    public void Move()
    {
        Vector3 movDirection = gameObject.transform.forward * verticalInput;//+ gameObject.transform.right * horizontalInput;
        rb.AddForce(movDirection.normalized * speed * 10f * Time.deltaTime, ForceMode.Force);
        Debug.Log(Mathf.Clamp(rb.velocity.magnitude, 0, 5)/5);
        animator.SetFloat("Blend", Mathf.Clamp(rb.velocity.magnitude, 0, 5) / 5);
        if (rb.velocity.magnitude > 0)
        {
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yCamRot, 0), Time.deltaTime * 1f);
        }

    }
    private void FpsLook()
    {
        //Calculate Camera Rots
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivityY;

        yCamRot += mouseX;
        xCamRot -= mouseY;

        xCamRot = Mathf.Clamp(xCamRot, -30f, 60f);
        //Apply Camera Rots
        cameraTarget.transform.rotation = Quaternion.Euler(xCamRot, yCamRot, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yCamRot, 0), Time.deltaTime * 1f);
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
