using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSound : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, 5);

    }
}
