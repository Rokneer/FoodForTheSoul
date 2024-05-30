using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    private static RecipeManager _instance;
    public static RecipeManager Instance => _instance;

    [Header("Ingredients")]
    public List<IngredientData> currentIngredients;
    public List<IngredientData> activeIngredients;

    [SerializeField]
    private bool hasCompleteRecipe;

    [Header("Recipes")]
    [SerializeField]
    private List<RecipeData> recipes;

    [SerializeField]
    private GameObject recipeUI;

    public List<RecipeData> currentRecipes;
    private readonly int lastIngredientIndex = -1;
    private readonly int lastRecipeIndex = -1;

    private readonly Dictionary<RecipeData, bool> isRecipeDoneDictionary = new();

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
        Debug.Log($"Added {ingredient.label} to current recipe");
        currentIngredients.Add(ingredient);

        if (currentIngredients.Count >= 3)
        {
            CompleteRecipe();
        }
    }

    public void ResetCurrentIngredients()
    {
        currentIngredients.Clear();
    }

    private void AddIngredients(RecipeData recipe)
    {
        List<RecipeIngredient> uniqueIngredients = recipe.ingredients.Distinct().ToList();
        foreach (RecipeIngredient uniqueIngredient in uniqueIngredients)
        {
            if (!activeIngredients.Contains(uniqueIngredient.ingredient))
            {
                activeIngredients.Add(uniqueIngredient.ingredient);
            }
        }
    }

    private void RemoveIngredients(RecipeData recipe)
    {
        List<RecipeIngredient> uniqueIngredients = recipe.ingredients.Distinct().ToList();

        List<IngredientData> uniqueIngredientsData = new();
        uniqueIngredientsData.AddRange(uniqueIngredients.Select(data => data.ingredient));

        List<IngredientData> ingredientsToRemove = uniqueIngredientsData
            .Except(activeIngredients)
            .ToList();

        foreach (IngredientData ingredient in ingredientsToRemove)
        {
            Debug.Log($"Removed {ingredient}");
            activeIngredients.Remove(ingredient);
        }
    }

    public IngredientData ChooseRandomIngredient()
    {
        int ingredientId = RandomIndex.GetRandomIndex(
            activeIngredients.ToArray(),
            lastIngredientIndex
        );

        return activeIngredients[ingredientId];
    }

    public RecipeData ChooseRecipe(int customerId)
    {
        // Select a random recipe
        int recipeId = RandomIndex.GetRandomIndex(recipes.ToArray(), lastRecipeIndex);
        RecipeData selectedRecipe = recipes[recipeId];

        // Add recipe and its ingredients to currently active
        currentRecipes.Add(selectedRecipe);
        isRecipeDoneDictionary[selectedRecipe] = false;
        AddIngredients(selectedRecipe);

        // Setup recipe's ingredients sprites
        Sprite[] recipeIngredientSprites = new Sprite[selectedRecipe.ingredients.Count];
        for (int i = 0; i < selectedRecipe.ingredients.Count; i++)
        {
            RecipeIngredient ingredient = selectedRecipe.ingredients[i];
            recipeIngredientSprites[i] = ingredient.ingredient.sprite;
        }

        // Set recipe sprites in UI
        RecipeUIManager
            .Instance
            .AddPhoto(selectedRecipe.sprite, recipeIngredientSprites, customerId);

        return selectedRecipe;
    }

    private void CompleteRecipe()
    {
        bool spawnedRecipe = false;
        foreach (RecipeData recipe in currentRecipes.ToArray())
        {
            //! TODO: Check for ingredient quantity
            isRecipeDoneDictionary[recipe] = CheckCurrentIngredients(recipe);

            if (isRecipeDoneDictionary[recipe] && !spawnedRecipe)
            {
                Debug.Log($"{recipe.label} is ready to serve!");
                RecipeSpawner.Instance.SpawnCompletedRecipe(recipe);
                spawnedRecipe = true;
            }
        }
    }

    private bool CheckCurrentIngredients(RecipeData recipe)
    {
        List<IngredientData> recipeIngredientsData = new();

        recipeIngredientsData.AddRange(recipe.ingredients.Select(data => data.ingredient));

        bool containsAllIngredients = currentIngredients.ContainsAllItems<IngredientData>(
            recipeIngredientsData
        );

        if (containsAllIngredients)
        {
            List<RecipeIngredient> currentIngredientGroups = currentIngredients
                .GroupBy(group => group)
                .Select(group => new RecipeIngredient(group.Key, group.Count()))
                .ToList();

            return true;
        }
        return false;
    }

    public void RemoveRecipe(RecipeData recipe, int photoId)
    {
        Debug.Log($"Removed {recipe.label} with on photo {photoId}");

        RecipeUIManager.Instance.HidePhoto(photoId);

        currentRecipes.Remove(recipe);
        RemoveIngredients(recipe);
    }
}
