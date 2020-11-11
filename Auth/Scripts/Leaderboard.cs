using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    public GameObject leaderboardCanvas;
    public GameObject[] leaderboardScoreEntries;
    public GameObject[] leaderboardInputEntries;

    public static Leaderboard instance;
    void Awake()
    {
        if (instance != null && instance != this)
            gameObject.SetActive(false);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }




    public void OnLoggedIn()
    {
        leaderboardCanvas.SetActive(true);
        DisplayLeaderboard();
    }




    #region Playfab Interactions


    public void DisplayLeaderboard()
    {

        GetLeaderboardRequest getLeaderboardTimeReq = new GetLeaderboardRequest
        {
            StatisticName = "FastestTime",
            MaxResultsCount = 3
        };

        GetLeaderboardRequest getLeaderboardInputReq = new GetLeaderboardRequest
        {
            StatisticName = "FewestInputs",
            MaxResultsCount = 3
        };

        PlayFabClientAPI.GetLeaderboard(getLeaderboardTimeReq,
            result => UpdateLeaderboardTimesUI(result.Leaderboard),
            error => Debug.LogError(error.ErrorMessage));


        PlayFabClientAPI.GetLeaderboard(getLeaderboardInputReq,
            result => UpdateLeaderboardInputsUI(result.Leaderboard),
            error => Debug.LogError(error.ErrorMessage));

    }

        void UpdateLeaderboardTimesUI(List<PlayerLeaderboardEntry> leaderboard)
    {

        for (int x = 0; x < leaderboardScoreEntries.Length; x++)
        {
            leaderboardScoreEntries[x].SetActive(x < leaderboard.Count);
            if (x >= leaderboard.Count) continue;
            leaderboardScoreEntries[x].transform.Find("PlayerName").GetComponent<TextMeshProUGUI>(
            ).text = (leaderboard[x].Position + 1) + ". " + leaderboard[x].DisplayName;
            leaderboardScoreEntries[x].transform.Find("Score").GetComponent<TextMeshProUGUI>()
           .text = (-(float)leaderboard[x].StatValue * 0.001f).ToString("F2");
        }
    }

    void UpdateLeaderboardInputsUI(List<PlayerLeaderboardEntry> leaderboard)
    {

        for (int x = 0; x < leaderboardInputEntries.Length; x++)
        {
            leaderboardInputEntries[x].SetActive(x < leaderboard.Count);
            if (x >= leaderboard.Count) continue;
            leaderboardInputEntries[x].transform.Find("PlayerName").GetComponent<TextMeshProUGUI>(
            ).text = (leaderboard[x].Position + 1) + ". " + leaderboard[x].DisplayName;
            leaderboardInputEntries[x].transform.Find("Score").GetComponent<TextMeshProUGUI>()
            .text = (-(float)leaderboard[x].StatValue).ToString();
        }
    }



    public void SetLeaderboardEntry(int newScore)
    {
        ExecuteCloudScriptRequest requisition = new ExecuteCloudScriptRequest
        {
            FunctionName = "UpdateHighScore",
            FunctionParameter = new { score = newScore }
        };

        Debug.Log("Time: " + newScore);


        PlayFabClientAPI.ExecuteCloudScript(requisition,
            result => DisplayLeaderboard(),
            error => Debug.Log(error.ErrorMessage));

    }


    public void SetLeaderboardEntryInputs(int inputs)
    {
        ExecuteCloudScriptRequest requisition = new ExecuteCloudScriptRequest
        {
            FunctionName = "UpdateInputScore",
            FunctionParameter = new { score = inputs }
        };

        Debug.Log("input count: " + inputs);


        PlayFabClientAPI.ExecuteCloudScript(requisition,
            result => DisplayLeaderboard(),
            error => Debug.Log(error.ErrorMessage));

    }

    #endregion Playfab Interactions
}
