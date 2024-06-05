using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CustomerSpawnManager : Spawner
{
    public static CustomerSpawnManager Instance { get; private set; }

    [Header("Spawn Points")]
    [SerializeField]
    private List<Transform> startPoints = new();

    [SerializeField]
    private List<Transform> endPoints = new();

    [SerializeField]
    private List<DeliveryArea> deliveryAreas = new();

    private readonly Dictionary<Transform, bool> spawnPointsDict = new();
    internal readonly Dictionary<Customer, int> currentCustomers = new();
    protected override bool CanSpawn => base.CanSpawn && !IsSpawnFull;
    private bool IsSpawnFull
    {
        get
        {
            foreach (Transform spawnPoint in startPoints)
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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        foreach (Transform spawnPoint in startPoints)
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
            new Quaternion(0, 180, 0, 0),
            PoolType.Customers
        );
    }

    internal IEnumerator SpawnCustomer()
    {
        if (CanSpawn)
        {
            // Get a random spawn index that isn't in use
            currentId = RandomIndex.GetUnusedRandomIndex(
                startPoints.ToArray(),
                currentId,
                spawnPointsDict
            );

            CreateCustomer(currentId);

            yield return new WaitForSeconds(spawnTime);

            StartCoroutine(SpawnCustomer());
        }
    }

    private void CreateCustomer(int customerId)
    {
        // Set selected transform as currently used
        spawnPointsDict[startPoints[customerId]] = true;

        // Select tween start and end point
        Transform startPoint = startPoints[customerId];
        Transform endPoint = endPoints[customerId];

        // Spawn customer
        Customer spawnedCustomer = SpawnObject(startPoints[customerId]).GetComponent<Customer>();

        spawnedCustomer.id = customerId;
        currentCustomers[spawnedCustomer] = customerId;

        // Set customer recipe
        spawnedCustomer.recipe = RecipeManager.Instance.ChooseRecipe(customerId);

        // Set delivery point recipe
        deliveryAreas[customerId].currentCustomer = spawnedCustomer;

        // Get tween movement
        TweenMovement movement = spawnedCustomer.movement;

        // Set timer on tween and UI
        float recipeTime = spawnedCustomer.recipe.time;
        movement.tweenTime = recipeTime;
        RecipeUIManager.Instance.SetupTimer(recipeTime, recipeTime, customerId);
        RecipeUIManager.Instance.StartTimer(customerId);

        // Start movement
        movement.SetUpMovement(startPoint, endPoint);
        movement.StartMovement();
        movement.tween.OnComplete(() => DamagePlayer(spawnedCustomer));
    }

    internal void RemoveCustomer(Customer customer)
    {
        int customerId = currentCustomers[customer];

        // End tween movement
        customer.movement.FinishTween();

        // Set selected transform as not longer in use
        spawnPointsDict[startPoints[customerId]] = false;

        // Remove delivery point recipe
        deliveryAreas[customerId].currentCustomer = customer;

        // Remove recipe from active
        RecipeManager.Instance.RemoveRecipe(customer.recipe, customerId);

        RecipeUIManager.Instance.DisableTimer(customerId);

        // Remove from current components
        currentCustomers.Remove(customer);
        base.RemoveObject(customer.gameObject);
    }

    private void DamagePlayer(Customer customer)
    {
        // Do damage to player
        customer.DoDamage();

        // Remove customer
        RemoveCustomer(customer);
    }
}
