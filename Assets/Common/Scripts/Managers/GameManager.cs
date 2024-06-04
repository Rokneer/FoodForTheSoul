using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [Header("Photo Camera")]
    [SerializeField]
    private CameraBattery cameraBattery;

    private List<Customer> Customers =>
        CustomerSpawnManager.Instance.currentCustomers.Keys.ToList();

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
        foreach (Customer customer in Customers)
        {
            StartCoroutine(customer.StunCustomer());
        }
    }
}
