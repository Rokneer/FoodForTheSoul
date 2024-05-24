using System.Collections.Generic;
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
    private Dictionary<Transform, bool> spawnPointsDict = new();

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
    private int randomIndex;

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
        if (canSpawn)
        {
            if (!IsSpawnFull)
            {
                // Get a random spawn index that isn't in use
                do
                {
                    randomIndex = RandomIndex.GetRandomIndex(startPoints, lastSpawnPointIndex);
                } while (spawnPointsDict[startPoints[randomIndex]]);

                // Set selected transform as currently used
                spawnPointsDict[startPoints[randomIndex]] = true;

                // Select tween start and end point
                Transform startPoint = startPoints[randomIndex];
                Transform endPoint = endPoints[randomIndex];

                // Spawn customer
                GameObject spawnedCustomerObj = SpawnObject(startPoints[randomIndex]);

                // Assign recipe
                Customer spawnedCustomer = spawnedCustomerObj.GetComponent<Customer>();
                spawnedCustomer.recipe = RecipeManager.Instance.ChooseRecipe();

                // Get tween movement
                TweenMovement tweenMovement = spawnedCustomerObj.GetComponent<TweenMovement>();

                // Set timer
                tweenMovement.tweenTime = spawnedCustomer.recipe.time;
                //* Start timer on recipe UI

                // Start movement
                tweenMovement.SetUpMovement(startPoint, endPoint);
                tweenMovement.StartMovement();
            }
        }
    }
}
