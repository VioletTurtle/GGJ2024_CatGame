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
    public float speed = 0.1f;
    public float jumping = 0f;

    //player control
    private float horizontalInput = 0;
    private float verticalInput = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        targetDirection = Vector3.zero;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        speed = Input.GetKey(KeyCode.LeftShift) ? 1f : 0.5f;

        if (Input.GetKey(KeyCode.Space))
        {
            //characterController.isGrounded = false;
            jumping = 100f;
        }
        else
        {
            //jumping = 0f;
        }

        targetDirection = targetDirection.normalized;
        Debug.Log(targetDirection);
        Move();
    }

    public void Move()
    {
        float mod = speed;
        Vector3 movDirection = gameObject.transform.forward * verticalInput + gameObject.transform.right * horizontalInput;
        movDirection = movDirection.normalized* mod + gameObject.transform.up * 10f * jumping;
        characterController.SimpleMove(movDirection.normalized * mod);
        //characterController.
    }
}
