using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int minute;
    public int second;
    public int millisecond;
    public string name;
    public string country;
    public Sprite flag;

 
    public GameData()
    {
        this.minute = 0;
        this.second = 0;
        this.millisecond = 0;
        this.name = null;
        this.country = null;
        this.flag = null;
    }

}
