using UnityEngine;

public class TakeOutArea : InteractableArea
{
    [SerializeField]
    private Recipe recipe;

    [SerializeField]
    private bool hasPickedUpRecipe = false;
}
