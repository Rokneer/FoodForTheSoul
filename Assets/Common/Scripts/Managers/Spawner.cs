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

    private int lastSpawnPointIndex = -1;

    protected abstract GameObject SpawnObject(Transform spawnPoint);

    public virtual void RemoveObject(GameObject objectToRemove)
    {
        currentObjectCount--;
        ObjectPoolManager.ReturnToPool(objectToRemove);
    }

    protected int GetRandomSpawnPointIndex(Transform[] transforms)
    {
        int randomIndex = Random.Range(0, transforms.Length);
        if (randomIndex == 0 && randomIndex == lastSpawnPointIndex)
        {
            return randomIndex + 1;
        }
        else if (
            randomIndex > 0
            && randomIndex <= transforms.Length
            && randomIndex == lastSpawnPointIndex
        )
        {
            return randomIndex - 1;
        }
        lastSpawnPointIndex = randomIndex;
        return randomIndex;
    }
}
