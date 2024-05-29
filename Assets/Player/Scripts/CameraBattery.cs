using UnityEngine;
using UnityEngine.UI;

public class CameraBattery : MonoBehaviour
{
    [SerializeField]
    private float cameraBattery = 90;
    public float CameraBatteryValue
    {
        get => cameraBattery;
        set
        {
            cameraBattery = value;
            batteryGauge.value = cameraBattery;
        }
    }

    private float targetValue = 90;

    [SerializeField]
    private Slider batteryGauge;
    private readonly float lerpSpeed = 2f;

    private void Awake()
    {
        batteryGauge.maxValue = cameraBattery;
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
}
