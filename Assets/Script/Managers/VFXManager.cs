using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager instance;

    [SerializeField] private List<GameObject> vfxGameobjectPrefabs;
    private Dictionary<string, ParticleSystem> vfxDictionary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitializeVFXDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeVFXDictionary()
    {
        vfxDictionary = new Dictionary<string, ParticleSystem>();
        foreach (var vfx in vfxGameobjectPrefabs)
        {
            vfxDictionary[vfx.name] = vfx.GetComponent<ParticleSystem>();
        }
    }

    public void PlayVFX(string vfxName, Vector3 position, bool playOnce = false)
    {
        if (vfxDictionary.TryGetValue(vfxName, out ParticleSystem vfx))
        {
            ParticleSystem instance = Instantiate(vfx, position, Quaternion.identity);
            instance.Play();

            if (playOnce)
            {
                StartCoroutine(DestroyAfterPlay(instance));
            }
        }
        else
        {
            Debug.LogWarning($"VFX with name {vfxName} not found!");
        }
    }

    private IEnumerator DestroyAfterPlay(ParticleSystem vfx)
    {
        yield return new WaitForSeconds(vfx.main.duration);
        Destroy(vfx.gameObject);
    }
}