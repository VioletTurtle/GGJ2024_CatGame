using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingFall : MonoBehaviour
{
    [SerializeField] Animator paintingThatFalls;
    void Start(){
        paintingThatFalls = GetComponent<Animator>();
    }
    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.name == "Green Book")
        {
            Debug.Log("Painting down");
            KnockDownPainting();
        }
    }
    public void KnockDownPainting(){
        paintingThatFalls.SetTrigger("StartPaintingFall");
        
    }
}
