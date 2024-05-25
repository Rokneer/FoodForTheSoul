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
    private readonly int lastSpawnPointPathIndex = -1;

    [SerializeField]
    private CeilingHook[] ceilingHooks;
    private Transform[] ceilingHooksSpawnPoints = new Transform[7];
    private readonly int lastSpawnPointHookIndex = -1;

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

        InvokeRepeating(nameof(SpawnFood), firstSpawnTime, spawnTime);
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
        int randomIndex = RandomIndex.GetRandomIndex(
            ceilingPathsStartPoints,
            lastSpawnPointPathIndex
        );
        Transform startPoint = ceilingPathsStartPoints[randomIndex];
        Transform endPoint = ceilingPathsEndPoints[randomIndex];

        GameObject spawnedObj = SpawnObject(startPoint);
        SetupIngredient(spawnedObj);

        // Setup movement
        TweenMovement tweenMovement = spawnedObj.GetComponent<TweenMovement>();
        tweenMovement.SetUpMovement(startPoint, endPoint);
        tweenMovement.StartMovement();
    }

    private void SpawnFoodInHook()
    {
        //! TODO: Ensure ingredients don't stack in one hook

        int randomIndex = RandomIndex.GetRandomIndex(
            ceilingHooksSpawnPoints,
            lastSpawnPointHookIndex
        );

        CeilingHook currentCeilingHook = ceilingHooks[randomIndex];

        Transform spawnPoint = ceilingHooksSpawnPoints[randomIndex];

        GameObject spawnedObj = SpawnObject(spawnPoint, spawnPoint);
        SetupIngredient(spawnedObj);

        currentCeilingHook.GetComponent<TweenMovement>().StartMovement();
    }

    private void SpawnFood()
    {
        if (canSpawn && currentObjectCount < maxObjectCount)
        {
            int random = Random.Range(0, 3);
            switch (random)
            {
                case 0:
                    Invoke(nameof(SpawnFoodInHook), spawnTime);
                    break;
                default:
                    Invoke(nameof(SpawnFoodInPath), spawnTime);
                    break;
            }
        }
    }

    private void SetupIngredient(GameObject spawnedObj)
    {
        // Select ingredient from a current recipe
        Ingredient ingredient = spawnedObj.GetComponent<Ingredient>();
        ingredient.data = RecipeManager.Instance.ChooseRandomIngredient();

        // Setup ingredient mesh filter and renderer
        MeshFilter meshFilter = spawnedObj.GetComponent<MeshFilter>();
        meshFilter.sharedMesh = ingredient.data.meshFilter.sharedMesh;

        MeshRenderer meshRenderer = spawnedObj.GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterials = ingredient.data.meshRenderer.sharedMaterials;
    }
}
