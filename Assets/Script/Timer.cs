using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour, IDataPersistance
{
    private float timer;
    private Text timetext;
    private int minute;
    public InputField name;
    public Dropdown country;

    public bool count = false;


    // Start is called before the first frame update
    void Start()
    {
        timetext = this.GetComponent<Text>();//Assign text component of itself to time text
    }

    // Update is called once per frame
    void Update()
    {
        if(count) timer = timer + Time.deltaTime;
        if(timer > 60)
        {
            minute += 1;
            timer = 0;
        }
        timetext.text = $"{minute.ToString("00")}:{timer.ToString("00.00").Substring(0, 2)}:{timer.ToString("00.00").Substring(3, 2)}";
    }

    public void Reset()
    {
        timer = 0;
    }

    /*public RecordInfo SetRecord()
    {
        RecordInfo record = new RecordInfo();
        record.minute = minute;
        record.second = Convert.ToInt32(timer);
        record.millisecond = Convert.ToInt32(timer.ToString("00.00").Substring(3, 2));
        record.name = name.text;
        record.country = country.options[country.value].text;
        record.flag = country.options[country.value].image;

        return record;
    }*/

    public void LoadData(GameData data)
    {
        return;
    }

    public void SaveData(ref GameData data)
    {
        data.minute = minute;
        data.second = Convert.ToInt32(timer);
        data.millisecond = Convert.ToInt32(timer.ToString("00.00").Substring(3, 2));
        data.name = name.text;
        data.country = country.options[country.value].text;
        data.flag = country.options[country.value].image;
    }
    public void SetStatus(bool state)
    {
        count = state;
    }
}
