using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public GameObject EndGameScreen;
    public bool isEndGame;

    private void Update()
    {
        if(isEndGame == true)
        {
            EndGameScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void restart()
    {
        SceneManager.LoadScene("House1");
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
