using UnityEngine;

public class CustomerSpawnManager : Spawner
{
    private static CustomerSpawnManager _instance;
    public static CustomerSpawnManager Instance => _instance;

    [Header("Spawn Points")]
    [SerializeField]
    private Transform[] spawnPoints;

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
    }

    protected override GameObject SpawnObject(Transform spawnPoint)
    {
        throw new System.NotImplementedException();
    }
}
