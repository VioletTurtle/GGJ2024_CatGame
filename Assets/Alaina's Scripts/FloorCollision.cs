using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FloorCollision : MonoBehaviour
{
    
    
    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.name == "Explodable TV")
        {
            Debug.Log("TV Down");
        }
    }
}
