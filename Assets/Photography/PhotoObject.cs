using UnityEngine;

public abstract class PhotoObject : MonoBehaviour
{
    #region Variables
    [SerializeField]
    protected PhotoObjectData data;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    #endregion Variables

    #region Lifecycle
    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        meshFilter.mesh = data.model.GetComponent<MeshFilter>().sharedMesh;

        meshRenderer.materials = data.model.GetComponent<MeshRenderer>().sharedMaterials;
    }
    #endregion Lifecycle

    #region Functions
    public virtual void WasPhotographed()
    {
        ObjectPoolManager.ReturnToPool(gameObject);
    }
    #endregion Functions
}
