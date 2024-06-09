using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    [SerializeField]
    private GameObject pauseMenu;

    public bool IsPaused { get; set; } = false;

    public bool canPause = true;

    private void Awake()
    {
        IsPaused = false;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

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
