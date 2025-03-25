using UnityEngine;

public class SoundMaker : ToolBase
{
    [SerializeField, Header("References")] private GameObject soundObjectPrefab;
    [SerializeField] private GameObject powerfulSoundPrefab;
    [SerializeField, Header("Settings")] private float holdThreshold = 2f;
    [SerializeField] private float timeToDestroy = 5f;

    private float holdTime = 0f;

    private bool isHolding = false;

    private void Update()
    {
        if (isHolding)
        {
            holdTime += Time.deltaTime;
        }
    }

    public override void UseTool(bool isHeld)
    {
        if (isHeld)
        {
            isHolding = true;
        }
        else
        {
            isHolding = false;

            GameObject soundToSpawn = holdTime >= holdThreshold ? powerfulSoundPrefab : soundObjectPrefab;

            GameObject spawnedSound = Instantiate(soundToSpawn, transform.position, Quaternion.identity);

            Destroy(spawnedSound, timeToDestroy);

            holdTime = 0f;
        }
    }
}