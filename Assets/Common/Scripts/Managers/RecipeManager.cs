using System.Collections.Generic;
using System.Linq;
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

    [Header("Recipes")]
    [SerializeField]
    private List<Recipe> recipes;

    [SerializeField]
    private GameObject recipeUI;

    public List<Recipe> activeRecipes;

    private Dictionary<Recipe, bool> isRecipeDoneDictionary = new();

    [Header("Ingredients")]
    [SerializeField]
    private List<IngredientData> activeIngredients;
    private int lastIndex = -1;

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

    public Recipe ChooseRecipe()
    {
        int randomIndex = RandomIndex.GetRandomIndex(recipes.ToArray(), lastIndex);

        Recipe selectedRecipe = recipes[randomIndex];

        activeRecipes.Add(selectedRecipe);
        isRecipeDoneDictionary[selectedRecipe] = false;
        AddIngredients(selectedRecipe);

        return selectedRecipe;
    }

    private void AddIngredients(Recipe recipe)
    {
        List<IngredientData> uniqueIngredients = recipe.ingredients.Distinct().ToList();
        foreach (IngredientData uniqueIngredient in uniqueIngredients)
        {
            if (!activeIngredients.Contains(uniqueIngredient))
            {
                activeIngredients.Add(uniqueIngredient);
            }
        }
    }

    private void RemoveIngredients(Recipe recipe)
    {
        foreach (IngredientData ingredient in recipe.ingredients)
        {
            if (currentIngredients.Contains(ingredient))
            {
                activeIngredients.Remove(ingredient);
            }
        }
    }

    private void CompletedRecipe()
    {
        foreach (Recipe recipe in activeRecipes.ToArray())
        {
            foreach (IngredientData recipeIngredient in recipe.ingredients)
            {
                isRecipeDoneDictionary[recipe] = currentIngredients.Contains(recipeIngredient);
            }
            if (isRecipeDoneDictionary[recipe])
            {
                Debug.Log($"{recipe.recipeName} is ready to serve!");
                activeRecipes.Remove(recipe);
                RemoveIngredients(recipe);
            }
        }
    }
}
