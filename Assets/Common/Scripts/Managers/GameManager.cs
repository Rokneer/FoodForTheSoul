using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [Header("Photo Camera")]
    [SerializeField]
    private CameraBattery cameraBattery;

    [Header("Customers")]
    [SerializeField]
    private List<Customer> currentCustomers;

    [Header("Spawners")]
    [SerializeField]
    private GameObject spawnManager;
    private FoodSpawnManager foodSpawnManager;
    private CreatureSpawnManager creatureSpawnManager;
    private CustomerSpawnManager customerSpawnManager;

    private void Awake()
    {
        // Checks if there is only one instance of the script in the scene
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        foodSpawnManager = spawnManager.GetComponent<FoodSpawnManager>();
        creatureSpawnManager = spawnManager.GetComponent<CreatureSpawnManager>();
        customerSpawnManager = spawnManager.GetComponent<CustomerSpawnManager>();
    }

    private void DisableSpawner(Spawner spawner)
    {
        spawner.isActive = false;
    }

    public void DamageBattery(float damageValue)
    {
        cameraBattery.LowerBattery(damageValue);
    }

    public void StunCustomers()
    {
        foreach (Customer customer in currentCustomers)
        {
            StartCoroutine(customer.StunCustomer());
        }
    }
}
