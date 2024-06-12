using UnityEngine;

public class TakeOutArea : InteractableArea
{
    [SerializeField]
    internal RecipeData recipe;
    private bool HasRecipe => recipe != null;
    protected override bool CanShowHint => HasRecipe;

    protected override void Interact()
    {
        if (HasRecipe)
        {
            Debug.Log($"Picked up {recipe.label}");

            RecipeUIManager.Instance.ShowRecipePhoto(recipe);

            RecipeSpawner.Instance.RemoveCompletedRecipe(recipe);

            recipe = null;
        }
    }
}
