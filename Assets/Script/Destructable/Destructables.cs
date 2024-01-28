using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructables : MonoBehaviour
{
    GameObject target;
    public float cleanupTimer = 100f;

    public void DespawnCountDown(GameObject obj)
    {
        StartCoroutine(doit(obj));
    }

    IEnumerator doit(GameObject obj)
    {
        yield return new WaitForSeconds(cleanupTimer);
        target = GameObject.Find(obj.name + "Fragments");
        Debug.Log(target.name);
        Destroy(target);
    }
}
