using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawnManager : Spawner
{
    public static CreatureSpawnManager Instance { get; private set; }

    [Header("Spawn Points")]
    [SerializeField]
    private List<Transform> spawnPoints;

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
