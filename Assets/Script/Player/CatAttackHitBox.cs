using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CatAttackHitBox : MonoBehaviour
{
    public List<Collider> colliders;

    private void Awake()
    {
        foreach (Collider collider in colliders)
        {
            if (collider.enabled)
            {
                collider.enabled = false;
            }
        }
    }

    public void enableBoxes()
    {
        foreach (Collider collider in colliders)
        {
            if (!collider.enabled)
            {
                collider.enabled = true;
            }
        }
    }

    public void disableBoxes()
    {
        foreach (Collider collider in colliders)
        {
            if (collider.enabled)
            {
                collider.enabled = false;
            }
        }
    }
}
