using UnityEngine;

public class FoodSpawnManager : Spawner
{
    private static FoodSpawnManager _instance;
    public static FoodSpawnManager Instance => _instance;

    [Header("Spawn Points")]
    [SerializeField]
    private CeilingPath[] ceilingPaths;
    private Transform[] ceilingPathsStartPoints = new Transform[4];
    private Transform[] ceilingPathsEndPoints = new Transform[4];

    [SerializeField]
    private CeilingHook[] ceilingHooks;
    private Transform[] ceilingHooksSpawnPoints = new Transform[7];

    private int randomRecipeIndex;
    private int randomIngredientIndex;

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
    }

    private void Start()
    {
        for (int i = 0; i < ceilingPaths.Length; i++)
        {
            ceilingPathsStartPoints[i] = ceilingPaths[i].startPoint;
            ceilingPathsEndPoints[i] = ceilingPaths[i].endPoint;
        }

        for (int i = 0; i < ceilingHooks.Length; i++)
        {
            ceilingHooksSpawnPoints[i] = ceilingHooks[i].spawnPoint;
        }

        InvokeRepeating(nameof(SpawnFoodInPath), spawnTime, spawnTime);
        InvokeRepeating(nameof(SpawnFoodInHook), spawnTime, spawnTime);
    }

    protected override GameObject SpawnObject(Transform spawnPoint)
    {
        currentObjectCount++;
        return ObjectPoolManager.SpawnObject(
            objectToSpawn,
            spawnPoint.position,
            Quaternion.identity,
            PoolType.Ingredient
        );
    }

    private void SpawnFoodInPath()
    {
        if (canSpawn)
        {
            int randomIndex = RandomIndex.GetRandomIndex(
                ceilingPathsStartPoints,
                lastSpawnPointIndex
            );
            Transform startPoint = ceilingPathsStartPoints[randomIndex];
            Transform endPoint = ceilingPathsEndPoints[randomIndex];

            if (currentObjectCount < maxObjectCount)
            {
                GameObject spawnedObj = SpawnObject(startPoint);
                // Assign ingredient from a current recipe
                TweenMovement tweenMovement = spawnedObj.GetComponent<TweenMovement>();
                tweenMovement.SetUpMovement(startPoint, endPoint);
                tweenMovement.StartMovement();
            }
        }
    }

    private void SpawnFoodInHook()
    {
        if (canSpawn)
        {
            Transform spawnPoint = ceilingHooksSpawnPoints[
                Random.Range(0, ceilingHooksSpawnPoints.Length)
            ];

            if (currentObjectCount < maxObjectCount)
            {
                SpawnObject(spawnPoint);
            }
        }
    }

    private IngredientData SelectIngredient()
    {
        // Select an recipe from the active list
        randomRecipeIndex = Random.Range(0, RecipeManager.Instance.activeRecipes.Count);
        Recipe selectedRecipe = RecipeManager.Instance.activeRecipes[randomRecipeIndex];

        // Select an ingredient from the selected recipe
        randomIngredientIndex = Random.Range(0, RecipeManager.Instance.activeRecipes.Count);
        return selectedRecipe.ingredients[randomIngredientIndex];
    }
}
