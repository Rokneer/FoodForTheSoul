using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    private float score = 0;

    [SerializeField]
    private TextMeshProUGUI[] scoreTexts;

    internal void AddScore(RecipeData recipe)
    {
        foreach (TextMeshProUGUI scoreText in scoreTexts)
        {
            score += recipe.points;
            scoreText.text = score.ToString();
        }
    }
}
