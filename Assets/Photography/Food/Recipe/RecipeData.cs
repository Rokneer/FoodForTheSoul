using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FoodForTheSoul/Recipe")]
public class RecipeData : ScriptableObject
{
    [Header("Data")]
    public int customerId;
    public string recipeName;
    public float time;

    [Header("Visuals")]
    public Sprite sprite;
    public List<IngredientData> ingredients;
}
