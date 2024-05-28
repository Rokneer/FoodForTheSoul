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
    private List<RecipeData> recipes;

    [SerializeField]
    private GameObject recipeUI;

    public List<RecipeData> currentRecipes;
    private int lastIngredientIndex = -1;
    private int lastRecipeIndex = -1;

    private Dictionary<RecipeData, bool> isRecipeDoneDictionary = new();

    [Header("Ingredients")]
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

    public void AddToCurrentIngredients(IngredientData ingredient)
    {
        Debug.Log($"Added {ingredient.objectName} to current recipe");
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
        foreach (IngredientData ingredient in recipe.ingredients)
        {
            if (currentIngredients.Contains(ingredient))
            {
                activeIngredients.Remove(ingredient);
            }
        }
    }

    public IngredientData ChooseRandomIngredient()
    {
        int randomIndex = RandomIndex.GetRandomIndex(
            activeIngredients.ToArray(),
            lastIngredientIndex
        );

        return activeIngredients[randomIndex];
    }

    public RecipeData ChooseRecipe(int customerIndex)
    {
        // Select a random recipe
        int recipeIndex = RandomIndex.GetRandomIndex(recipes.ToArray(), lastRecipeIndex);
        RecipeData selectedRecipe = recipes[recipeIndex];
        selectedRecipe.customerId = customerIndex;

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
            customerIndex
        );

        return selectedRecipe;
    }

    private void CompleteRecipe()
    {
        foreach (RecipeData recipe in currentRecipes.ToArray())
        {
            foreach (IngredientData recipeIngredient in recipe.ingredients)
            {
                isRecipeDoneDictionary[recipe] = currentIngredients.Contains(recipeIngredient);
            }
            if (isRecipeDoneDictionary[recipe])
            {
                Debug.Log($"{recipe.recipeName} is ready to serve!");
                RecipeSpawner.Instance.SpawnCompletedRecipe(recipe);
            }
        }
    }

    public void RemoveRecipe(RecipeData recipe, int photoIndex)
    {
        Debug.Log($"Removed {recipe.recipeName} with on photo {photoIndex}");

        RecipeUIManager.Instance.HidePhoto(photoIndex);

        currentRecipes.Remove(recipe);
        RemoveIngredients(recipe);
    }
}
