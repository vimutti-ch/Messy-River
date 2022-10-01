using System;
using UnityEngine;

public class Record : MonoBehaviour, ILoad
{
    #region - Variable Declaration (การประกาศตัวแปร) -

    [Header("Attributes")]
    public float x;
    public float y;
    public float size;

    public float space;

    public int recordLimiter;

    [Header("Object Assign")]
    //public static RecordInfo[] infos = new RecordInfo[1];
    public RecordLine recordText;

    public RectTransform puRect;
    public Flag countryInfo;
    
    private RectTransform prRect;

    private int[] _minute;
    private int[] _second;
    private int[] _millisecond;
    private string[] _name;
    private string[] _country;
    private Sprite[] _flag;

    #endregion

    #region - Unity's Method (คำสั่งของ Unity เอง) -
    
    //Nothing Here

    #endregion

    #region - Custom Method (คำสั่งที่เขียนขึ้นมาเอง) -

    public void LoadData(GameData data)
    {
        int dataQuantity = data.time.Length;

        _minute = new int[dataQuantity];
        _second = new int[dataQuantity];
        _millisecond = new int[dataQuantity];
        _name = new string[dataQuantity];
        _country = new string[dataQuantity];
        _flag = new Sprite[dataQuantity];

        for (int i = 0; i < dataQuantity; i++)
        {
            if (string.IsNullOrEmpty(data.name[i])) return;

            string time = data.time[i].ToString();

            switch (time.Length)
            {
                case 6:
                    this._minute[i] = Convert.ToInt32(time.Substring(0, 2));
                    this._second[i] = Convert.ToInt32(time.Substring(2, 2));
                    this._millisecond[i] = Convert.ToInt32(time.Substring(4, 2));
                    break;
                case 5:
                    this._minute[i] = Convert.ToInt32(time.Substring(0, 1));
                    this._second[i] = Convert.ToInt32(time.Substring(1, 2));
                    this._millisecond[i] = Convert.ToInt32(time.Substring(3, 2));
                    break;
                case 4:
                    this._second[i] = Convert.ToInt32(time.Substring(0, 2));
                    this._millisecond[i] = Convert.ToInt32(time.Substring(2, 2));
                    break;
                case 3:
                    this._second[i] = Convert.ToInt32(time.Substring(0, 1));
                    this._millisecond[i] = Convert.ToInt32(time.Substring(1, 2));
                    break;
                case 2:
                    this._millisecond[i] = Convert.ToInt32(time.Substring(0, 2));
                    break;
                case 1:
                    this._millisecond[i] = Convert.ToInt32(time.Substring(0, 1));
                    break;
            }

            this._name[i] = data.name[i].Substring(3);
            this._country[i] = data.name[i].Substring(0, 3).ToUpper();
            this._flag[i] = FindFlag(_country[i]);

            Debug.Log($"Load Data #{i}: {_name[i]} - {time[i]}");
            
            CreateLeaderboard();
        }
    }
    
    private void CreateLeaderboard()
    {
        Debug.Log("Start Create Leaderboard");
        
        prRect = puRect;
        y = prRect.localPosition.y - space;

        string[] word = new string[recordLimiter];
        
        Debug.Log("Before loop");
        for (int i = 0; i < recordLimiter; i++)
        {
            Debug.Log("Enter Loop");
            if (_name[i] == null)
            {
                Debug.Log("Return");
                return;
            }
            
            word[i] =
                $"{this._minute[i].ToString("00")}:{this._second[i].ToString("00")}:{this._millisecond[i].ToString("00")} - {this._name[i]}\n";

            var obj = Instantiate(recordText, Vector2.zero, Quaternion.identity);
            obj.transform.SetParent(this.transform);
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.localPosition = new Vector3(puRect.localPosition.x, y, 0);
            rect.localScale = new Vector3(size, size, size);

            RecordLine line = obj.GetComponent<RecordLine>();

            line.SetRecord(word[i], this._flag[i]);

            y = y - space;
            
            Debug.Log("Leaderboard Created");
        }
        
        Debug.Log("Stop Create Leaderboard");
    }

    private Sprite FindFlag(string countryAbbreviate)
    {
        foreach (countryInfo info in countryInfo.country)
        {
            if (info.abbreviate.Trim().ToUpper() == countryAbbreviate.Trim().ToUpper())
            {
                return info.flag;
            }
        }

        return null;
    }

    #endregion
}