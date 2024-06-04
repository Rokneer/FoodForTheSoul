using UnityEngine;

public abstract class PhotoObjectData : ScriptableObject
{
    public string label;

    [Header("Visuals")]
    public GameObject model;
}
