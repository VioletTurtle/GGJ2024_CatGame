using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{   private Vector3 playerMovement;
    [SerializeField] float playerSpeed = 10f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        playerMovement = new Vector3(-Input.GetAxis("Horizontal"), 0f, -Input.GetAxis("Vertical"));
        MovePlayer();
    }
    void MovePlayer(){
        Vector3 moveInput = transform.TransformDirection(playerMovement) * playerSpeed;
        rb.velocity = new Vector3(moveInput.x, rb.velocity.y, moveInput.z);
        if(Input.GetKeyDown(KeyCode.Space)){
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
