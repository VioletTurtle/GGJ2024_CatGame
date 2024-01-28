using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public void Caught(GameObject Granny, GameObject Cat)
    {
        float timer = 5f;
        while (timer > 0)
        {
            Freeze(Cat);
            Freeze(Granny);
            timer -= 1;
        }
        //fadeToBlack();
        //resetCat(Cat);
    }

    public void Freeze(GameObject ob)
    {
        // Freeze the Y position by setting it to its current value
        Vector3 currentPosition = ob.transform.position;
        currentPosition.y = Mathf.Clamp(currentPosition.y, currentPosition.y, currentPosition.y);
        transform.position = currentPosition;
    }

    public void resetCat(GameObject cat)
    {
        
    }

    public void fadeToBlack()
    {

    }
}
