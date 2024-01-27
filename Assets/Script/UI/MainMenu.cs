using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioMixer Mixer;
    public void Play(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void MasterVolume(float v)
    {
        Mixer.SetFloat("MasterV", v);
    }
    public void SFXVolume(float v)
    {
        Mixer.SetFloat("SFXV", v);
    }
    public void BackgroundVolume(float v)
    {
        Mixer.SetFloat("BackgroundV", v);
    }

    public void SetFullScreen(float i)
    {
        if(i == 0)
        {
            Screen.fullScreen = false;
        }
        else if(i == 1)
        {
            Screen.fullScreen = true;
        }
    }
}
