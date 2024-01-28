using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatFood : MonoBehaviour
{
    [SerializeField] EvilCat evilCat;
    [SerializeField] ParticleSystem food;
    void Start(){
        food.Pause();
    }
    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.layer == 6)
        {
            Debug.Log("Food hit the floor");
            food.Play();
            evilCat.MoveEvilCat();
        }
    }

}
