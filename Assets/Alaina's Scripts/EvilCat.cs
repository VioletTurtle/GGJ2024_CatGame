using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EvilCat : MonoBehaviour
{
    [SerializeField] GameObject evilCat;
    public float step = 0.36f;
    [SerializeField] GameObject foodTarget;

    void Start(){
  

    }
    public void MoveEvilCat(){

        Debug.Log("Evil Cat: FOOOOOD");
       
        evilCat.transform.position = Vector3.MoveTowards(evilCat.transform.position, foodTarget.transform.position, step);
        evilCat.GetComponent<Collider>().enabled = false;

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