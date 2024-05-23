using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FoodForTheSoul/Recipe")]
public class Recipe : ScriptableObject
{
    [Header("Data")]
    public string recipeName;
    public float time;

    [Header("Visuals")]
    public Sprite sprite;
    public List<IngredientData> ingredients;
}
