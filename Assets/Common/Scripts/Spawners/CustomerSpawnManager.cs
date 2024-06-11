using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CustomerSpawnManager : Spawner<CustomerSpawnManager>
{
    [Header("Skins")]
    [SerializeField]
    private List<GameObject> customerSkins = new();
    private int currentSkinId;

    [Header("Rows")]
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
            objectToSpawn.transform.rotation,
            PoolType.Customers
        );
    }

    protected GameObject SpawnObject(Transform spawnPoint, GameObject obj)
    {
        currentObjectCount++;
        return ObjectPoolManager.SpawnObject(
            obj,
            spawnPoint.position,
            obj.transform.rotation,
            PoolType.Customers
        );
    }

    internal override void StartSpawner()
    {
        InvokeRepeating(nameof(SpawnCustomer), firstSpawnTime, spawnTime);
    }

    internal override void StopSpawner()
    {
        CancelInvoke(nameof(SpawnCustomer));
    }

    internal void SpawnCustomer()
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
        }
    }

    private Customer SetupCustomer(int customerId)
    {
        // Select a random skin
        currentSkinId = RandomIndex.GetRandomIndex(customerSkins, currentSkinId);
        GameObject customerSkin = customerSkins[currentSkinId];

        // Spawn customer
        GameObject customerObj = SpawnObject(startPoints[customerId], customerSkin);

        return customerObj.GetComponent<Customer>();
    }

    private void CreateCustomer(int customerId)
    {
        // Set selected transform as currently used
        spawnPointsDict[startPoints[customerId]] = true;

        // Select tween start and end point
        Transform startPoint = startPoints[customerId];
        Transform endPoint = endPoints[customerId];

        // Setup customer
        Customer customer = SetupCustomer(customerId);
        customer.id = customerId;
        currentCustomers[customer] = customerId;

        // Set customer recipe
        customer.recipe = RecipeManager.Instance.ChooseRecipe(customerId);

        // Set delivery point recipe
        deliveryAreas[customerId].currentCustomer = customer;

        // Get tween movement
        TweenMovement movement = customer.movement;

        // Set timer on tween and UI
        float recipeTime = customer.recipe.time;
        movement.tweenTime = recipeTime;
        RecipeUIManager.Instance.SetupTimer(recipeTime, recipeTime, customerId);
        RecipeUIManager.Instance.StartTimer(customerId);

        // Start movement
        movement.SetUpMovement(startPoint, endPoint);
        movement.StartMovement();
        movement.tween.OnComplete(() => DamagePlayer(customer));

        customer.isActive = true;
    }

    internal void RemoveCustomer(Customer customer)
    {
        customer.isActive = false;

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
