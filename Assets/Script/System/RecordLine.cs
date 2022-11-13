using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RecordLine : MonoBehaviour
{
    [FormerlySerializedAs("name")] public TMP_Text displayTime;
    public Image flag;
    public TMP_Text displayName;

    public void SetRecord(string time, Sprite picture, string name)
    {
        displayTime.text = time;
        flag.sprite = picture;
        displayName.text = name;
    }
}
