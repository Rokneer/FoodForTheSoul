using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
        // Checks if there is only one instance of the script in the scene
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(CustomerSpawnManager.Instance.SpawnCustomer());

        StartCoroutine(FoodSpawnManager.Instance.SpawnFoodInPath());
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
    }

    private void DisableSpawner(Spawner spawner)
    {
        spawner.isActive = false;
    }

    internal void StunCustomers()
    {
        foreach (Customer customer in Customers)
        {
            StartCoroutine(customer.StunCustomer());
        }
    }
}
