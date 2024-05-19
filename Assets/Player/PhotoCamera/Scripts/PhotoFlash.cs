using System.Collections;
using UnityEngine;

public class PhotoFlash : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private Light flash;
    private float targetIntensity = 0;

    private readonly float lerpSpeed = 20f;

    [Header("Flash Timers")]
    [SerializeField]
    private float flashDuration;

    [SerializeField]
    private float flashDelay;

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
        flash.intensity = Mathf.Lerp(flash.intensity, targetIntensity, Time.deltaTime * lerpSpeed);
    }
    #endregion Lifecycle

    #region Functions
    internal IEnumerator ActivateFlash()
    {
        canUseFlash = false;
        SoundFXManager.Instance.PlaySoundFXClip(flashSFX, transform, 1);
        targetIntensity = 4;

        yield return new WaitForSeconds(flashDuration);

        targetIntensity = 0;

        yield return new WaitForSeconds(flashDelay);

        SoundFXManager.Instance.PlaySoundFXClip(flashRechargeSFX, transform, 1);
        canUseFlash = true;
    }
    #endregion Functions
}
