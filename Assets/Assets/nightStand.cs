using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nightStand : MonoBehaviour
{
    private bool isMoving = false;
    public Transform nightEnd;

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, nightEnd.position, Time.deltaTime * 2f);

            if (Vector3.Distance(transform.position, nightEnd.position) < 0.1f)
                isMoving = false;
        }
    }

    public void setMove()
    {
        isMoving = true;
    }
}
