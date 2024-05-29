public class Ingredient : PhotoObject
{
    public override void WasPhotographed()
    {
        RecipeManager.Instance.AddToCurrentIngredients((IngredientData)data);
        FoodSpawnManager.Instance.RemoveObject(gameObject);
    }
}
