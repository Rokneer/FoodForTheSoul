using UnityEngine;

public class FoodSpawnManager : Spawner
{
    [Header("Spawn Points")]
    [SerializeField]
    private CeilingPath[] ceilingPaths;
    private Transform[] ceilingPathsStartPoints;
    private Transform[] ceilingPathsEndPoints;

    [SerializeField]
    private CeilingHook[] ceilingHooks;
    private Transform[] ceilingHooksSpawnPoints;

    private void Start()
    {
        for (int i = 0; i < ceilingPaths.Length; i++)
        {
            ceilingPathsStartPoints[i] = ceilingPaths[i].startPoint;
            ceilingPathsEndPoints[i] = ceilingPaths[i].endPoint;
        }

        for (int i = 0; i < ceilingHooks.Length; i++)
        {
            ceilingHooksSpawnPoints[i] = ceilingHooks[i].spawnPoint;
        }

        InvokeRepeating(nameof(SpawnFoodInPath), spawnTime, spawnTime);
        InvokeRepeating(nameof(SpawnFoodInHook), spawnTime, spawnTime);
    }

    private void SpawnFoodInPath()
    {
        if (currentObjectCount < maxObjectCount)
        {
            GameObject spawnedObj = base.SpawnObject(ceilingPathsStartPoints);
            TweenMovement tweenMovement = spawnedObj.GetComponent<TweenMovement>();
            tweenMovement.startTransform = spawnedObj.transform;
            tweenMovement.endTransform = spawnedObj.transform;
        }
    }

    private void SpawnFoodInHook()
    {
        if (currentObjectCount < maxObjectCount)
        {
            base.SpawnObject(ceilingHooksSpawnPoints);
        }
    }
}
