using UnityEngine;

public class Ingredient : PhotoObject
{
    public override void WasPhotographed()
    {
        AddToCurrentRecipe();
        base.WasPhotographed();
    }

    private void AddToCurrentRecipe()
    {
        Debug.Log($"Added {data.objectName} to recipe");
    }
}
