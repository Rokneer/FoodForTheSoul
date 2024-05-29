using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhotoFlash : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private Light flash;
    private float targetIntensity = 0;
    private float targetSliderValue = 1;

    private readonly float intensitylerpSpeed = 20f;
    private float sliderLerpSpeed = 20f;

    [Header("Flash Timers")]
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
    #endregion Variables

    #region Lifecycle
    private void Update()
    {
        flash.intensity = Mathf.Lerp(
            flash.intensity,
            targetIntensity,
            Time.deltaTime * intensitylerpSpeed
        );

        flashCooldownSlider.value = Mathf.Lerp(
            flashCooldownSlider.value,
            targetSliderValue,
            Time.deltaTime * sliderLerpSpeed
        );
    }
    #endregion Lifecycle

    #region Functions
    internal IEnumerator ActivateFlash()
    {
        canUseFlash = false;

        // Update flash UI
        targetSliderValue = 0;

        //* Stun enemies with StunCustomer()
        //? Maybe use a GameManager event to trigger it in all current enemies

        SoundFXManager.Instance.PlaySoundFXClip(flashSFX, transform, 1);
        targetIntensity = 4;

        yield return new WaitForSeconds(flashDuration);

        targetIntensity = 0;

        // Start flash UI cooldown timer
        sliderLerpSpeed = flashCooldown;
        targetSliderValue = 1;
        yield return new WaitForSeconds(flashCooldown);

        SoundFXManager.Instance.PlaySoundFXClip(flashRechargeSFX, transform, 1);
        sliderLerpSpeed = 20f;
        canUseFlash = true;
    }
    #endregion Functions
}
