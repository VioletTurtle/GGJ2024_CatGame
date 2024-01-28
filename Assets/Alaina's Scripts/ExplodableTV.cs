using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.Mathematics;
using UnityEngine;

public class ExplodableTV : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6)
        {
            /* void Explode()
            {
                var exp = GetComponent<ParticleSystem>();
                exp.Play();
                Destroy(gameObject);
            }
            //Debug.Log("tv on the floor"); */
            
        }
    }
    //on collision enter
        //if collisioin.layer = (layer of the floor)
            //print (tv on the floor)
    //figure out waht to do to make tv shatter
    //particle system, how to make object shatter in unity, animation
    //before destroy game.object       
    //try to leave particles
}
