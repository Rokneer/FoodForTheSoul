using System.Collections.Generic;
using UnityEngine;

public class RecipeSpawner : Spawner
{
    private static RecipeSpawner _instance;
    public static RecipeSpawner Instance => _instance;

    [Header("Spawn Points")]
    [SerializeField]
    private Transform[] recipeSpawnPoints;
    private readonly Dictionary<Transform, bool> spawnPointsDict = new(3);
    private readonly Dictionary<RecipeData, int> completeRecipes = new(3);

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
            PoolType.Recipes
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

            // Get take out area
            TakeOutArea takeOutArea = spawnPoint.gameObject.GetComponentInParent<TakeOutArea>();

            // Set take out area recipe
            takeOutArea.recipe = recipe;

            // Spawn completed recipe
            GameObject spawnedRecipe = SpawnObject(spawnPoint);

            SetupRecipe(spawnedRecipe, recipe);
        }
    }

    public void RemoveCompletedRecipe(RecipeData recipeData)
    {
        Transform recipeTransform = recipeSpawnPoints[completeRecipes[recipeData]];

        // Set selected transform as not longer in use
        spawnPointsDict[recipeTransform] = false;
        base.RemoveObject(objectToSpawn);
    }

    private void SetupRecipe(GameObject spawnedRecipe, RecipeData recipeData)
    {
        // Get current recipe data
        Recipe recipe = spawnedRecipe.GetComponent<Recipe>();
        recipe.data = recipeData;

        // Setup current mesh filter and renderer
        recipe.SetMeshData();
    }
}
