using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    private static RecipeManager _instance;
    public static RecipeManager Instance => _instance;

    [Header("Ingredients")]
    public List<IngredientData> currentIngredients;

    [SerializeField]
    private List<IngredientData> activeIngredients;

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
        List<IngredientData> uniqueIngredients = recipe.ingredients.Distinct().ToList();
        foreach (IngredientData uniqueIngredient in uniqueIngredients)
        {
            if (!activeIngredients.Contains(uniqueIngredient))
            {
                activeIngredients.Add(uniqueIngredient);
            }
        }
    }

    private void RemoveIngredients(RecipeData recipe)
    {
        List<IngredientData> uniqueIngredients = recipe.ingredients.Distinct().ToList();
        List<IngredientData> ingredientsToRemove = uniqueIngredients
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
        selectedRecipe.customerId = customerId;

        // Add recipe and its ingredients to currently active
        currentRecipes.Add(selectedRecipe);
        isRecipeDoneDictionary[selectedRecipe] = false;
        AddIngredients(selectedRecipe);

        // Setup recipe's ingredients sprites
        Sprite[] recipeIngredientSprites = new Sprite[selectedRecipe.ingredients.Count];
        for (int i = 0; i < selectedRecipe.ingredients.Count; i++)
        {
            IngredientData ingredient = selectedRecipe.ingredients[i];
            recipeIngredientSprites[i] = ingredient.sprite;
        }

        // Set recipe sprites in UI
        RecipeUIManager.Instance.AddPhoto(
            selectedRecipe.sprite,
            recipeIngredientSprites,
            customerId
        );

        return selectedRecipe;
    }

    private void CompleteRecipe()
    {
        bool spawnedRecipe = false;
        foreach (RecipeData recipe in currentRecipes.ToArray())
        {
            //! TODO: Check for ingredient quantity
            isRecipeDoneDictionary[recipe] = currentIngredients.ContainsAllItems(
                recipe.ingredients
            );

            if (isRecipeDoneDictionary[recipe] && !spawnedRecipe)
            {
                Debug.Log($"{recipe.label} is ready to serve!");
                RecipeSpawner.Instance.SpawnCompletedRecipe(recipe);
                spawnedRecipe = true;
            }
        }
    }

    public void RemoveRecipe(RecipeData recipe, int photoId)
    {
        Debug.Log($"Removed {recipe.label} with on photo {photoId}");

        RecipeUIManager.Instance.HidePhoto(photoId);

        currentRecipes.Remove(recipe);
        RemoveIngredients(recipe);
    }
}

public static class LinqExtras
{
    public static bool ContainsAllItems<T>(this IEnumerable<T> a, IEnumerable<T> b)
    {
        return !b.Except(a).Any();
    }
}
