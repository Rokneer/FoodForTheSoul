using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FoodSpawnManager : Spawner
{
    #region Variables
    public static FoodSpawnManager Instance { get; private set; }

    [Header("Spawn Points")]
    [SerializeField]
    private List<CeilingPath> ceilingPaths;
    private readonly int lastPathIndex = -1;

    [SerializeField]
    private List<CeilingHook> ceilingHooks;
    private readonly int lastHookIndex = -1;
    private readonly Dictionary<CeilingHook, bool> hooksDict = new();
    private readonly Dictionary<CeilingHook, int> hooksIdDict = new();

    private bool AreHooksFull
    {
        get
        {
            foreach (CeilingHook hook in ceilingHooks)
            {
                if (!hooksDict[hook])
                {
                    return false;
                }
            }
            return true;
        }
    }

    protected override bool CanSpawn =>
        base.CanSpawn && RecipeManager.Instance.activeIngredients.Count != 0;
    #endregion Variables
    #region Lifecycle
    private void Awake()
    {
        // Checks if there is only one instance of the script in the scene
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        foreach (CeilingHook hook in ceilingHooks)
        {
            hooksDict[hook] = false;
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnFood), firstSpawnTime, spawnTime);
    }
    #endregion Lifecycle
    #region Functions
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

    private void SpawnFood()
    {
        if (CanSpawn)
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

    #region Paths
    private void SpawnFoodInPath()
    {
        int foodSpawnId = RandomIndex.GetRandomIndex(ceilingPaths.ToArray(), lastPathIndex);
        Transform startPoint = ceilingPaths[foodSpawnId].startPoint;
        Transform endPoint = ceilingPaths[foodSpawnId].endPoint;
        int loopCount = ceilingPaths[foodSpawnId].loopCount;

        GameObject spawnedIngredient = SpawnObject(startPoint);
        SetupIngredient(spawnedIngredient);

        // Setup movement
        TweenMovement tweenMovement = spawnedIngredient.GetComponent<TweenMovement>();
        tweenMovement.SetUpMovement(startPoint, endPoint);
        tweenMovement.StartMovement(Ease.InOutSine, loopCount);
        tweenMovement.tween.OnComplete(() => RemoveObject(spawnedIngredient));
    }
    #endregion Paths
    #region Hooks
    private void SpawnFoodInHook()
    {
        if (!AreHooksFull)
        {
            // Get a random spawn index that isn't in use
            int hookSpawnId = RandomIndex.GetUnusedRandomIndex(
                ceilingHooks.ToArray(),
                lastHookIndex,
                hooksDict
            );

            CeilingHook currentCeilingHook = ceilingHooks[hookSpawnId];
            hooksIdDict[currentCeilingHook] = hookSpawnId;
            Transform spawnPoint = currentCeilingHook.spawnPoint;

            // Set selected transform as currently used
            hooksDict[currentCeilingHook] = true;
            currentCeilingHook.isActive = hooksDict[ceilingHooks[hookSpawnId]];

            GameObject spawnedIngredient = SpawnObjectWithParent(spawnPoint);
            SetupIngredient(spawnedIngredient);

            TweenMovement tweenMovement = currentCeilingHook.GetComponent<TweenMovement>();
            currentCeilingHook.isActive = true;
            tweenMovement.StartMovement();
            tweenMovement
                .tween
                .OnComplete(
                    () =>
                        StartCoroutine(
                            DelayHookRetraction(
                                tweenMovement,
                                currentCeilingHook,
                                spawnedIngredient
                            )
                        )
                );
        }
    }

    private void RetractHook(
        TweenMovement tweenMovement,
        CeilingHook ceilingHook,
        GameObject ingredientObj
    )
    {
        tweenMovement.ReverseMovement();
        ceilingHook.isActive = false;

        int hookId = hooksIdDict[ceilingHook];

        hooksDict[ceilingHooks[hookId]] = false;

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
    #endregion Hooks
    #endregion Functions
}
