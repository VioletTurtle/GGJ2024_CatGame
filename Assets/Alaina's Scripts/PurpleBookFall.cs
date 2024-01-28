using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleBookFall : MonoBehaviour
{
    [SerializeField] Animator purpleBook;
   
    void Start(){
        purpleBook = GetComponent<Animator>();
    }
    public void ShakePurpleBook(){
        purpleBook.SetTrigger("StartBookFallPurp");
        Debug.Log("purple down");
    }

}
