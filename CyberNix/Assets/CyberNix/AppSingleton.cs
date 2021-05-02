using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AppSingleton : Singleton<AppSingleton>
{
    public LevelService LevelService;

    public HandleHighScores HighScores;
    public TMPro.TMP_InputField NameInputField;
    public GameObject ButtonSend;
    public string MenuSceneName;

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        SceneManager.LoadScene(MenuSceneName);
    }

    public void SendResult()
    {
        StartCoroutine(_sendScore(LevelService.Score));
    }

    IEnumerator _sendScore(int score)
    {
        if (string.IsNullOrWhiteSpace(NameInputField.text))
            yield break;
        ButtonSend.SetActive(false);
        UnityWebRequest request = UnityWebRequest.Post("https://paintjam2021.azurewebsites.net/setscore", "");
        request.SetRequestHeader("content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("name", NameInputField.text);
        request.SetRequestHeader("score", $"{score}");
        request.SetRequestHeader("pwd", "trollolol");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            HighScores.RefreshScores();
        }
    }

    void Start()
    {
        
    }
}
