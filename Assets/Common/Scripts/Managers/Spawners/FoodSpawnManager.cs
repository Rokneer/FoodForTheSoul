using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FoodSpawnManager : Spawner<FoodSpawnManager>
{
    #region Variables
    [Header("Spawn Points")]
    [SerializeField]
    private List<CeilingPath> ceilingPaths;
    private int currentFoodId = -1;
    internal bool canSpawnInPath = true;

    /* [SerializeField]
    private List<CeilingHook> ceilingHooks;
    private int currentHookId = -1;
    private readonly Dictionary<CeilingHook, bool> hooksDict = new();
    private readonly Dictionary<CeilingHook, int> hooksIdDict = new();
    private bool AreHooksFull => hooksDict.ContainsValue(false); */

    protected override bool CanSpawn =>
        base.CanSpawn && RecipeManager.Instance.activeIngredients.Count != 0;
    #endregion

    #region Lifecycle
    private void Awake()
    {
        /* foreach (CeilingHook hook in ceilingHooks)
        {
            hooksDict[hook] = false;
        } */
    }
    #endregion

    #region Functions
    protected override GameObject SpawnObject(Transform spawnPoint)
    {
        currentObjectCount++;
        return ObjectPoolManager.SpawnObject(
            objectToSpawn,
            spawnPoint.position,
            objectToSpawn.transform.rotation,
            PoolType.Ingredients
        );
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
    internal IEnumerator SpawnFoodInPath()
    {
        if (CanSpawn && canSpawnInPath)
        {
            currentFoodId = RandomIndex.GetRandomIndex(ceilingPaths, currentFoodId);

            CreateFoodInPath(currentFoodId);

            canSpawnInPath = false;
            yield return new WaitForSeconds(spawnTime);
            canSpawnInPath = true;
        }
    }

    private void CreateFoodInPath(int foodSpawnId)
    {
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

    /* #region Hooks
    internal IEnumerator SpawnFoodInHook()
    {
        if (CanSpawn && AreHooksFull)
        {
            // Get a random spawn index that isn't in use
            currentHookId = RandomIndex.GetUnusedRandomIndex(
                ceilingHooks.ToArray(),
                currentHookId,
                hooksDict
            );

            CreateFoodInHook(currentHookId);

            yield return new WaitForSeconds(spawnTime);

            StartCoroutine(SpawnFoodInHook());
        }
    }

    private void CreateFoodInHook(int hookId)
    {
        CeilingHook currentHook = ceilingHooks[hookId];
        hooksIdDict[currentHook] = hookId;

        // Set selected transform as currently used
        hooksDict[currentHook] = true;
        currentHook.isActive = hooksDict[ceilingHooks[hookId]];

        Transform spawnPoint = currentHook.spawnPoint;
        GameObject spawnedIngredient = SpawnObjectWithParent(spawnPoint);
        SetupIngredient(spawnedIngredient);

        TweenMovement tweenMovement = currentHook.GetComponent<TweenMovement>();
        currentHook.isActive = true;
        tweenMovement.StartMovement();
        tweenMovement
            .tween
            .OnComplete(
                () =>
                    StartCoroutine(
                        DelayHookRetraction(tweenMovement, currentHook, spawnedIngredient)
                    )
            );
    }

    private void RetractHook(
        TweenMovement tweenMovement,
        CeilingHook hook,
        GameObject ingredientObj
    )
    {
        tweenMovement.ReverseMovement();
        hook.isActive = false;

        int hookId = hooksIdDict[hook];

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
    #endregion Hooks */
    #endregion
}
