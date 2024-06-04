using System.Collections;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public int id;

    [Header("Recipe")]
    public RecipeData recipe;

    [Header("Stun")]
    [SerializeField]
    private float stunTime;
    public bool isStunned = false;

    [Header("Damage")]
    [SerializeField]
    private float damageValue = 25;

    public TweenMovement movement;

    private void Awake()
    {
        movement = GetComponent<TweenMovement>();
    }

    public IEnumerator StunCustomer()
    {
        isStunned = true;

        Debug.Log($"Customer with {recipe.label} was stunned for {stunTime} seconds!");

        // Stop tween movement
        movement.PauseMovement();

        // Stop timer
        RecipeUIManager.Instance.PauseTimer(id);

        //* Start stun particle effect

        yield return new WaitForSeconds(stunTime);

        //* Stop stun particle effect

        // Resume tween movement
        movement.ResumeMovement();

        // Resume timer
        RecipeUIManager.Instance.ResumeTimer(id);

        isStunned = false;
    }

    public void DoDamage()
    {
        Debug.Log($"Customer with {recipe.label} attacked angrily!");
        GameManager.Instance.DamageBattery(damageValue);
    }
}
