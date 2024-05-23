using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    [Header("Spawn Setup")]
    [SerializeField]
    protected float spawnTime;

    [SerializeField]
    protected GameObject objectToSpawn;

    public bool canSpawn = true;

    [Header("Object Data")]
    [SerializeField]
    protected int currentObjectCount;

    [SerializeField]
    protected int maxObjectCount;

    protected int lastSpawnPointIndex = -1;

    protected abstract GameObject SpawnObject(Transform spawnPoint);

    public virtual void RemoveObject(GameObject objectToRemove)
    {
        currentObjectCount--;
        ObjectPoolManager.ReturnToPool(objectToRemove);
    }
}
