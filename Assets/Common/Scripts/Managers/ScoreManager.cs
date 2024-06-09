using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private float score = 0;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    internal void AddScore(RecipeData recipe)
    {
        score += recipe.points;
        scoreText.text = score.ToString();
    }
}
