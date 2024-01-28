using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatFood : MonoBehaviour
{
    [SerializeField] EvilCat evilCat;
    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.layer == 6)
        {
            Debug.Log("Food hit the floor");
            evilCat.MoveEvilCat();
        }
    }

}
