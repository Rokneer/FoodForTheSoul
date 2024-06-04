using UnityEngine;

public class TakeOutArea : InteractableArea
{
    public RecipeData recipe;

    protected override void Interact()
    {
        if (recipe != null)
        {
            Debug.Log($"Picked up {recipe.label}");

            RecipeUIManager.Instance.ShowRecipePhoto(recipe);

            RecipeSpawner.Instance.RemoveCompletedRecipe(recipe);

            recipe = null;
        }
    }
}
