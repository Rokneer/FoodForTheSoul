using System.Collections;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [Header("Recipe")]
    public RecipeData recipe;

    [Header("Stun")]
    [SerializeField]
    private float stunTime;
    public bool isStunned = false;

    [Header("Damage")]
    [SerializeField]
    private float damageValue = 25;

    private TweenMovement tweenMovement;

    private void Awake()
    {
        tweenMovement = GetComponent<TweenMovement>();
    }

    public IEnumerator StunCustomer()
    {
        isStunned = true;

        Debug.Log($"Customer with {recipe.label} was stunned!");

        // Stop tween movement
        tweenMovement.PauseMovement();

        // Stop timer
        RecipeUIManager.Instance.PauseTimer(recipe.customerId);

        //* Start stun particle effect

        yield return new WaitForSeconds(stunTime);

        //* Stop stun particle effect

        // Resume tween movement
        tweenMovement.ResumeMovement();

        // Resume timer
        RecipeUIManager.Instance.ResumeTimer(recipe.customerId);

        isStunned = false;
    }

    public void DoDamage()
    {
        Debug.Log($"Customer with {recipe.label} attacked angrily!");
        GameManager.Instance.DamageBattery(damageValue);
    }
}
