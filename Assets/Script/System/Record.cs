using System;
using UnityEngine;

public class Record : MonoBehaviour, ILoad
{
    #region - Variable Declaration (การประกาศตัวแปร) -

    [Header("Attributes")]
    [HideInInspector] public float x = 350f;
    [HideInInspector] public float y = 105f;
    public float size = 0.8f;

    [HideInInspector] public float space = 30f;

    public Transform local;
    public Transform global;
    
    [Range(1, 5)] public int recordLimiter = 1;

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
    private string[] _wholeTime;
    private string[] _displayTime;
    
    public int[] Minute => _minute;
    public int[] Second => _second;
    public int[] Millisecond => _millisecond;
    public string[] Name => _name;
    public string[] Country => _country;
    public Sprite[] Flag => _flag;
    public string[] WholeTime => _wholeTime;
    public string[] DisplayTime => _displayTime;

    private int[] _minuteGlobal;
    private int[] _secondGlobal;
    private int[] _millisecondGlobal;
    private string[] _nameGlobal;
    private string[] _countryGlobal;
    private Sprite[] _flagGlobal;
    private string[] _wholeTimeGlobal;
    private string[] _displayTimeGlobal;
    
    public int[] MinuteGlobal => _minuteGlobal;
    public int[] SecondGlobal => _secondGlobal;
    public int[] MillisecondGlobal => _millisecondGlobal;
    public string[] NameGlobal => _nameGlobal;
    public string[] CountryGlobal => _countryGlobal;
    public Sprite[] FlagGlobal => _flagGlobal;
    public string[] WholeTimeGlobal => _wholeTimeGlobal;
    public string[] DisplayTimeGlobal => _displayTimeGlobal;
    
    public static Record Instance;
    
    #endregion

    private GameData[] datas;
    
    #region - Unity's Method (คำสั่งของ Unity เอง) -

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

    #endregion

    #region - Custom Method (คำสั่งที่เขียนขึ้นมาเอง) -

    public void LoadData(GameData[] data)
    {
        datas = data;
        
        if(data.Length <= 0) return;
        
        int dataQuantity = data.Length;
        Debug.Log("Data Quantity: " + dataQuantity);

        _minute = new int[dataQuantity];
        _second = new int[dataQuantity];
        _millisecond = new int[dataQuantity];
        _name = new string[dataQuantity];
        _country = new string[dataQuantity];
        _flag = new Sprite[dataQuantity];
        _wholeTime = new string[dataQuantity];
        _displayTime = new string[dataQuantity];

        for (int i = 0; i < dataQuantity; i++)
        {
            Debug.Log(i);
            if (data[i].time < 0) return;

            _wholeTime[i] = data[i].time.ToString();

            TimeFormatter(_wholeTime[i], out _minute[i], out _second[i], out _millisecond[i]);
            _displayTime[i] = TimeDisplayFormatter(_minute[i], _second[i], _millisecond[i]);

            this._name[i] = data[i].name.Substring(3);
            this._country[i] = data[i].name.Substring(0, 3).ToUpper();
            this._flag[i] = FindFlag(_country[i]);

            Debug.Log($"Load Data #{i}: {_name[i]} - {_wholeTime[i]}");
        }
        
        CreateLocalLeaderboard();
    }

    public void LoadDataGlobal(GameData[] data)
    {
        datas = data;
        
        if(data.Length <= 0) return;
        
        int dataQuantity = data.Length;
        Debug.Log("Data Quantity: " + dataQuantity);

        CreateGlobalRecord(dataQuantity);

        for (int i = 0; i < dataQuantity; i++)
        {
            Debug.Log(i);
            if (data[i].time < 0) return;

            GlobalRecordSetter(i, data[i].name, data[i].time);
            // _wholeTimeGlobal[i] = data[i].time.ToString();
            //
            // TimeFormatter(_wholeTimeGlobal[i], out _minuteGlobal[i], out _secondGlobal[i], out _millisecondGlobal[i]);
            // _displayTimeGlobal[i] = TimeDisplayFormatter(_minuteGlobal[i], _secondGlobal[i], _millisecondGlobal[i]);
            //
            // this._nameGlobal[i] = data[i].name.Substring(3);
            // this._countryGlobal[i] = data[i].name.Substring(0, 3).ToUpper();
            // this._flagGlobal[i] = FindFlag(_country[i]);

            Debug.Log($"Load Data #{i}: {_nameGlobal[i]} - {_wholeTimeGlobal[i]}");
        }
        
        CreateGlobalLeaderboard();
    }

    public void CreateGlobalRecord(int dataQuantity)
    {
        _minuteGlobal = new int[dataQuantity];
        _secondGlobal = new int[dataQuantity];
        _millisecondGlobal = new int[dataQuantity];
        _nameGlobal = new string[dataQuantity];
        _countryGlobal = new string[dataQuantity];
        _flagGlobal = new Sprite[dataQuantity];
        _wholeTimeGlobal = new string[dataQuantity];
        _displayTimeGlobal = new string[dataQuantity];
    }
    
