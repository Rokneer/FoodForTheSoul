using UnityEngine;

public class CreatureSpawnManager : Spawner
{
    private static CreatureSpawnManager _instance;
    public static CreatureSpawnManager Instance => _instance;

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
        currentObjectCount++;
        return ObjectPoolManager.SpawnObject(
            objectToSpawn,
            spawnPoint.position,
            Quaternion.identity,
            PoolType.Creatures
        );
    }
}
