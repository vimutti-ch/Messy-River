using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLeaderboard : MonoBehaviour
{
    public bool isGlobal = false;

    public string localTitle;
    public string globalTitle;

    public DataToLeaderboard leaderboard;
    
    public void SwitchBoard()
    {
        isGlobal = !isGlobal;
    }
}