    public void GlobalRecordSetter(int index, string name, int time)
    {
        _wholeTimeGlobal[index] = time.ToString();

            TimeFormatter(_wholeTimeGlobal[index], out _minuteGlobal[index], out _secondGlobal[index], out _millisecondGlobal[index]);
            _displayTimeGlobal[index] = TimeDisplayFormatter(_minuteGlobal[index], _secondGlobal[index], _millisecondGlobal[index]);

            this._nameGlobal[index] = name.Substring(3);
            this._countryGlobal[index] = name.Substring(0, 3).ToUpper();
            this._flagGlobal[index] = FindFlag(_countryGlobal[index]);

        Debug.Log($"Create Global Data #{index}: {_nameGlobal[index]} - {_wholeTimeGlobal[index]}");
    }

    public void TimeFormatter(string time, out int minute, out int second, out int millisecond)
    {
        minute = 0;
        second = 0;
        millisecond = 0;
        
        switch (time.Length)
        {
            case 6:
                 minute = Convert.ToInt32(time.Substring(0, 2));
                 second = Convert.ToInt32(time.Substring(2, 2));
                 millisecond = Convert.ToInt32(time.Substring(4, 2));
                break;
            case 5:
                 minute = Convert.ToInt32(time.Substring(0, 1));
                 second = Convert.ToInt32(time.Substring(1, 2));
                 millisecond = Convert.ToInt32(time.Substring(3, 2));
                break;
            case 4:
                 second = Convert.ToInt32(time.Substring(0, 2));
                 millisecond = Convert.ToInt32(time.Substring(2, 2));
                break;
            case 3:
                 second = Convert.ToInt32(time.Substring(0, 1));
                 millisecond = Convert.ToInt32(time.Substring(1, 2));
                break;
            case 2:
                 millisecond = Convert.ToInt32(time.Substring(0, 2));
                break;
            case 1:
                 millisecond = Convert.ToInt32(time.Substring(0, 1));
                break;
        }
    }

    private void CreateLocalLeaderboard()
    {
        Debug.Log("Start Create Leaderboard");
        
        prRect = puRect;
        y = prRect.localPosition.y - space;

        int actualLimit = Mathf.Min(recordLimiter, _name.Length);
        
        string[] timeFormatted = new string[actualLimit];

        Debug.LogWarning($"Limiter is {actualLimit}");
        
        Debug.Log("Before loop");
        for (int i = 0; i < actualLimit; i++)
        {
            Debug.LogWarning("Created Leaderboard");
            
            Debug.Log("Enter Loop");
            if (_name[i] == null)
            {
                Debug.Log("Return");
                return;
            }
            
            timeFormatted[i] =
                $"{this._minute[i].ToString("00")}:{this._second[i].ToString("00")}:{this._millisecond[i].ToString("00")}";

            var obj = Instantiate(recordText, Vector2.zero, Quaternion.identity);
            obj.transform.SetParent(this.transform);
            obj.transform.SetSiblingIndex(local.GetSiblingIndex() + 1);
            
            RectTransform rect = obj.GetComponent<RectTransform>();
            //rect.localPosition = new Vector3(puRect.localPosition.x, y, 0);
            rect.localScale = new Vector3(size, size, size);

            RecordLine line = obj.GetComponent<RecordLine>();

            line.SetRecord(timeFormatted[i], this._flag[i],  this._name[i]);

            //y = y - space;
            
            Debug.Log("Leaderboard Created");
        }
        
        Debug.Log("Stop Create Leaderboard");
    }
    
    public void CreateGlobalLeaderboard()
    {
        Debug.Log("Start Create Leaderboard");
        
        prRect = puRect;
        y = prRect.localPosition.y - space;

        int actualLimit = Mathf.Min(recordLimiter, _nameGlobal.Length);
        
        string[] timeFormatted = new string[actualLimit];
        
        Debug.LogWarning($"Limiter is {actualLimit}");
        
        Debug.Log("Before loop");
        for (int i = 0; i < actualLimit; i++)
        {
            Debug.LogWarning("Created Leaderboard");
            
            Debug.Log("Enter Loop");
            if (_nameGlobal[i] == null)
            {
                Debug.Log("Return");
                return;
            }
            
            timeFormatted[i] =
                $"{this._minuteGlobal[i].ToString("00")}:{this._secondGlobal[i].ToString("00")}:{this._millisecondGlobal[i].ToString("00")}";

            var obj = Instantiate(recordText, Vector2.zero, Quaternion.identity);
            obj.transform.SetParent(this.transform);
            obj.transform.SetSiblingIndex(global.GetSiblingIndex() + 1);
            
            RectTransform rect = obj.GetComponent<RectTransform>();
            //rect.localPosition = new Vector3(puRect.localPosition.x, y, 0);
            rect.localScale = new Vector3(size, size, size);

            RecordLine line = obj.GetComponent<RecordLine>();

            line.SetRecord(timeFormatted[i], this._flagGlobal[i], this._nameGlobal[i]);

            //y = y - space;
            
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

    public static string TimeDisplayFormatter(int minute, int second, int milliseccond)
    {
        return $"{minute.ToString("00")}:{second.ToString("00")}:{milliseccond.ToString("00")}";
    }

    #endregion
}