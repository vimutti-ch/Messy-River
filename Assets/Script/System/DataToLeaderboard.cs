using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataToLeaderboard : MonoBehaviour
{
    public static DataToLeaderboard Instance;

    public TMP_Text[] leaderText;
    public TMP_Text[] globalLeaderText;

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
    
    public void UpdateLocalLeaderBoard()
    {
        for (int i = 0; i < Record.Instance.Name.Length; i++)
        {
            if(Record.Instance.Name[i] == null) return;
            
            leaderText[i].text = $"Rank {i+1} {Record.Instance.DisplayTime[i]} {Record.Instance.Name[i]}";
        }
    }
    
    public void UpdateGlobalLeaderBoard()
    {
        for (int i = 0; i < Record.Instance.NameGlobal.Length; i++)
        {
            if(Record.Instance.NameGlobal[i] == null) return;
            
            globalLeaderText[i].text = $"Rank {i+1} {Record.Instance.DisplayTimeGlobal[i]} {Record.Instance.NameGlobal[i]}";
        }
    }
}
