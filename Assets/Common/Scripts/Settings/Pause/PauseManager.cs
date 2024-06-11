using UnityEngine;

public class PauseManager : Singleton<PauseManager>
{
    [SerializeField]
    private GameObject pauseMenu;

    internal bool IsPaused { get; set; } = false;
    internal bool canPause = true;

    public void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1;
    }

    public void ManageMouseVisibility(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.Confined : CursorLockMode.Locked;
    }

    public void ManagePauseMenu()
    {
        if (canPause)
        {
            if (IsPaused)
            {
                pauseMenu.SetActive(false);
                ResumeGame();
                ManageMouseVisibility(false);
            }
            else
            {
                pauseMenu.SetActive(true);
                PauseGame();
                ManageMouseVisibility(true);
            }
        }
    }
}
