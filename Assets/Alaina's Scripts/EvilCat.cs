using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EvilCat : MonoBehaviour
{
    [SerializeField] GameObject evilCat;
    public float step = 0.36f;
    [SerializeField] GameObject foodTarget;
    [SerializeField] Animator _animator;
    void Start(){
        _animator = GetComponent<Animator>();

    }
    public void MoveEvilCat(){
        //_animator.SetTrigger("OnCatFoodDrop");
        Debug.Log("Evil Cat: FOOOOOD");
       
        evilCat.transform.position = Vector3.MoveTowards(evilCat.transform.position, foodTarget.transform.position, step);
        

    }

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("Death");
        }
    }
    /*void Update()
    {
        evilCat.transform.position = Vector3.MoveTowards(evilCat.transform.position, foodTarget.transform.position, step);
    }*/
}