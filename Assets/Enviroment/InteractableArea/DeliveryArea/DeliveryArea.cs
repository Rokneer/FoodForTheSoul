using UnityEngine;

public class DeliveryArea : InteractableArea
{
    [SerializeField]
    private RecipeData currentRecipe;

    [SerializeField]
    private bool isCurrentRecipeComplete = false;

    protected override void Interact()
    {
        if (isCurrentRecipeComplete)
        {
            Debug.Log($"Delivery - Interacted with {gameObject.name}");
        }
    }
}
