using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandleMenu : MonoBehaviour
{
    public string StartSceneName;
    public GameObject MainMenuHolder;
    public GameObject CreditsHolder;
    public GameObject HighScoresHolder;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void PressStart()
    {
        MainMenuHolder.SetActive(false);
        SceneManager.LoadSceneAsync(StartSceneName, LoadSceneMode.Single);
    }

    public void PressCredits()
    {
        MainMenuHolder.SetActive(false);
        CreditsHolder.SetActive(true);
    }
    public void PressPreviousToMenu()
    {
        MainMenuHolder.SetActive(true);
        HighScoresHolder.SetActive(false);
        CreditsHolder.SetActive(false);
    }
    public void PressScores()
    {
        MainMenuHolder.SetActive(false);
        HighScoresHolder.SetActive(true);
    }
}
