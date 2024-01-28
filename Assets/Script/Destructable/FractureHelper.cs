using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureHelper : MonoBehaviour
{
    private GameObject Cleaner;
    public void DestroyMe()
    {
        Cleaner = GameObject.FindGameObjectWithTag("Cleaner");
        Debug.Assert(Cleaner != null);
        Cleaner.GetComponent<Destructables>().DespawnCountDown(gameObject);
    }
}
