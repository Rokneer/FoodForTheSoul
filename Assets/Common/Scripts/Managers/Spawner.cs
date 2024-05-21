using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    [Header("Spawn Setup")]
    [SerializeField]
    protected float spawnTime;

    [SerializeField]
    private GameObject objectToSpawn;

    [Header("Object Data")]
    [SerializeField]
    protected int currentObjectCount;

    [SerializeField]
    protected int maxObjectCount;

    private int randomSpawnPointIndex;

    protected virtual GameObject SpawnObject(Transform[] spawnPoints)
    {
        randomSpawnPointIndex = Random.Range(0, spawnPoints.Length);

        currentObjectCount++;
        return ObjectPoolManager.SpawnObject(
            objectToSpawn,
            spawnPoints[randomSpawnPointIndex].position,
            Quaternion.identity,
            PoolType.GameObjects
        );
    }

    protected virtual void RemoveObject()
    {
        currentObjectCount--;
        ObjectPoolManager.ReturnToPool(objectToSpawn);
    }
}
