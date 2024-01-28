using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Threading.Tasks;

public class FootstepPlayer : MonoBehaviour
{
    public bool stepping;

    //Interval at which to play the sound effect, in milliseconds;
    public int stepInterval;

    public AudioClip[] sounds;
    public AudioSource audioSource;

    private bool playingFootsteps;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayFootsteps();   
    }

    public async void PlayFootsteps()
    {
        if (playingFootsteps) return;

        while (stepping) 
        {
            await Task.Delay(stepInterval);

            int soundToPlay = Random.Range(0, sounds.Length);
            float volumeToPlay = Random.Range(0.95f, 1);
            float pitchToPlay = Random.Range(0.95f, 1.05f);
            audioSource.clip = sounds[soundToPlay];
            audioSource.volume = volumeToPlay;
            audioSource.pitch = pitchToPlay;
            audioSource.Play();

            playingFootsteps = true;
        }
        
    }

    public void StopFootsteps()
    {
        playingFootsteps = false;
    }
    
}
