using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wardrobe : MonoBehaviour
{
    [SerializeField] GreenBookFall greenBook;
    [SerializeField] PurpleBookFall purpleBook;
    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.layer == 7)
        {
            purpleBook.ShakePurpleBook();
            greenBook.ShakeGreenBook();
            Debug.Log("Shake");
        }
    }

}
