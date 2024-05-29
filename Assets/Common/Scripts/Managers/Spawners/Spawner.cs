using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    [Header("Spawn Setup")]
    [SerializeField]
    protected float firstSpawnTime;

    [SerializeField]
    protected float spawnTime;

    [SerializeField]
    protected GameObject objectToSpawn;

    public bool isActive = true;
    protected virtual bool CanSpawn => isActive && currentObjectCount < maxObjectCount;

    [Header("Object Data")]
    [SerializeField]
    protected int currentObjectCount;

    [SerializeField]
    protected int maxObjectCount;

    protected int lastSpawnPointIndex = -1;

    protected abstract GameObject SpawnObject(Transform spawnPoint);

    protected GameObject SpawnObject(Transform spawnPoint, Transform parentTransform)
    {
        currentObjectCount++;
        return ObjectPoolManager.SpawnObject(objectToSpawn, parentTransform);
    }

    public virtual void RemoveObject(GameObject objectToRemove)
    {
        currentObjectCount--;
        ObjectPoolManager.ReturnToPool(objectToRemove);
    }
}
