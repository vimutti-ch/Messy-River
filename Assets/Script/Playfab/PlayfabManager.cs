using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
    void Awake()
    {
        Login();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful login/account create!");
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in/creating account!");
        Debug.Log(error.GenerateErrorReport());
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successful leaderboard sent");
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (var item in result.Leaderboard)
        {
            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
            //Debug.Log($"{item.Position} {item.PlayFabId} {item.StatValue}");
        }
    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated display name!");
    }

    public void SendLeaderboard(SimplifyGameData data)
    {
        Debug.Log($"Sent a score of {data.time}");
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "GlobalLeaderboard",
                    Value = data.time
                }
            }
        };

        var requestName = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = data.name
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
        PlayFabClientAPI.UpdateUserTitleDisplayName(requestName, OnDisplayNameUpdate, OnError);
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "GlobalLeaderboard",
            StartPosition = 0,
            MaxResultsCount = 5
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }
}