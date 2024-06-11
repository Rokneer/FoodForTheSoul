using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : Singleton<LevelLoader>
{
    private Animator transition;

    private void Awake()
    {
        transition = GetComponent<Animator>();
    }

    public void LoadLevel(int id)
    {
        StartCoroutine(LoadLevelTransition(id));
    }

    private IEnumerator LoadLevelTransition(int id)
    {
        transition.SetTrigger(TransitionStrings.StartTrigger);

        yield return new WaitForSeconds(transition.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        SceneManager.LoadSceneAsync(id);
        PauseManager.Instance.ResumeGame();
    }

    public void ResetCurrentLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        PauseManager.Instance.ResumeGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

public class TransitionStrings
{
    internal static readonly string StartTrigger = "Start";
}
