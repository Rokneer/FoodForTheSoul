using UnityEngine;

public class DeliveryArea : InteractableArea
{
    [SerializeField]
    private Recipe currentRecipe;

    [SerializeField]
    private bool isCurrentRecipeComplete = false;
}
