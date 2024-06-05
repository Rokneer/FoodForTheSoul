using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CameraBattery : MonoBehaviour
{
    [SerializeField]
    private float maxBattery = 100;
    public float MaxBattery
    {
        get => maxBattery;
        set
        {
            maxBattery = value;
            batteryGauge.maxValue = maxBattery;
        }
    }

    [SerializeField]
    private float battery = 100;
    public float Battery
    {
        get => battery;
        set
        {
            battery = Mathf.Clamp(value, 0, maxBattery);
            batteryGauge.value = battery;

            if (battery == 0)
            {
                Debug.Log("Out of battery!");
                PlayerController.Instance.canPause = false;
                GameManager.Instance.GameOver();
            }
        }
    }

    [SerializeField]
    private Slider batteryGauge;
    private readonly float tweenDuration = 0.2f;

    private void Awake()
    {
        batteryGauge.maxValue = maxBattery;
        batteryGauge.value = Battery;
    }

    internal void LowerBattery(float damageValue)
    {
        ChangeBatteryValue(-damageValue);
    }

    internal void IncreaseBattery(float rechargeValue)
    {
        ChangeBatteryValue(rechargeValue);
    }

    internal void ChangeBatteryValue(float value)
    {
        DOTween
            .To(() => Battery, value => Battery = value, Battery + value, tweenDuration)
            .SetEase(Ease.OutExpo);
    }
}
