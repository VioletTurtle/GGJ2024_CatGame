using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PawseMenu : MonoBehaviour
{
    public AudioMixer Mixer;
    public GameObject PauseMenu;
    private bool isPaused;
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
        if (i == 0)
        {
            Screen.fullScreen = false;
        }
        else if (i == 1)
        {
            Screen.fullScreen = true;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if(isPaused == true)
            {
                PauseGame();
                PauseMenu.SetActive(true);
            }
            else
            {
                ResumeGame();
                PauseMenu.SetActive(false);
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void Start()
    {
        isPaused = PauseMenu.activeInHierarchy;
    }
}
