using System.Collections.Generic;
using UnityEngine;

public class RandomIndex : MonoBehaviour
{
    public static int GetRandomIndex<T>(T[] valueArray, int lastIndex)
        where T : Object
    {
        int randomIndex = Random.Range(0, valueArray.Length);
        if (randomIndex == 0 && randomIndex == lastIndex)
        {
            return randomIndex + 1;
        }
        else if (randomIndex > 0 && randomIndex <= valueArray.Length && randomIndex == lastIndex)
        {
            return randomIndex - 1;
        }
        lastIndex = randomIndex;
        return randomIndex;
    }

    public static int GetUnusedRandomIndex<T>(
        T[] valueArray,
        int lastIndex,
        Dictionary<T, bool> valuesDict
    )
        where T : Object
    {
        int randomIndex;
        do
        {
            randomIndex = RandomIndex.GetRandomIndex(valueArray, lastIndex);
        } while (valuesDict[valueArray[randomIndex]]);
        return randomIndex;
    }
}
