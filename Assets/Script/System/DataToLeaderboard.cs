using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataToLeaderboard : MonoBehaviour
{
    public static DataToLeaderboard Instance;

    public TMP_Text[] leaderText;

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
    
    public void UpdateLeaderBoard()
    {
        for (int i = 0; i < Record.Instance.Name.Length; i++)
        {
            if(Record.Instance.Name[i] == null) return;
            
            leaderText[i].text = $"Rank {i+1} {Record.Instance.WholeTime[i]} {Record.Instance.Name[i]}";
        }
    }
}
