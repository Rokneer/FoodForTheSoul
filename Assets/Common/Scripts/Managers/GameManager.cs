using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Game Over")]
    [SerializeField]
    private GameObject gameOverUI;

    [Header("Photo Camera")]
    [SerializeField]
    private CameraBattery cameraBattery;

    private List<Customer> Customers =>
        CustomerSpawnManager.Instance.currentCustomers.Keys.ToList();

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

    internal void StunCustomers()
    {
        foreach (Customer customer in Customers)
        {
            StartCoroutine(customer.StunCustomer());
        }
    }
}
