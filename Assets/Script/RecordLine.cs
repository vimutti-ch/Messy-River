using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RecordLine : MonoBehaviour
{
    [FormerlySerializedAs("name")] public Text displayName;
    public Image flag;

    public void SetRecord(string word, Sprite picture)
    {
        displayName.text = word;
        flag.sprite = picture;
    }
}
