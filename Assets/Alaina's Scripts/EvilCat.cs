using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EvilCat : MonoBehaviour
{
    [SerializeField] Transform evilCat;
    [SerializeField] Animator _animator;
    void Start(){
        _animator = GetComponent<Animator>();
    }
    public void MoveEvilCat(){
        _animator.SetTrigger("OnCatFoodDrop");
        Debug.Log("Evil Cat: FOOOOOD");
    }

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("Death");
        }
    }
}