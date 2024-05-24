using UnityEngine;

public abstract class PhotoObjectData : ScriptableObject
{
    public string objectName;
    public GameObject model;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
}
