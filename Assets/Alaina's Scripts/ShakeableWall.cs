using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeableWall : MonoBehaviour
{
    [SerializeField] GreenBookFall greenBook;
    [SerializeField] PurpleBookFall purpleBook;
    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.name == "Knockable Wardrobe")
        {
            purpleBook.ShakePurpleBook();
            greenBook.ShakeGreenBook();
            Debug.Log("Shake");
        }
    }
}
