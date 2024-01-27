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
    private Vector3 targetDirection = Vector3.forward;
    public float speed = 0.05f;
    public float jumping = 0f;
    public Rigidbody rb;
    public GameObject cameraTarget;
    private float jump = 3f;

    //Camera Control
    private float sensitivityX = 400f;
    private float sensitivityY = 400f;
    public CinemachineVirtualCamera virtualCamera;
    private float yCamRot;
    private float xCamRot;
    //player control
    private float horizontalInput = 0;
    private float verticalInput = 0;

    bool IsGrounded()
    {
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
        targetDirection = Vector3.zero;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");



        speed = Input.GetKey(KeyCode.LeftShift) ? 8f : 7f;
        Debug.Log(IsGrounded());
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            //Destroy(gameObject);
            rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
        }

        targetDirection = targetDirection.normalized;
        Debug.Log(targetDirection);
        FpsLook();
        Move();
    }

    public void Move()
    {
        float mod = speed * Time.deltaTime;
        Vector3 movDirection = gameObject.transform.forward * verticalInput + gameObject.transform.right * horizontalInput;
        rb.AddForce(movDirection.normalized * mod * 100f, ForceMode.Force);
    }
    private void FpsLook()
    {
        //Calculate Camera Rots
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

        yCamRot += mouseX;
        xCamRot -= mouseY;

        xCamRot = Mathf.Clamp(xCamRot, -90f, 90f);
        //Apply Camera Rots
        cameraTarget.transform.rotation = Quaternion.Euler(xCamRot, yCamRot, 0);
        transform.rotation = Quaternion.Euler(0, yCamRot, 0);
    }

}
