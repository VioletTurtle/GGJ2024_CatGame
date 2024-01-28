using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureHelper : MonoBehaviour
{
    private GameObject Cleaner;
    private GameObject mySound;
    [SerializeField] public AudioClip myClip;
    public void DestroyMe()
    {
        Cleaner = GameObject.FindGameObjectWithTag("Cleaner");
        mySound = Resources.Load<GameObject>("Prefabs/ghostSound");
        mySound = Instantiate(mySound, transform.position, transform.rotation);
        mySound.GetComponent<AudioSource>().clip = myClip;
        mySound.GetComponent <AudioSource>().pitch = Random.Range(0.7f, 1.3f);
        mySound.GetComponent<AudioSource>().volume = Random.Range(0.4f, 0.75f);
        mySound.GetComponent <AudioSource>().Play();
        Debug.Assert(Cleaner != null);
        Cleaner.GetComponent<Destructables>().DespawnCountDown(gameObject);
    }
}
