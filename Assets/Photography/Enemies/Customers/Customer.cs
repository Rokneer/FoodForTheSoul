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

    private TweenMovement tweenMovement;

    private void Awake()
    {
        tweenMovement = GetComponent<TweenMovement>();
    }

    public IEnumerator StunCustomer()
    {
        isStunned = true;

        Debug.Log($"Customer with {recipe.recipeName} was stunned!");

        // Stop tween movement
        tweenMovement.PauseMovement();

        //* Start stun particle effect

        yield return new WaitForSeconds(stunTime);

        //* Stop stun particle effect

        // Continue tween movement
        tweenMovement.ResumeMovement();

        isStunned = false;
    }

    public void DoDamage()
    {
        Debug.Log($"Customer with {recipe.recipeName} attacked angrily!");
    }
}
