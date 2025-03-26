using UnityEngine;

public class MosquitoGun : ToolBase
{
    [SerializeField, Header("References")] private GameObject soundObjectPrefab;
    [SerializeField] private GameObject powerfulSoundPrefab;
    [SerializeField, Header("Settings")] private float holdThreshold = 3f;
    [SerializeField] private float littleDisturbAreaRange = 60f;
    [SerializeField] private float littleDisturbAreDistance = 3f;
    [SerializeField] private float bigDisturbAreaRange = 90f;
    [SerializeField] private float bigDisturbAreDistance = 5f;
    [SerializeField] private float bigDisturbAreaTimeLife = 2f;
    [SerializeField] private float recoverTime = .5f;

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

            Destroy(spawnedSound, bigDisturbAreaTimeLife);

            holdTime = 0f;
        }
    }
}