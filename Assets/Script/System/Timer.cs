using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Timer : MonoBehaviour, ISave
{
    public string TimeRecord => timerText;
    
    [Header("Object Assign")]
    [FormerlySerializedAs("name")] public TMP_InputField username;
    public TMP_Dropdown country;
    public TMP_Text resultTimer;
    public TMP_Text dateText;
    public PlayfabManager playfabManager;

    [Header("Status")]
    public bool saveUpdate;
    public bool recordUpdated;
    public string timerText;
    
    private bool _count;
    
    private float _timer;
    private TMP_Text _timeText;
    private int _minute;

    void Start()
    {
        _timeText = GetComponent<TMP_Text>();//Assign text component of itself to time text
    }
    
    void Update()
    {
        if(_count) _timer += Time.deltaTime;
        if(_timer > 60)
        {
            _minute += 1;
            _timer = 0;
        }
        timerText =
            $"{_minute:00}:{_timer.ToString("00.00").Substring(0, 2)}:{_timer.ToString("00.00").Substring(3, 2)}";
        
        _timeText.text = timerText;
    }

    public void SetStatus(bool stage)
    {
        _count = stage;
    }

    public void ToggleClock()
    {
        _count = !_count;
    }

    public void Reset()
    {
        _timer = 0;
    }

    // public RecordInfo SetRecord()
    // {
    //     RecordInfo record = new RecordInfo();
    //     record.minute = minute;
    //     record.second = Convert.ToInt32(timer);
    //     record.millisecond = Convert.ToInt32(timer.ToString("00.00").Substring(3, 2));
    //     record.name = name.text;
    //     record.country = country.options[country.value].text;
    //     record.flag = country.options[country.value].image;
    //
    //     return record;
    // }

    public void SaveData(ref GameData data, ref SimplifyGameData simpleData)
    {
        saveUpdate = false;
        recordUpdated = false;

        // data.minute = this.minute;
        // data.second = Convert.ToInt32(timer);
        // data.millisecond = Convert.ToInt32(timer.ToString("00.00").Substring(3, 2));
        int timerToInt = (int) (_timer * 100);
        int wholeTime = Int32.Parse(_minute.ToString() + timerToInt.ToString()); // 00 00 00
        
        Debug.Log(wholeTime);

        int recordLimiter = data.time.Length;

        int tempTime = wholeTime;
        string tempName = country.options[country.value].text+username.text;
        
        for (int i = 0; i < recordLimiter; i++)
        {
            Debug.Log(data.time[i] + data.name[i]);
            if (tempTime < data.time[i] || data.time[i] == 0)
            {
                (data.time[i], tempTime) = (tempTime, data.time[i]);
                (data.name[i], tempName) = (tempName, data.name[i]);

                saveUpdate = true;

                if (!recordUpdated)
                {
                    recordUpdated = true;

                    simpleData.time = wholeTime;
                    simpleData.name = country.options[country.value].text+username.text;
                    
                    Debug.Log($"Simplify Data Update : {simpleData.time} by {simpleData.name}");
                }
                
                Debug.Log("Saved Data");
            }
        }
        
        // data.time = wholeTime;
        // data.name = country.options[country.value].text+name.text; //THAName - USAPhufah
        //data.country = country.options[country.value].text;
        //data.flag = country.options[country.value].image;
        
        if(!saveUpdate) Debug.Log("No data update.");
    }

    public void PassResult()
    {
        resultTimer.text = timerText;

        string date = DateTime.Now.Year.ToString();
        switch (DateTime.Now.Month)
        {
            case 1:
                date += "Jan";
                break;
            case 2:
                date += "Feb";
                break;
            case 3:
                date += "Mar";
                break;
            case 4:
                date += "Apr";
                break;
            case 5:
                date += "May";
                break;
            case 6:
                date += "Jun";
                break;
            case 7:
                date += "Jul";
                break;
            case 8:
                date += "Aug";
                break;
            case 9:
                date += "Sep";
                break;
            case 10:
                date += "Oct";
                break;
            case 11:
                date += "Nov";
                break;
            case 12:
                date += "Dec";
                break;
        }

        dateText.text = date;
    }
}
