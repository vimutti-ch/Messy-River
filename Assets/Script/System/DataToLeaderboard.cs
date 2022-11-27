using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataToLeaderboard : MonoBehaviour
{
    public static DataToLeaderboard Instance;

    public TMP_Text[] leaderText;
    public TMP_Text[] globalLeaderText;

    public TMP_Text playerLocal;
    public TMP_Text playerGlobal;

    private Timer timer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(Instance);
        }
    }

    private void Start()
    {
        timer = Timer.Instance;
    }

    public void UpdateLocalLeaderBoard()
    {
        for (int i = 0; i < Record.Instance.Name.Length; i++)
        {
            if(Record.Instance.Name[i] == null) return;
            
            leaderText[i].text = $"Rank {i+1} {Record.Instance.DisplayTime[i]} - {Record.Instance.Name[i]}";
        }

        string playerStatistics = $"Your time: {timer.PlayerTime} {timer.PlayerName}";
        
        playerLocal.text = playerStatistics;
    }
    
    public void UpdateGlobalLeaderBoard()
    {
        for (int i = 0; i < Record.Instance.NameGlobal.Length; i++)
        {
            if(Record.Instance.NameGlobal[i] == null) return;
            
            globalLeaderText[i].text = $"Rank {i+1} {Record.Instance.DisplayTimeGlobal[i]} - {Record.Instance.NameGlobal[i]}";
        }
        
        string playerStatistics = $"Your time: {timer.PlayerTime} {timer.PlayerName}";
        
        playerLocal.text = playerStatistics;
    }
}
