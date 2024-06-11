using System.Collections;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [Header("Data")]
    internal int id;
    internal bool isActive;

    [SerializeField]
    private GameObject skin;

    [Header("Recipe")]
    [SerializeField]
    internal RecipeData recipe;

    [Header("Stun")]
    [SerializeField]
    private float stunTime;
    public bool isStunned = false;

    [Header("Damage")]
    [SerializeField]
    private float damageValue = 25;

    [SerializeField]
    internal TweenMovement movement;

    [Header("Audio")]
    private bool canGroan = true;

    [SerializeField]
    private AudioClip[] groanSounds;

    [SerializeField]
    private AudioClip[] damageSounds;

    [SerializeField]
    internal AudioClip[] eatingSounds;

    [SerializeField]
    private AudioClip stunSound;

    private void Awake()
    {
        movement = GetComponent<TweenMovement>();
    }

    private void Update()
    {
        if (isActive)
        {
            StartCoroutine(PlayGroanSFX());
        }
    }

    internal IEnumerator StunCustomer()
    {
        isStunned = true;

        Debug.Log($"Customer with {recipe.label} was stunned for {stunTime} seconds!");

        // Stop tween movement
        movement.PauseMovement();

        // Stop timer
        RecipeUIManager.Instance.PauseTimer(id);

        // Play sfx
        SoundFXManager.Instance.PlaySFXClip(stunSound, transform, 0.4f, true);

        yield return new WaitForSeconds(stunTime);

        // Resume tween movement
        movement.ResumeMovement();

        // Resume timer
        RecipeUIManager.Instance.ResumeTimer(id);

        isStunned = false;
    }

    internal IEnumerator PlayGroanSFX()
    {
        if (canGroan)
        {
            AudioClip groanClip = SoundFXManager
                .Instance
                .PlayRandomSFXClip(groanSounds, transform, 0.3f, true);

            canGroan = false;
            yield return new WaitForSeconds(groanClip.length + 3f);
            canGroan = true;
        }
    }

    internal void DoDamage()
    {
        Debug.Log($"Customer with {recipe.label} attacked angrily!");
        SoundFXManager.Instance.PlayRandomSFXClip(damageSounds, transform, 0.5f, true);
        GameManager.Instance.DamageBattery(damageValue);
    }
}
