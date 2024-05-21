using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> ObjectPools = new();
    private GameObject objectPoolEmptyHolder;
    private static GameObject gameObjectsEmpty;

    public static PoolType PoolingType;

    private void Awake()
    {
        SetupEmpties();
    }

    private void SetupEmpties()
    {
        objectPoolEmptyHolder = new GameObject("Pooled Objects");

        gameObjectsEmpty = new GameObject("GameObjects");
        gameObjectsEmpty.transform.SetParent(objectPoolEmptyHolder.transform);
    }

    public static GameObject SpawnObject(
        GameObject objectToSpawn,
        Vector3 spawnPosition,
        Quaternion spawnRotation,
        PoolType poolType = PoolType.None
    )
    {
        PooledObjectInfo pool = ObjectPools.Find(pool => pool.LookUpString == objectToSpawn.name);

        // Create pool if it doesn't exist
        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookUpString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObject == null)
        {
            GameObject parentObject = SetParentObject(poolType);

            // Create object if there are no inactive ones
            spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

            // Parent object to its empty holder
            if (parentObject != null)
            {
                spawnableObject.transform.SetParent(parentObject.transform);
            }
        }
        else
        {
            // Activate inactive object and set its position and rotation
            spawnableObject.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            pool.InactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }
        return spawnableObject;
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Transform parentTransform)
    {
        PooledObjectInfo pool = ObjectPools.Find(pool => pool.LookUpString == objectToSpawn.name);

        // Create pool if it doesn't exist
        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookUpString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObject == null)
        {
            // Create object if there are no inactive ones
            spawnableObject = Instantiate(objectToSpawn, parentTransform);
        }
        else
        {
            // Activate inactive object
            pool.InactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }
        return spawnableObject;
    }

    public static void ReturnToPool(GameObject objectToRemove)
    {
        // Remove (Clone) from object name
        string fixedName = objectToRemove.name.Replace("(Clone)", string.Empty);

        PooledObjectInfo pool = ObjectPools.Find(pool => pool.LookUpString == fixedName);

        if (pool == null)
        {
            Debug.LogWarning("Trying to release an object that isn't pooled" + objectToRemove.name);
        }
        else
        {
            objectToRemove.SetActive(false);
            pool.InactiveObjects.Add(objectToRemove);
        }
    }

    public static IEnumerator ReturnToPool(GameObject objectToRemove, float t = 0)
    {
        yield return new WaitForSeconds(t);
        ReturnToPool(objectToRemove);
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.GameObjects:
                return gameObjectsEmpty;
            case PoolType.None:
                return null;
            default:
                return null;
        }
    }
}

public class PooledObjectInfo
{
    public string LookUpString;
    public List<GameObject> InactiveObjects = new();
}

public enum PoolType
{
    GameObjects,
    None
}
