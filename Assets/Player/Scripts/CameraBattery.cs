using UnityEngine;
using UnityEngine.UI;

public class CameraBattery : MonoBehaviour
{
    [SerializeField]
    private float maxCameraBattery = 100;
    public float MaxCameraBatteryValue
    {
        get => maxCameraBattery;
        set
        {
            maxCameraBattery = value;
            batteryGauge.maxValue = maxCameraBattery;
        }
    }

    [SerializeField]
    private float cameraBattery = 100;
    public float CameraBatteryValue
    {
        get => cameraBattery;
        set
        {
            cameraBattery = Mathf.Clamp(value, 0, maxCameraBattery);
            batteryGauge.value = cameraBattery;

            if (batteryGauge.value == 0)
            {
                //* Call GameManager GameOver() method
                Debug.Log("Out of battery!");
            }
        }
    }

    [SerializeField]
    private Slider batteryGauge;
    private float targetValue = 100;
    private readonly float lerpSpeed = 2f;

    private void Awake()
    {
        batteryGauge.maxValue = maxCameraBattery;
        batteryGauge.value = CameraBatteryValue;
    }

    private void Update()
    {
        CameraBatteryValue = Mathf.Lerp(
            CameraBatteryValue,
            targetValue,
            Time.deltaTime * lerpSpeed
        );
    }

    internal void LowerBattery(float damageValue)
    {
        targetValue -= damageValue;
    }

    internal void IncreaseBattery(float rechargeValue)
    {
        targetValue += rechargeValue;
    }
}
