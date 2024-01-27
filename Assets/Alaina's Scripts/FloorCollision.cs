using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FloorCollision : MonoBehaviour
{
    [SerializeField] EvilCat evilCat;

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.name == "Cat food")
        {
            evilCat.MoveEvilCat();
        }
    }
}
