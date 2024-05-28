using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    private int hookRandomIndex;
    private readonly int lastSpawnPointHookIndex = -1;
    private Dictionary<Transform, bool> hooksSpawnPointsDict = new();
    private bool IsSpawnFull
    {
        get
        {
            foreach (Transform spawnPoint in ceilingHooksSpawnPoints)
            {
                if (!hooksSpawnPointsDict[spawnPoint])
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

        for (int i = 0; i < ceilingPaths.Length; i++)
        {
            ceilingPathsStartPoints[i] = ceilingPaths[i].startPoint;
            ceilingPathsEndPoints[i] = ceilingPaths[i].endPoint;
        }

        for (int i = 0; i < ceilingHooks.Length; i++)
        {
            ceilingHooksSpawnPoints[i] = ceilingHooks[i].spawnPoint;
            hooksSpawnPointsDict[ceilingHooks[i].spawnPoint] = false;
        }
    }

    private void Start()
    {
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
        tweenMovement.StartMovement(() => RemoveObject(spawnedObj));
    }

    private void SpawnFoodInHook()
    {
        if (!IsSpawnFull)
        {
            // Get a random spawn index that isn't in use
            hookRandomIndex = RandomIndex.GetUnusedRandomIndex(
                ceilingHooksSpawnPoints,
                lastSpawnPointHookIndex,
                hooksSpawnPointsDict
            );

            // Set selected transform as currently used
            hooksSpawnPointsDict[ceilingHooksSpawnPoints[hookRandomIndex]] = true;
            ceilingHooks[hookRandomIndex].isActive = hooksSpawnPointsDict[
                ceilingHooksSpawnPoints[hookRandomIndex]
            ];

            CeilingHook currentCeilingHook = ceilingHooks[hookRandomIndex];

            Transform spawnPoint = ceilingHooksSpawnPoints[hookRandomIndex];

            GameObject spawnedObj = SpawnObject(spawnPoint, spawnPoint);
            SetupIngredient(spawnedObj);

            TweenMovement tweenMovement = currentCeilingHook.GetComponent<TweenMovement>();
            currentCeilingHook.isActive = true;
            tweenMovement
                .StartMovement(() =>
                {
                    StartCoroutine(
                        DelayHookRetraction(tweenMovement, currentCeilingHook, spawnedObj)
                    );
                })
                .SetAutoKill(false);
        }
    }

    private void RetractHook(
        TweenMovement tweenMovement,
        CeilingHook ceilingHook,
        GameObject spawnedObj
    )
    {
        tweenMovement.ReverseMovement().SetAutoKill(true);
        ceilingHook.isActive = false;
        hooksSpawnPointsDict[ceilingHooksSpawnPoints[hookRandomIndex]] = false;
        RemoveObject(spawnedObj);
    }

    private IEnumerator DelayHookRetraction(
        TweenMovement tweenMovement,
        CeilingHook ceilingHook,
        GameObject spawnedObj
    )
    {
        yield return new WaitForSeconds(ceilingHook.delay);
        RetractHook(tweenMovement, ceilingHook, spawnedObj);
    }

    private void SpawnFood()
    {
        if (canSpawn && currentObjectCount < maxObjectCount)
        {
            SpawnFoodInPath();
            /* int random = Random.Range(0, 3);
            switch (random)
            {
                case 0:
                    SpawnFoodInHook();
                    break;
                default:
                    SpawnFoodInPath();
                    break;
            } */
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
