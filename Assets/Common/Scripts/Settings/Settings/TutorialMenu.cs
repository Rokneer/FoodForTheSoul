using UnityEngine;

public class TutorialMenu : MonoBehaviour
{
    [SerializeField]
    private TutorialSettings tutorialSettings;

    public void PlayTutorial()
    {
        tutorialSettings.skipTutorial = false;
    }

    public void SkipTutorial()
    {
        tutorialSettings.skipTutorial = true;
    }
}
