using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBookFall : MonoBehaviour
{
    [SerializeField] Animator greenBook;
    void Start(){
        greenBook = GetComponent<Animator>();
    }
    public void ShakeGreenBook(){
        greenBook.SetTrigger("StartBookFallGreen");
        Debug.Log("green down");
    }
}
