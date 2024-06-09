using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    public static List<PooledObjectInfo> ObjectPools = new();
    private GameObject objectPoolEmptyHolder;
    private static GameObject gameObjectsEmpty;
    private static GameObject ingredientsEmpty;
    private static GameObject recipesEmpty;
    private static GameObject creaturesEmpty;
    private static GameObject customersEmpty;

    public static PoolType PoolingType;

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
        SetupEmpties();
    }

    private void SetupEmpties()
    {
        objectPoolEmptyHolder = new GameObject(PoolStrings.PooledObjects);

        gameObjectsEmpty = new GameObject(PoolStrings.GameObjects);
        gameObjectsEmpty.transform.SetParent(objectPoolEmptyHolder.transform);

        recipesEmpty = new GameObject(PoolStrings.Recipes);
        recipesEmpty.transform.SetParent(objectPoolEmptyHolder.transform);

        ingredientsEmpty = new GameObject(PoolStrings.Ingredients);
        ingredientsEmpty.transform.SetParent(objectPoolEmptyHolder.transform);

        creaturesEmpty = new GameObject(PoolStrings.Creatures);
        creaturesEmpty.transform.SetParent(objectPoolEmptyHolder.transform);

        customersEmpty = new GameObject(PoolStrings.Customers);
        customersEmpty.transform.SetParent(objectPoolEmptyHolder.transform);
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
            Debug.LogWarning(
                "Trying to release an object that isn't pooled " + objectToRemove.name
            );
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
        return poolType switch
        {
            PoolType.Ingredients => ingredientsEmpty,
            PoolType.Recipes => recipesEmpty,
            PoolType.Creatures => creaturesEmpty,
            PoolType.Customers => customersEmpty,
            PoolType.GameObjects => gameObjectsEmpty,
            _ => null,
        };
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
    Ingredients,
    Recipes,
    Creatures,
    Customers,
    None
}
