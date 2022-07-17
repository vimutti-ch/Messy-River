using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Record : MonoBehaviour, IDataPersistance
{
    public RecordLine recordText;
    public RectTransform puRect;
    private RectTransform prRect;
    public float x;
    public float y;
    public float size;
    public float space;

    public int minute;
    public int second;
    public int millisecond;
    public string name;
    public string country;
    public Sprite flag;

    private void Start()
    {
        prRect = puRect;
        y = prRect.localPosition.y - space;

        
            string word = $"{this.minute.ToString("00")}:{this.second.ToString("00")}:{this.millisecond.ToString("00")} - {this.name}\n";
           
            var obj = Instantiate(recordText, Vector2.zero, Quaternion.identity);
                obj.transform.SetParent(this.transform);
                RectTransform rect = obj.GetComponent<RectTransform>();
                rect.localPosition = new Vector3(puRect.localPosition.x, y, 0);
                rect.localScale = new Vector3(size, size, size);

                RecordLine line = obj.GetComponent<RecordLine>();

                line.SetRecord(word, this.flag);

                y = y - space;
    }

    public void LoadData(GameData data)
    {
        this.minute = data.minute;
        this.second = data.second;
        this.millisecond = data.millisecond;
        this.name = data.name;
        this.country = data.country;
        this.flag = data.flag;

        Debug.Log("Load data");
    }

    public void SaveData(ref GameData data)
    {
        return;
    }

    /*public static void AddInfo(RecordInfo add)
    {

        //Initial Value
        RecordInfo[] temp = new RecordInfo[0];

        if(infos[0] != null || infos.Length > 1)
        {
            //Prepare temp
            temp = new RecordInfo[infos.Length];
            temp = infos;

            //Reconstruct infos
            infos = new RecordInfo[temp.Length + 1];

            for (int i = 0; i < infos.Length; i++)
            {
                if (i != infos.Length - 1) infos[i] = temp[i];
                else infos[i] = add;
            }
        }
        else
        {
            infos[0] = add;
        }
    }*/
}

/*public class RecordInfo
{
    public int minute;
    public int second;
    public int millisecond;
    public string name;
    public string country;
    public Sprite flag;
}*/
