using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FoodForTheSoul/Recipe")]
public class Recipe : ScriptableObject
{
    public string recipeName;
    public Sprite sprite;
    public List<IngredientData> ingredients;
}
