using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CustomerSpawnManager : Spawner
{
    private static CustomerSpawnManager _instance;
    public static CustomerSpawnManager Instance => _instance;

    [Header("Spawn Points")]
    [SerializeField]
    private Transform[] startPoints = new Transform[3];

    [SerializeField]
    private Transform[] endPoints = new Transform[3];

    [SerializeField]
    private DeliveryArea[] deliveryAreas = new DeliveryArea[3];

    private readonly Dictionary<Transform, bool> spawnPointsDict = new(3);
    public readonly Dictionary<Customer, int> currentCustomers = new(3);
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
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
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
                startPoints,
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
            deliveryAreas[customerId].currentRecipe = spawnedCustomer.recipe;

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
            movement.tween.OnComplete(() => RemoveCustomer(spawnedCustomer));
        }
    }

    public void RemoveCustomer(Customer customer)
    {
        int customerId = currentCustomers[customer];

        // Set selected transform as not longer in use
        spawnPointsDict[startPoints[customerId]] = false;

        // Remove delivery point recipe
        deliveryAreas[customerId].currentRecipe = customer.recipe;

        // Remove recipe from active
        RecipeManager.Instance.RemoveRecipe(customer.recipe, customerId);

        // Do damage to player
        customer.DoDamage();

        RecipeUIManager.Instance.DisableTimer(customerId);

        // Remove from current components
        currentCustomers.Remove(customer);
        base.RemoveObject(customer.gameObject);
    }
}
