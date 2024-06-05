using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.Rendering;
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

        InvokeRepeating(nameof(SpawnCustomer), firstSpawnTime, spawnTime);
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

    private void SpawnCustomer()
    {
        if (CanSpawn)
        {
            // Get a random spawn index that isn't in use
            int customerId = RandomIndex.GetUnusedRandomIndex(
                startPoints.ToArray(),
                lastSpawnPointIndex,
                spawnPointsDict
            );

            // Set selected transform as currently used
            spawnPointsDict[startPoints[customerId]] = true;

            // Select tween start and end point
            Transform startPoint = startPoints[customerId];
            Transform endPoint = endPoints[customerId];

            // Spawn customer
            GameObject spawnedCustomerObj = SpawnObject(startPoints[customerId]);
            Customer spawnedCustomer = spawnedCustomerObj.GetComponent<Customer>();
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
