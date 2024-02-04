using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(DestructableProp))]

public class FlammableProp : MonoBehaviour
{
    [Header("Fire Dynamics")]
    public bool onFire;
    public bool flammable;
    public bool spreadsFire;
    public float heatRadius;
    

    [Range(0, 100)]
    public int chanceToIgnite;

    private DestructableProp _owningProp;

    [Header("Components")]
    public ParticleSystem fireParticles;

    [Header("Debug and Gizmos")]
    public bool debug;
    public bool drawGizmos;
    public bool drawHeatRadius;

    private void Awake()
    {
        _owningProp = GetComponent<DestructableProp>();
    }

    private void Start()
    {
        if (onFire) { CatchFire(); }
    }

    public void CatchFire()
    {
        onFire = true;
        fireParticles.Play();

        //Debug.Log("Catching fire...");

        Burn();
    }

    public async void Burn()
    {
        while (onFire && !_owningProp.IsDestroyed())
        {
            //Run all fire actions only once per tick
            //to save performance
            await Task.Delay(GameManager.Instance.fireTickRate);

            if (_owningProp.GetHealth() <= 0)
            {
                _owningProp.DoDestruct();
            }

            _owningProp.TakeDamage(GameManager.Instance.fireDamagePerTick);
            //Debug.Log("Burning...");
            CheckForNearbyFlammables();
        }
    }

    public void CheckForNearbyFlammables()
    {
        if (!onFire)
        {
            Debug.Log("Not on fire, leaving CheckForNearbyFlammables().");
            return;
        }

        Collider[] inHeatRadius = Physics.OverlapSphere(transform.position, heatRadius);
        if (debug && drawHeatRadius) Gizmos.DrawWireSphere(transform.position, heatRadius);

        foreach (Collider prop in inHeatRadius)
        {
            FlammableProp flammableProp = prop.GetComponent<FlammableProp>();

            if (flammableProp != null)
            {
                //Get outta here if it's not flammable or it's already on fire
                if (flammableProp.onFire || !flammableProp.flammable) continue;

                //Debug.Log("Flammable prop " + prop.name + " found.");

                int roll = Random.Range(0, 100);
                //Debug.Log("Roll: " + roll + " | chanceToIgnite: " + flammableProp.chanceToIgnite);

                if (roll < chanceToIgnite && !flammableProp.onFire)
                {
                    //Debug.Log("Igniting Flammable Prop " + prop.name + "...");
                    flammableProp.CatchFire();
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos) { return; }

        if (drawHeatRadius)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, heatRadius);
        }
    }
}
