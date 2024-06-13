using System.Collections;
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
                PauseManager.Instance.canPause = false;
                GameManager.Instance.GameOver();
            }
        }
    }

    [SerializeField]
    private float rechargeValue = 1;

    [SerializeField]
    private float rechargeDelay = 3;
    internal bool canRecharge = true;

    [SerializeField]
    private float damageDelay = 7;

    [SerializeField]
    private Slider batteryGauge;
    private readonly float tweenDuration = 0.2f;

    private void Awake()
    {
        batteryGauge.maxValue = maxBattery;
        batteryGauge.value = Battery;
    }

    private void Start()
    {
        InvokeRepeating(nameof(RechargeBattery), 0, rechargeDelay);
    }

    private void RechargeBattery()
    {
        if (canRecharge && Battery < MaxBattery)
        {
            IncreaseBattery(rechargeValue);
        }
    }

    internal IEnumerator PauseRecharge()
    {
        canRecharge = false;
        yield return new WaitForSeconds(damageDelay);
        canRecharge = true;
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
