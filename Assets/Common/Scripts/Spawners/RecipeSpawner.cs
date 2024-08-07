using System.Collections.Generic;
using UnityEngine;

public class RecipeSpawner : Spawner<RecipeSpawner>
{
    [Header("Spawn Points")]
    [SerializeField]
    private List<Transform> recipeSpawnPoints;
    private readonly Dictionary<Transform, bool> spawnPointsDict = new();
    private readonly Dictionary<RecipeData, int> completeRecipes = new();
    protected override bool CanSpawn => base.CanSpawn && !IsSpawnFull;
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

    [Header("Audio")]
    [SerializeField]
    private AudioClip recipeDoneSound;

    private void Awake()
    {
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

    internal override void StartSpawner()
    {
        //* Start spawner
    }

    internal override void StopSpawner()
    {
        //* Stop spawner
    }

    internal void SpawnCompletedRecipe(RecipeData recipe)
    {
        if (CanSpawn)
        {
            // Get a random spawn index that isn't in use
            currentId = RandomIndex.GetUnusedRandomIndex(
                recipeSpawnPoints.ToArray(),
                currentId,
                spawnPointsDict
            );

            // Select spawn point
            Transform spawnPoint = recipeSpawnPoints[currentId];

            // Set selected transform as currently used
            spawnPointsDict[spawnPoint] = true;
            completeRecipes[recipe] = currentId;

            // Get take out area
            TakeOutArea takeOutArea = spawnPoint.gameObject.GetComponentInParent<TakeOutArea>();

            // Set take out area recipe
            takeOutArea.recipe = recipe;

            // Spawn completed recipe
            GameObject spawnedRecipe = SpawnObjectWithParent(spawnPoint);

            spawnedRecipe.SetActive(true);

            SetupRecipe(spawnedRecipe, recipe);

            SoundFXManager.Instance.PlaySFXClip(recipeDoneSound, transform, 0.5f);
        }
    }

    internal void RemoveCompletedRecipe(RecipeData recipe)
    {
        Transform spawnTransform = recipeSpawnPoints[completeRecipes[recipe]];

        // Set selected transform as not longer in use
        spawnPointsDict[spawnTransform] = false;

        GameObject recipeObj = spawnTransform.GetComponentInChildren<Recipe>().gameObject;

        Debug.Log($"Removed {recipe.label}");
        base.RemoveObject(recipeObj);
    }

    private void SetupRecipe(GameObject spawnedRecipe, RecipeData recipeData)
    {
        // Get current recipe data
        Recipe recipe = spawnedRecipe.GetComponent<Recipe>();
        recipe.data = recipeData;

        recipe.transform.eulerAngles = new Vector3(-90, 0, 0);

        // Setup current mesh filter and renderer
        recipe.SetMeshData();
    }
}
