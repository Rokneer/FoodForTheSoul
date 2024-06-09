using UnityEngine;

public class DeliveryArea : InteractableArea
{
    public Customer currentCustomer;

    private bool IsCurrentRecipeComplete =>
        currentCustomer != null && RecipeUIManager.Instance.equipedRecipe == currentCustomer.recipe;

    protected override bool CanShowHint => IsCurrentRecipeComplete;

    protected override void Interact()
    {
        if (IsCurrentRecipeComplete)
        {
            Debug.Log($"Delivered {currentCustomer.recipe.label} on {gameObject.name}");
            RecipeUIManager.Instance.HideRecipePhoto();
            CustomerSpawnManager.Instance.RemoveCustomer(currentCustomer);
            ScoreManager.Instance.AddScore(currentCustomer.recipe);
        }
    }
}
