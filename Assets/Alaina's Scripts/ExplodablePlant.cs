using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodablePlant : MonoBehaviour
{
    [SerializeField] GrannyMovement grammy;
    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.layer == 6)
        {
            Debug.Log("Plant hit the floor");
            grammy.MoveGrammy();
        }
    }
}
