using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField, Header("References")] private AudioSource mainAudioSource;
    [SerializeField] private AudioSource playOnceAudioSource;

    [SerializeField] private List<SoundData> soundDataList;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Cette fonction permet de jouer un son à partir de l'index dans la liste des sons.
    public void PlaySound(int index)
    {
        if (index < 0 || index >= soundDataList.Count)
        {
            Debug.LogWarning("Invalid SoundData index");
            return;
        }

        SoundData data = soundDataList[index];
        playOnceAudioSource.PlayOneShot(SetAudioParameters(data, playOnceAudioSource).AudioToPlay);
    }

    // Cette fonction permet de jouer un son aléatoire parmi ceux disponibles dans la liste. 
    public void PlayRandomSound()
    {
        if (soundDataList.Count == 0)
        {
            Debug.LogWarning("SoundData list is empty");
            return;
        }

        int randomIndex = Random.Range(0, soundDataList.Count);
        PlaySound(randomIndex);
    }

    // Cette fonction permet de jouer une musique en utilisant un index spécifique, avec la possibilité de boucler.
    public void PlayMusic(int index, bool isLoop = false)
    {
        if (index < 0 || index >= soundDataList.Count)
        {
            Debug.LogWarning("Invalid SoundData index");
            return;
        }

        SoundData data = soundDataList[index];
        if (mainAudioSource.isPlaying && mainAudioSource.clip == data.AudioToPlay)
            return;

        mainAudioSource.clip = SetAudioParameters(data, mainAudioSource).AudioToPlay;
        mainAudioSource.Play();

        if (isLoop)
        {
            mainAudioSource.loop = true;
        }
    }

    // Cette fonction permet de configurer les paramètres de l'AudioSource (volume, pitch, etc.) en fonction des données du son.
    private SoundData SetAudioParameters(SoundData soundData, AudioSource audioSource)
    {
        //audioSource.volume = soundData.Volume;
        audioSource.pitch = soundData.GetPitch();
        audioSource.outputAudioMixerGroup = soundData.AudioMixerGroup;
        return soundData;
    }
}