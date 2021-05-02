using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HandleHighScores : MonoBehaviour
{
    public GameObject ScoresHolder;
    public GameObject ScorePrefab;

    void Start()
    {
        StartCoroutine(_getScores());
    }
    IEnumerator _getScores()
    {
        foreach (Transform t in ScoresHolder.transform) // Clear previous data
            Destroy(t);

        UnityWebRequest request = UnityWebRequest.Get("https://paintjam2021.azurewebsites.net/getscores");
        request.SetRequestHeader("content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");
        //request.SetRequestHeader("api-version", "0.1");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("We got the results !");

            var json = request.downloadHandler.text;
            var highScores = JsonConvert.DeserializeObject<HighScoresDTO>(json);
            foreach (var score in highScores.Scores)
            {
                var scoreGO = GameObject.Instantiate(ScorePrefab, ScoresHolder.transform);
                var displayScore = scoreGO.GetComponent<DisplayScore>();
                displayScore.SetText(score.Name, score.Score);
            }
        }
    }
}
