using UnityEngine;

[CreateAssetMenu(menuName = "FoodForTheSoul/TutorialSettings")]
public class TutorialSettings : ScriptableObject
{
    [Header("Tutorial")]
    public bool skipTutorial = false;
    public bool hasPlayedTutorial = false;
}
