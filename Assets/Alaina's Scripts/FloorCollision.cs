using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FloorCollision : MonoBehaviour
{
    [SerializeField] EvilCat evilCat;
    [SerializeField] GrannyMovement grammy;
    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.name == "Cat food")
        {
            Debug.Log("Food hit the floor");
            evilCat.MoveEvilCat();
        }
        if (collision.gameObject.name == "Explodable Plant")
        {
            Debug.Log("Plant hit the floor");
            grammy.MoveGrammy();
        }
        if (collision.gameObject.name == "Explodable TV")
        {
            Debug.Log("TV Down");
        }
    }
}
