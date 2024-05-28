using UnityEngine;

public abstract class InteractableArea : MonoBehaviour
{
    protected virtual void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag(TagStrings.Player))
        {
            PlayerController player = collider.gameObject.GetComponentInParent<PlayerController>();

            player.isInsideInteractable = true;
            player.InteractableAction += Interact;
        }
    }

    protected virtual void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag(TagStrings.Player))
        {
            PlayerController player = collider.gameObject.GetComponentInParent<PlayerController>();

            player.isInsideInteractable = false;
            player.InteractableAction -= Interact;
        }
    }

    protected abstract void Interact();
}
