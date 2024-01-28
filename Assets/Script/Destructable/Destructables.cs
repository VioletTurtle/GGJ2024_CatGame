using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructables : MonoBehaviour
{
    GameObject target;

    public void DespawnCountDown(GameObject obj)
    {
        StartCoroutine(doit(obj));
    }

    IEnumerator doit(GameObject obj)
    {
        yield return new WaitForSeconds(100f);
        target = GameObject.Find(obj.name + "Fragments");
        Debug.Log(target.name);
        Destroy(target, 5);
    }
}
