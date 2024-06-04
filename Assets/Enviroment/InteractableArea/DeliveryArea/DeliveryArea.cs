using UnityEngine;

public class DeliveryArea : InteractableArea
{
    public Customer currentCustomer;
    private bool IsCurrentRecipeComplete =>
        RecipeUIManager.Instance.equipedRecipe == currentCustomer.recipe;

    protected override void Interact()
    {
        if (IsCurrentRecipeComplete)
        {
            Debug.Log($"Delivered {currentCustomer.recipe.label} on {gameObject.name}");
            RecipeUIManager.Instance.HideRecipePhoto();
            CustomerSpawnManager.Instance.RemoveCustomer(currentCustomer);
        }
    }
}
