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
            SoundFXManager
                .Instance
                .PlayRandomSFXClip(currentCustomer.eatingSounds, transform, 0.4f);

            RecipeUIManager.Instance.HideRecipePhoto();

            CustomerSpawnManager.Instance.RemoveCustomer(currentCustomer);

            ScoreManager.Instance.AddScore(currentCustomer.recipe);
        }
    }
}
