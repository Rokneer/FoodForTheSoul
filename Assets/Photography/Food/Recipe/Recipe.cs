using UnityEngine;

public class Recipe : MonoBehaviour
{
    #region Variables
    public RecipeData data;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    #endregion Variables

    #region Functions
    public void SetMeshData()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        meshFilter.mesh = data.model.GetComponent<MeshFilter>().sharedMesh;

        meshRenderer.materials = data.model.GetComponent<MeshRenderer>().sharedMaterials;
    }
    #endregion Functions
}

[System.Serializable]
public class RecipeIngredient
{
    [SerializeReference]
    private IngredientData data;
    public IngredientData Data
    {
        get => data;
        set => data = value;
    }

    [SerializeField]
    private int count;
    public int Count
    {
        get => count;
        set => count = value;
    }

    public RecipeIngredient(IngredientData ingredient, int count)
    {
        this.Data = ingredient;
        this.count = count;
    }
}
