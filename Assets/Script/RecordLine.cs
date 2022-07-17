using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordLine : MonoBehaviour
{
    public Text name;
    public Image flag;

    public void SetRecord(string word, Sprite picture)
    {
        name.text = word;
        flag.sprite = picture;
    }
}
