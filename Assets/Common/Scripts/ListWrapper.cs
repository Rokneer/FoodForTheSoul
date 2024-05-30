using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ListWrapper<T> : List<T>
    where T : Object
{
    public List<T> innerList;
}
