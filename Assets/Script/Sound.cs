using System;
using UnityEngine;

[Serializable]
public class Sound
{
    public SoundManager.SoundName soundName;

    public AudioClip clip; //sound file

    [Range(0f, 1f)]public float volume;  //volume change

    public bool loop; //repeat

    [HideInInspector] public AudioSource audioSource;
}
