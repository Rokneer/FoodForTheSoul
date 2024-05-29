using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [Header("Spawners")]
    [SerializeField]
    private GameObject spawnManager;
    private FoodSpawnManager foodSpawnManager;
    private CreatureSpawnManager creatureSpawnManager;
    private CustomerSpawnManager customerSpawnManager;

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

        foodSpawnManager = spawnManager.GetComponent<FoodSpawnManager>();
        creatureSpawnManager = spawnManager.GetComponent<CreatureSpawnManager>();
        customerSpawnManager = spawnManager.GetComponent<CustomerSpawnManager>();
    }

    private void DisableSpawner(Spawner spawner)
    {
        spawner.canSpawn = false;
    }
}
