using System.Collections.Generic;
using UnityEngine;

public class RandomIndex
{
    private static int InternalRandomIndex(int length, int lastIndex)
    {
        if (length == 1)
        {
            return 0;
        }

        int randomIndex = Random.Range(0, length);
        if (randomIndex == lastIndex)
        {
            if (randomIndex == 0)
            {
                return randomIndex + 1;
            }
            else if (randomIndex != 0 && randomIndex <= length)
            {
                return randomIndex - 1;
            }
        }
        return randomIndex;
    }

    public static int GetRandomIndex<T>(T[] valueArray, int lastIndex)
        where T : Object => InternalRandomIndex(valueArray.Length, lastIndex);

    public static int GetRandomIndex<T>(List<T> valueList, int lastIndex)
        where T : Object => InternalRandomIndex(valueList.Count, lastIndex);

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
            randomIndex = GetRandomIndex(valueArray, lastIndex);
            lastIndex = randomIndex;
        } while (valuesDict[valueArray[randomIndex]]);
        return randomIndex;
    }

    public static int GetUnusedRandomIndex<T>(
        List<T> valueList,
        int lastIndex,
        Dictionary<T, bool> valuesDict
    )
        where T : Object
    {
        int randomIndex;
        do
        {
            randomIndex = GetRandomIndex(valueList, lastIndex);
            lastIndex = randomIndex;
        } while (valuesDict[valueList[randomIndex]]);
        return randomIndex;
    }
}
