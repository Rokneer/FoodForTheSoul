using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PhotoFlash : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private Light flash;

    [Header("Flash Values")]
    [SerializeField]
    private float flashIntensity;

    [SerializeField]
    private float flashDuration;

    [SerializeField]
    private float flashCooldown;

    [SerializeField]
    private Slider flashCooldownSlider;

    [Space]
    public bool canUseFlash = true;

    [Header("Audio")]
    [SerializeField]
    private AudioClip flashSFX;

    [SerializeField]
    private AudioClip flashRechargeSFX;
    #endregion

    #region Functions
    internal IEnumerator ActivateFlash()
    {
        canUseFlash = false;

        // Update flash UI
        flashCooldownSlider.DOValue(0, flashDuration).SetEase(Ease.Flash);

        // Stun all current customers
        GameManager.Instance.StunCustomers();

        // Play flash SFX
        SoundFXManager.Instance.PlaySoundFXClip(flashSFX, transform, 1);

        // Increase light intensity
        flash.DOIntensity(flashIntensity, flashDuration).SetEase(Ease.OutExpo);

        yield return new WaitForSeconds(flashDuration);

        // Lower light intensity
        flash.DOIntensity(0, 0.2f).SetEase(Ease.Flash);

        // Start flash UI cooldown timer
        flashCooldownSlider.DOValue(1, flashCooldown).SetEase(Ease.OutExpo);
        yield return new WaitForSeconds(flashCooldown);

        // Play flash recharge SFX
        SoundFXManager.Instance.PlaySoundFXClip(flashRechargeSFX, transform, 1);

        canUseFlash = true;
    }
    #endregion
}
