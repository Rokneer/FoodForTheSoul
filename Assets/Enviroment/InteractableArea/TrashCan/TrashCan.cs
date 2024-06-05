public class TrashCan : InteractableArea
{
    protected override void Interact()
    {
        RecipeUIManager.Instance.HideRecipePhoto();
    }
}
