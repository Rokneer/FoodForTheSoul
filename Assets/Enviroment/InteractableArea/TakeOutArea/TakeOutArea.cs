using UnityEngine;

public class TakeOutArea : InteractableArea
{
    [SerializeField]
    private RecipeData recipe;

    [SerializeField]
    private bool hasPickedUpRecipe = false;

    protected override void Interact()
    {
        if (!hasPickedUpRecipe)
        {
            Debug.Log($"Take Out - Interacted with {gameObject.name}");
        }
    }
}
