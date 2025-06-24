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
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        foreach (Sound sound in sounds)
        {
            if (sound.source != null)
                sound.source.loop = sound.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null|| s.source==null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        if(debug)
            Debug.Log("Playing "+name);

        if (s.humanize)
        {
            s.source.volume = Random.Range(globalVolume - (globalVolume * 0.5f), globalVolume);
            s.source.pitch = Random.Range(0.95f, 1.05f);
        }
        else
        {
            s.source.volume = (s.source.volume * globalVolume);
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null|| s.source==null)
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
}