using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FoodForTheSoul/Recipe")]
public class RecipeData : PhotoObjectData
{
    public Sprite sprite;
    public List<RecipeIngredient> ingredients;

    [Header("Data")]
    public int points;
    public float time;
}
