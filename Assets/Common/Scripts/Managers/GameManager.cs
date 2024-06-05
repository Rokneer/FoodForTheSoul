using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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

    private void DisableSpawner(Spawner spawner)
    {
        spawner.isActive = false;
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
