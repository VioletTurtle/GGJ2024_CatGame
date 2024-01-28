using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndColliders : MonoBehaviour
{
    [SerializeField] EndGame endGame;
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == 8)
        {
            endGame.isEndGame = true;
        }
    }
}
