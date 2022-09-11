using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Asset/Flag")]
public class Flag : ScriptableObject
{
    public countryInfo[] country;
}


[Serializable]
public class countryInfo
{
    public string abbreviate;
    public Sprite flag;
}
