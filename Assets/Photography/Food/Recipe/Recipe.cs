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
    public IngredientData ingredient;
    public int count;

    public RecipeIngredient(IngredientData ingredient, int count)
    {
        this.ingredient = ingredient;
        this.count = count;
    }
}
