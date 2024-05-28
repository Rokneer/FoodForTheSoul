using System;
using System.Collections.Generic;
using UnityEngine;

public class RecipeSpawner : Spawner
{
    private static RecipeSpawner _instance;
    public static RecipeSpawner Instance => _instance;

    [Header("Spawn Points")]
    [SerializeField]
    private Transform[] recipeSpawnPoints;
    private Dictionary<Transform, bool> spawnPointsDict = new();

    private bool IsSpawnFull
    {
        get
        {
            foreach (Transform spawnPoint in recipeSpawnPoints)
            {
                if (!spawnPointsDict[spawnPoint])
                {
                    return false;
                }
            }
            return true;
        }
    }
    private int randomIndex;

    private void Awake()
    {
        // Checks if there is only one instance of the script in the scene
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        foreach (Transform spawnPoint in recipeSpawnPoints)
        {
            spawnPointsDict[spawnPoint] = false;
        }
        InvokeRepeating(nameof(SpawnRecipe), firstSpawnTime, spawnTime);
    }

    protected override GameObject SpawnObject(Transform spawnPoint)
    {
        currentObjectCount++;
        return ObjectPoolManager.SpawnObject(
            objectToSpawn,
            spawnPoint.position,
            Quaternion.identity,
            PoolType.GameObjects
        );
    }

    public void SpawnRecipe(Recipe recipe)
    {
        if (canSpawn && !IsSpawnFull)
        {
            // Get a random spawn index that isn't in use
            randomIndex = RandomIndex.GetUnusedRandomIndex(
                recipeSpawnPoints,
                lastSpawnPointIndex,
                spawnPointsDict
            );

            // Set selected transform as currently used
            spawnPointsDict[recipeSpawnPoints[randomIndex]] = true;

            // Spawn completed recipe
            SpawnObject(recipeSpawnPoints[randomIndex]);
        }
    }

    public void RemoveRecipe(GameObject objectToRemove)
    {
        // Set selected transform as not longer in use
        spawnPointsDict[recipeSpawnPoints[randomIndex]] = false;
        base.RemoveObject(objectToRemove);
    }
}
