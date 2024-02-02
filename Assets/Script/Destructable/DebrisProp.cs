using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class DebrisProp : MonoBehaviour
{
    [Header("Core Stats")]
    public bool decayOnSpawn;
    public int decayTimer;
    [Range(0, 100)]
    public float decayChance;

    [Header("Debug and Gizmos")]
    public bool debug;
    public bool drawGizmos;
    public bool drawHeatRadius;

    private int roll;
    private float decaySpeed;

    private Collider collider;

    private void Awake()
    {
        roll = Random.Range(0, 100);
        collider = GetComponent<Collider>();

        if (decaySpeed == 0) decaySpeed = 5;
    }

    private void Start()
    {
        //transform.localScale = Vector3.one;
        if (decayOnSpawn) Decay();
    }

    /// <summary>
    /// Tells the debris to shrink and despawn. Decaytimer must be in milleseconds. DecayChance must be a percentage as an integer
    /// </summary>
    /// <param name="decayTimer"></param>
    /// <param name="decayChance"></param>
    public async void Decay(int decayTimer, int decayChance)
    {
        transform.SetParent(null);

        //Debug.Log("RandNum: " +  roll + " | decayChance: " + decayChance);
        if (roll < decayChance)
        {
            Light2D debrisLight = GetComponent<Light2D>();

            await Task.Delay(decayTimer * 1000); //InBounce

            if (debrisLight != null)
            {
                DOTween.To(() => debrisLight.intensity, x => debrisLight.intensity = x, 0, 1);
            }

            transform.DOScale(Vector2.zero, decaySpeed).SetEase(Ease.OutCirc).OnComplete(() =>
            {
                Despawn();
            });
        }
        else
        {
            GameManager.Instance.SendToGarbage(gameObject);
        }
    }

    public async void Decay()
    {
        transform.SetParent(null);
        //Debug.Log("RandNum: " +  roll + " | decayChance: " + decayChance);
        if (roll < decayChance)
        {
            Light2D debrisLight = GetComponent<Light2D>();

            await Task.Delay(decayTimer * 1000); //InBounce

            if (debrisLight != null)
            {
                DOTween.To(() => debrisLight.intensity, x => debrisLight.intensity = x, 0, 1);
            }

            transform.DOScale(Vector2.zero, decaySpeed).SetEase(Ease.OutCirc).OnComplete(() =>
            {
                Despawn();
            });
        }
        else
        {
            GameManager.Instance.SendToGarbage(gameObject);
        }
    }

    public async void Despawn()
    {
        //Debug.Log("Despawning...");
        GameObject.Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos) { return; }

        if (drawHeatRadius)
        {
            Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(transform.position, heatRadius);
        }
    }
}
