using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Animator animator;
    private float wait = 30f;
    private float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if(Random.Range(0,100) >= 98 && timer >= wait)
        {
            animator.SetBool("isIdling", false);
            int i = Random.Range(1, 4);
            if(i == 1)
            {
                animator.SetBool("Grounded", false);
            }
            else if(i == 2)
            {
                animator.SetBool("isAttacking", true);
            }
            else if (i == 3)
            {
                animator.SetBool("isDead", true);
            }

            timer = 0;
        }
        else
        {
            animator.SetBool("Grounded", true);
            animator.SetBool("isDead", false);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isIdling", true);
        }
    }
}
