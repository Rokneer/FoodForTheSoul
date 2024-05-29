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
    private readonly Transform[] ceilingPathsStartPoints = new Transform[4];
    private readonly Transform[] ceilingPathsEndPoints = new Transform[4];
    private readonly int lastPathIndex = -1;

    [SerializeField]
    private CeilingHook[] ceilingHooks;
    private readonly Transform[] ceilingHooksSpawnPoints = new Transform[7];
    private readonly int lastHookIndex = -1;
    private readonly Dictionary<Transform, bool> hooksSpawnPointsDict = new(7);
    private readonly Dictionary<CeilingHook, int> hooksIndexDict = new(7);
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
            PoolType.Ingredients
        );
    }

    private void SpawnFoodInPath()
    {
        int randomIndex = RandomIndex.GetRandomIndex(ceilingPathsStartPoints, lastPathIndex);
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
            int hookIndex = RandomIndex.GetUnusedRandomIndex(
                ceilingHooksSpawnPoints,
                lastHookIndex,
                hooksSpawnPointsDict
            );

            // Set selected transform as currently used
            hooksSpawnPointsDict[ceilingHooksSpawnPoints[hookIndex]] = true;
            ceilingHooks[hookIndex].isActive = hooksSpawnPointsDict[
                ceilingHooksSpawnPoints[hookIndex]
            ];

            CeilingHook currentCeilingHook = ceilingHooks[hookIndex];
            hooksIndexDict[currentCeilingHook] = hookIndex;

            Transform spawnPoint = ceilingHooksSpawnPoints[hookIndex];

            GameObject spawnedIngredient = SpawnObject(spawnPoint, spawnPoint);
            SetupIngredient(spawnedIngredient);

            TweenMovement tweenMovement = currentCeilingHook.GetComponent<TweenMovement>();
            currentCeilingHook.isActive = true;
            tweenMovement
                .StartMovement(() =>
                {
                    StartCoroutine(
                        DelayHookRetraction(tweenMovement, currentCeilingHook, spawnedIngredient)
                    );
                })
                .SetAutoKill(false);
        }
    }

    private void RetractHook(
        TweenMovement tweenMovement,
        CeilingHook ceilingHook,
        GameObject ingredientObj
    )
    {
        tweenMovement.ReverseMovement().SetAutoKill(true);
        ceilingHook.isActive = false;

        int hookIndex = hooksIndexDict[ceilingHook];

        hooksSpawnPointsDict[ceilingHooksSpawnPoints[hookIndex]] = false;

        RemoveObject(ingredientObj);
    }

    private IEnumerator DelayHookRetraction(
        TweenMovement tweenMovement,
        CeilingHook ceilingHook,
        GameObject ingredientObj
    )
    {
        yield return new WaitForSeconds(ceilingHook.delay);
        RetractHook(tweenMovement, ceilingHook, ingredientObj);
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

    private void SetupIngredient(GameObject spawnedIngredient)
    {
        // Select ingredient from a current recipe
        Ingredient ingredient = spawnedIngredient.GetComponent<Ingredient>();
        ingredient.data = RecipeManager.Instance.ChooseRandomIngredient();

        // Setup ingredient mesh filter and renderer
        ingredient.SetMeshData();
    }
}
