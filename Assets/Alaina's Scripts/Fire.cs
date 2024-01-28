using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] ParticleSystem flames;
    bool stopEmission;
    [SerializeField] float decreaseSpeed;
    [SerializeField] float emissionRate;
    ParticleSystem.EmissionModule flameEmission;
    [SerializeField] PlayerController playerController;
    
    void Start()
    {
        emissionRate = 80;
        flames = GetComponent<ParticleSystem>();
        flameEmission = flames.emission;
        stopEmission = false;
        decreaseSpeed = 0.5f;
    }
    void OnTriggerEnter(Collider collider){
        if (collider.gameObject.name == "Vase")
        {
            stopEmission = true;
        }
        if (collider.gameObject.tag == "Player"){
            if (stopEmission){
                playerController.ChimneyEscape();
            }
            else{ 
                playerController.BurnPlayer();
            }
            
        }
        
    }

    void Update()
    {
        
        flameEmission.rateOverTime = emissionRate;
        if (stopEmission)
        {
            emissionRate -= decreaseSpeed;
            
        }

    }
}
