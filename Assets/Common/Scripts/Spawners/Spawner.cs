using UnityEngine;

public abstract class Spawner<T> : Singleton<T>
    where T : MonoBehaviour
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

    protected int currentId = -1;

    protected abstract GameObject SpawnObject(Transform spawnPoint);
    internal abstract void StartSpawner();
    internal abstract void StopSpawner();

    protected GameObject SpawnObjectWithParent(Transform parentTransform)
    {
        currentObjectCount++;
        return ObjectPoolManager.SpawnObject(objectToSpawn, parentTransform);
    }

    internal virtual void RemoveObject(GameObject objectToRemove)
    {
        currentObjectCount--;
        ObjectPoolManager.ReturnToPool(objectToRemove);
    }
}
