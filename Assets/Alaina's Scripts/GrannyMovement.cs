using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrannyMovement : MonoBehaviour
{
    [SerializeField] Animator grammy;
    void Start(){
        grammy = GetComponent<Animator>();
    }
    public void MoveGrammy(){
        grammy.SetTrigger("GrammyStartMove");
        Debug.Log("Grammy on the MOVEEEEE");
    }
}
