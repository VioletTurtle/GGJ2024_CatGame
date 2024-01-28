using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.Mathematics;
using UnityEngine;

public class ExplodableTV : MonoBehaviour
{
    [SerializeField] ParticleSystem smoke;
    void Start(){
        smoke.Pause();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6)
        {
            smoke.Play();
            
        }
    }

}
