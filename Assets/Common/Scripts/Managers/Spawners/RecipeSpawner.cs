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
    private Dictionary<Transform, bool> spawnPointsDict = new(3);
    private Dictionary<RecipeData, int> completeRecipes = new(3);

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

    public void SpawnCompletedRecipe(RecipeData recipe)
    {
        if (canSpawn && !IsSpawnFull)
        {
            // Get a random spawn index that isn't in use
            int spawnIndex = RandomIndex.GetUnusedRandomIndex(
                recipeSpawnPoints,
                lastSpawnPointIndex,
                spawnPointsDict
            );

            // Select spawn point
            Transform spawnPoint = recipeSpawnPoints[spawnIndex];

            // Set selected transform as currently used
            spawnPointsDict[spawnPoint] = true;

            completeRecipes[recipe] = spawnIndex;

            // Spawn completed recipe
            SpawnObject(spawnPoint);
        }
    }

    public void RemoveCompletedRecipe(RecipeData recipeData)
    {
        Transform recipeTransform = recipeSpawnPoints[completeRecipes[recipeData]];

        // Set selected transform as not longer in use
        spawnPointsDict[recipeTransform] = false;
        base.RemoveObject(objectToSpawn);
    }
}
