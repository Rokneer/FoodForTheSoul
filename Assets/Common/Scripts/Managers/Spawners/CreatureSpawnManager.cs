using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawnManager : Spawner<CreatureSpawnManager>
{
    [Header("Spawn Points")]
    [SerializeField]
    private List<Transform> spawnPoints;

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
