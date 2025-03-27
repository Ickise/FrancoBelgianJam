using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "ScriptableObjects/Sound/SoundData", order = 1)]
public class SoundData : ScriptableObject
{
 //   [SerializeField] private float volume = 1f;

    [Tooltip("À mettre faible si on veut un pitch convenable sinon, mettre 0 si on ne veut pas de pitch")]
    [SerializeField]
    private float pitchVariation = 0.2f;

    [SerializeField] private AudioMixerGroup audioMixerGroup;

    [SerializeField] private AudioClip audioToPlay;

 //   public float Volume => volume;
    public AudioMixerGroup AudioMixerGroup => audioMixerGroup;
    public AudioClip AudioToPlay => audioToPlay;

    // Cette méthode permet de renvoyer un pitch aléatoire basé sur la Range défini.
    public float GetPitch()
    {
        return 1 + Random.Range(-pitchVariation, pitchVariation);
    }
}