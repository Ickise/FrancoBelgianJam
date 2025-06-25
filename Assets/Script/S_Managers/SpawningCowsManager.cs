using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawningCowsManager : MonoBehaviour
{
    [SerializeField] private List<CowsArea> spawnCowsAreas;
    
    [SerializeField] private List<Transform> poolCows;
    [SerializeField] private GameObject cowPrefab;
    [SerializeField] private List<GameObject> specialCows;
    
    private void Start()
    {
        PoolCows();
    }

    private void PoolCows()
    {
        foreach (var area in spawnCowsAreas)
        {
            var number = Random.Range((int)area.numberOfCows.x, (int)area.numberOfCows.y + 1);
            
            for (int i = 0; i < number; i++)
            {
                if (poolCows.Count > 0)
                {
                    var cowToSet = poolCows[0];
                    cowToSet.position = GetRandomPointInsideCollider(area.spawnArea, false);
                    cowToSet.rotation = Quaternion.Euler(GetRandomRotation());
                
                    cowToSet.parent = transform;
                
                    poolCows.RemoveAt(0);
                    cowToSet.gameObject.SetActive(true);
                }
                else 
                {
                    var cowClone = Instantiate(
                        cowPrefab,
                        GetRandomPointInsideCollider(area.spawnArea, false),
                        Quaternion.identity);
                    
                    cowClone.transform.parent = transform;
                }
            }

            if (area.numberOfSpecialCows.Length > 0)
            {
                for (int i = 0; i < area.numberOfSpecialCows.Length; i++)
                {
                    for (int j = 0; j < area.numberOfSpecialCows[i]; j++)
                    {
                        var specialCowClone = Instantiate(
                            specialCows[i],
                            GetRandomPointInsideCollider(area.spawnArea, true),
                            Quaternion.identity);

                        specialCowClone.transform.parent = transform;
                    }
                }
            }
        }
    }
    
    private Vector3 GetRandomPointInsideCollider(BoxCollider boxCollider, bool closeToCenter)
    {
        Vector3 bounds = boxCollider.size / 2f;

        if (closeToCenter)
        {
            bounds = bounds / 2f;
        }

        Vector3 point = new Vector3(
            Random.Range(-bounds.x, bounds.x),
            0,
            Random.Range(-bounds.z, bounds.z)
        ) + boxCollider.center;

        return boxCollider.transform.TransformPoint(point);
    }
    
    private Vector3 GetRandomRotation()
    {
        return new Vector3(0,Random.Range(0f,360f),0);
    }

}

[Serializable]
public class CowsArea
{
    public BoxCollider spawnArea;
    public Vector2 numberOfCows = new(6,8);
    public int[] numberOfSpecialCows;
}


