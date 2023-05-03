using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class PlayfabScoreSystem : MonoBehaviour
{
    private static PlayfabScoreSystem instance;

    public static PlayfabScoreSystem Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        instance = this;
        GetScoreFromPlayfabSever();
    }

    private void GetScoreFromPlayfabSever()
    {
        GetPlayerStatisticsRequest request = new GetPlayerStatisticsRequest();
        PlayFabClientAPI.GetPlayerStatistics(request, OnGetStats, OnGetStatsError);
    }

    private void OnGetStats(GetPlayerStatisticsResult result)
    {
        foreach (var stat in result.Statistics)
        {
            ScoreManager.Instance.bestScore = stat.Value;
        }

        Debug.Log("Get Best Score successfully!");
    }

    private void OnGetStatsError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    public void UpdateScoreToPlayfab()
    {
        if (ScoreManager.Instance.score <= ScoreManager.Instance.bestScore) return;

        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "UpdatePlayerScore",
            FunctionParameter = new
            {
                score = ScoreManager.Instance.score
            },
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnCloudUpdate, OnCloudUpdateError);
    }

    private void OnCloudUpdateError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    private void OnCloudUpdate(ExecuteCloudScriptResult result)
    {
        GetScoreFromPlayfabSever();
    }
}
