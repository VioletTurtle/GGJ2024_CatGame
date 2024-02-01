using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyCatForce : MonoBehaviour
{
    public float power;
    private PlayerController player;
    private ParticleSystem hitParticles;

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            //Debug.Log("triggers");
            Vector3 dir = collision.transform.position - player.transform.position;
            collision.rigidbody.AddForce(dir.normalized * power, ForceMode.Impulse);
            collision.rigidbody.AddTorque(Vector3.forward, ForceMode.Impulse);
            hitParticles.Play();
        }
    }
}
