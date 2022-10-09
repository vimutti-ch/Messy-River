using System;
using UnityEngine;

public class TimeFormat
{
    public int minute;
    public int second;
    public int millisecond;

    public TimeFormat()
    {
        minute = 0;
        second = 0;
        millisecond = 0;
    }

    public TimeFormat(int m, int s, int mi)
    {
        minute = m;
        second = s;
        millisecond = mi;
    }
}
