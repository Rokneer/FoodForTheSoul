using System.Collections.Generic;
using System.Linq;
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

    private Dictionary<Transform, bool> spawnPointsDict = new(3);
    private Dictionary<Customer, int> currentCustomers = new(3);
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
        if (canSpawn && !IsSpawnFull)
        {
            // Get a random spawn index that isn't in use
            int customerIndex = RandomIndex.GetUnusedRandomIndex(
                startPoints,
                lastSpawnPointIndex,
                spawnPointsDict
            );

            // Set selected transform as currently used
            spawnPointsDict[startPoints[customerIndex]] = true;

            // Select tween start and end point
            Transform startPoint = startPoints[customerIndex];
            Transform endPoint = endPoints[customerIndex];

            // Spawn customer
            GameObject spawnedCustomerObj = SpawnObject(startPoints[customerIndex]);
            Customer spawnedCustomer = spawnedCustomerObj.GetComponent<Customer>();

            currentCustomers[spawnedCustomer] = customerIndex;

            // Set customer recipe
            spawnedCustomer.recipe = RecipeManager.Instance.ChooseRecipe(customerIndex);

            // Get tween movement
            TweenMovement tweenMovement = spawnedCustomerObj.GetComponent<TweenMovement>();

            // Set timer
            tweenMovement.tweenTime = spawnedCustomer.recipe.time;
            //* Start timer on recipe UI

            // Start movement
            tweenMovement.SetUpMovement(startPoint, endPoint);
            tweenMovement.StartMovement(() =>
            {
                RemoveCustomer(spawnedCustomer);
            });
        }
    }

    public void RemoveCustomer(Customer customer)
    {
        int customerIndex = currentCustomers[customer];

        // Set selected transform as not longer in use
        spawnPointsDict[startPoints[customerIndex]] = false;

        // Do damage to player
        customer.DoDamage();

        // Remove recipe from active
        RecipeManager.Instance.RemoveRecipe(customer.recipe, customerIndex);

        // Remove from current components
        currentCustomers.Remove(customer);
        base.RemoveObject(customer.gameObject);
    }
}
