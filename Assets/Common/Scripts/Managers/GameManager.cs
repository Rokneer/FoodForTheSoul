using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Tutorial")]
    [SerializeField]
    private TutorialSettings tutorialSettings;
    private bool hasPlayedTutorial;
    public bool HasPlayedTutorial
    {
        get => hasPlayedTutorial;
        set
        {
            hasPlayedTutorial = value;
            tutorialSettings.hasPlayedTutorial = value;
            if (hasPlayedTutorial)
            {
                EndTutorial();
            }
        }
    }

    [SerializeField]
    private GameObject tutorialUI;

    [SerializeField]
    private GameObject[] tutorialTexts;
    private int currentTutorialIndex = 0;
    public int CurrentTutorialIndex
    {
        get => currentTutorialIndex;
        set
        {
            currentTutorialIndex = Mathf.Clamp(value, 0, tutorialTexts.Length);
            if (currentTutorialIndex >= tutorialTexts.Length)
            {
                HasPlayedTutorial = true;
            }
        }
    }

    [SerializeField]
    private GameObject[] interactableAreas;

    [Header("Game Over")]
    [SerializeField]
    private GameObject gameOverUI;

    [Header("Photo Camera")]
    [SerializeField]
    private CameraBattery cameraBattery;

    private List<Customer> Customers =>
        CustomerSpawnManager.Instance.currentCustomers.Keys.ToList();

    private void Awake()
    {
        hasPlayedTutorial = tutorialSettings.hasPlayedTutorial;
    }

    private void Start()
    {
        TimeManager.Instance.isActive = false;

        if (tutorialSettings.skipTutorial || HasPlayedTutorial)
        {
            StartGame();
        }
        else
        {
            StartTutorial();
        }
    }

    private void StartTutorial()
    {
        Debug.Log("Started tutorial");

        foreach (GameObject interactableArea in interactableAreas)
        {
            interactableArea.SetActive(false);
        }

        tutorialUI.SetActive(true);
        tutorialTexts[CurrentTutorialIndex].SetActive(true);

        PlayerController.Instance.isInsideInteractable = true;
        PlayerController.Instance.InteractableAction += ContinueTutorial;
    }

    private void ContinueTutorial()
    {
        CurrentTutorialIndex++;
        if (CurrentTutorialIndex <= tutorialTexts.Length - 1)
        {
            tutorialTexts[CurrentTutorialIndex - 1].SetActive(false);
            tutorialTexts[CurrentTutorialIndex].SetActive(true);
        }
        else
        {
            tutorialTexts[CurrentTutorialIndex - 1].SetActive(false);
        }
    }

    private void EndTutorial()
    {
        Debug.Log("Finished tutorial");

        tutorialUI.SetActive(false);
        PlayerController.Instance.isInsideInteractable = false;
        PlayerController.Instance.InteractableAction -= ContinueTutorial;
        StartGame();
    }

    private void StartGame()
    {
        Debug.Log("Started game");

        foreach (GameObject interactableArea in interactableAreas)
        {
            interactableArea.SetActive(true);
        }

        TimeManager.Instance.isActive = true;

        CustomerSpawnManager.Instance.StartSpawner();
        FoodSpawnManager.Instance.StartSpawner();
    }

    internal void GameOver()
    {
        PauseManager.Instance.PauseGame();
        PauseManager.Instance.ManageMouseVisibility(true);
        gameOverUI.SetActive(true);
    }

    internal void DamageBattery(float damageValue)
    {
        cameraBattery.LowerBattery(damageValue);
        StartCoroutine(cameraBattery.PauseRecharge());
    }

    internal void StunCustomers()
    {
        foreach (Customer customer in Customers)
        {
            StartCoroutine(customer.StunCustomer());
        }
    }
}
