using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] ParticleSystem flames;
    bool stopEmission;
    [SerializeField] float decreaseSpeed;
    [SerializeField] float pauseTime = 2f;
    private float time;
    [SerializeField] float emissionRate;
    ParticleSystem.EmissionModule flameEmission;
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
        
    }

    void Update()
    {
        
        flameEmission.rateOverTime = emissionRate;
        if (stopEmission)
        {
            emissionRate = emissionRate - decreaseSpeed;
            if (emissionRate == 0){
                stopEmission = false;
            }
        }

    }
}
