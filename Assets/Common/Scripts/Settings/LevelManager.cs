using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public void LoadLevel(int ID)
    {
        SceneManager.LoadSceneAsync(ID);
        PauseManager.Instance.ResumeGame();
    }

    public void ResetCurrentLevel()
    {
        PauseManager.Instance.ResumeGame();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
