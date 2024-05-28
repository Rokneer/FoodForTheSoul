using UnityEngine;

public class DeliveryArea : InteractableArea
{
    [SerializeField]
    private Recipe currentRecipe;

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
