using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;

    public float globalVolume;

    [SerializeField] bool debug;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);


        foreach (Sound sound in sounds)
        {
            if (sound.source != null)
                sound.source.loop = sound.loop;
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.source == null)
        {
            Debug.LogWarning("SFX: " + name + " not found!");
            return;
        }

        var volume = s.humanize
            ? Random.Range(globalVolume - (globalVolume * 0.5f), globalVolume)
            : (s.source.volume * globalVolume);

        var pitch = s.humanize
            ? Random.Range(0.95f, 1.05f)
            : 1f;

        s.source.pitch = pitch;

        s.source.PlayOneShot(s.clip, volume);
        
        if (debug) Debug.Log("Played SFX: " + s.clip);
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.source == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        if (debug)
            Debug.Log("Playing " + name);

        s.source.volume = s.humanize
            ? Random.Range(globalVolume - (globalVolume * 0.5f), globalVolume)
            : (s.source.volume * globalVolume);

        s.source.pitch = s.humanize
            ? Random.Range(0.95f, 1.05f)
            : 1f;

        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.source == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Stop();
    }
}

[Serializable]
public class Sound
{
    public string name;

    public bool loop;

    public bool humanize = true;

    public AudioSource source;

    public AudioClip clip;
}