using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    private static RecipeManager _instance;
    public static RecipeManager Instance => _instance;

    [Header("Current Ingredients")]
    [SerializeField]
    private List<IngredientData> currentIngredients;

    [SerializeField]
    private bool hasCompleteRecipe;

    [Header("Active Recipes")]
    [SerializeField]
    private List<Recipe> activeRecipes;

    private Dictionary<string, bool> isRecipeDoneDictionary = new();

    [SerializeField]
    private List<IngredientData> activeIngredients;

    private void Awake()
    {
        // Checks if there is only one instance of the script in the scene
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        foreach (Recipe recipe in activeRecipes)
        {
            isRecipeDoneDictionary[recipe.recipeName] = false;
            foreach (IngredientData ingredient in recipe.ingredients)
            {
                activeIngredients.Add(ingredient);
            }
        }
    }

    public void AddToCurrentIngredients(IngredientData ingredient)
    {
        Debug.Log($"Added {ingredient.objectName} to current recipe");
        currentIngredients.Add(ingredient);

        if (currentIngredients.Count >= 3)
        {
            CompletedRecipe();
        }
    }

    public void ResetCurrentIngredients()
    {
        currentIngredients.Clear();
    }

    private void CompletedRecipe()
    {
        foreach (Recipe recipe in activeRecipes)
        {
            foreach (IngredientData recipeIngredient in recipe.ingredients)
            {
                isRecipeDoneDictionary[recipe.recipeName] = currentIngredients.Contains(
                    recipeIngredient
                );
            }
            if (isRecipeDoneDictionary[recipe.recipeName])
            {
                Debug.Log($"{recipe.recipeName} is ready to serve!");
            }
        }
    }
}
