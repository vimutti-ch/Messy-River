using System;
using UnityEngine;

public class PlayerInfo
{
    public string username;
    public string country;
    public Sprite flag;
    public string wholeTime;
    public TimeFormat aTime;

    public PlayerInfo()
    {
        username = null;
        country = null;
        flag = null;
        wholeTime = null;
        aTime = new TimeFormat();
    }

    public PlayerInfo(string name, string country, Sprite flag, string time)
    {
        username = name;
        this.country = country;
        this.flag = flag;
        wholeTime = time;
    }
}
