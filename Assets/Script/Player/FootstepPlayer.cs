using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Threading.Tasks;

public class FootstepPlayer : MonoBehaviour
{
    public bool stepping;

    //Interval at which to play the sound effect, in milliseconds;
    public float stepInterval = 0.1f;
    public AudioClip clip;

    public AudioSource audioSource;

    public bool playingFootsteps;


    // Start is called before the first frame update

    public void PlayFootsteps()
    {
        if (!playingFootsteps)
        {
            playingFootsteps = true;
            StartCoroutine(PlayFootstep());
        }

    }

    IEnumerator PlayFootstep()
    {
        float volumeToPlay = Random.Range(0.60f, 0.75f);
        float pitchToPlay = Random.Range(0.95f, 1.05f);
        audioSource.volume = volumeToPlay;
        audioSource.pitch = pitchToPlay;
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(stepInterval);
        playingFootsteps = false;
    }
    
}
