using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EvilCat : MonoBehaviour
{
    [SerializeField] Transform evilCat;
    public float speed;

    [SerializeField] Animator _animator;
    void Start(){
        _animator = GetComponent<Animator>();
        speed = 1.0f * Time.deltaTime;
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