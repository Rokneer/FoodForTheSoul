using UnityEngine;

public class TakeOutArea : InteractableArea
{
    public RecipeData recipe;

    [SerializeField]
    private bool hasPickedUpRecipe = false;

    protected override void Interact()
    {
        if (!hasPickedUpRecipe)
        {
            Debug.Log($"Take Out - Interacted with {recipe.label}");
            hasPickedUpRecipe = true;
        }
    }
}
