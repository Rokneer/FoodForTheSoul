using System.Collections;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [Header("Recipe")]
    public Recipe recipe;

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

        //* Stop tween movement
        tweenMovement.StopMovement();

        //* Start stun particle effect

        yield return new WaitForSeconds(stunTime);

        //* Stop stun particle effect

        //* Continue tween movement
        tweenMovement.StartMovement();

        isStunned = false;
    }

    public void DoDamage()
    {
        Debug.Log($"Customer with {recipe.recipeName} attacked angrily!");
    }
}
