using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FoodForTheSoul/Recipe")]
public class RecipeData : PhotoObjectData
{
    public Sprite sprite;
    public List<IngredientData> ingredients;

    [Header("Data")]
    public float time;
    public int customerId;
}
