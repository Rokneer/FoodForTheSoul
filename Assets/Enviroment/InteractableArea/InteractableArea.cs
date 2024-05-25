using UnityEngine;

public class InteractableArea : MonoBehaviour
{
    [SerializeField]
    protected bool isInsideTrigger = false;

    protected virtual void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(TagStrings.Player))
        {
            isInsideTrigger = true;
        }
    }

    protected virtual void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag(TagStrings.Player))
        {
            isInsideTrigger = false;
        }
    }
}
