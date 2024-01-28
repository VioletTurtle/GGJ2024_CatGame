using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{   private Vector3 playerMovement;
    [SerializeField] float playerSpeed = 10f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject player;
    [SerializeField] Material redMaterial;
    [SerializeField] Material originalMaterial;
    [SerializeField] Animator playerAnimator;
    void Start()
    {
        playerAnimator.gameObject.GetComponent<Animator>().enabled = false;
        player.GetComponent<MeshRenderer>().material = player.GetComponent<MeshRenderer>().materials[0];
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
    public void burnPlayer()
    {
        player.GetComponent<MeshRenderer>().material = redMaterial;
    }
    public void stopBurnPlayer()
    {
        player.GetComponent<MeshRenderer>().material = originalMaterial;
    }
    public void ChimneyEscape()
    {
        playerAnimator.gameObject.GetComponent<Animator>().enabled = true;
        playerAnimator.SetTrigger("ChimneyEscape");
    }
    void OnTriggerExit(Collider other)
    {
        Debug.Log("I work");
        stopBurnPlayer();
    }

}
