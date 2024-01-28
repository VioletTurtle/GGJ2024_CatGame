using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyCatForce : MonoBehaviour
{
    public float power;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            Debug.Log("triggers");
            Vector3 dir = collision.transform.position - gameObject.transform.position;
            collision.rigidbody.AddForce(dir.normalized * power, ForceMode.Impulse);
        }
    }
}
