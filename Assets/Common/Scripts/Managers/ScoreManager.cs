using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    private float score = 0;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    internal void AddScore(RecipeData recipe)
    {
        score += recipe.points;
        scoreText.text = score.ToString();
    }
}
