using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum SoundName
    {
        Music,
        Move,
        Water,
        Crocodile,
        WoodLog
    }

    [SerializeField] private Sound[] sounds;

    public static SoundManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private Sound GetSound(SoundName name)
    {
        return Array.Find(sounds, s => s.soundName == name);
    }

    public void Play(SoundName name)
    {
        Sound sound = GetSound(name);
        if(sound.audioSource == null)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.loop = sound.loop;
        }
        sound.audioSource.Play();
    }
}
