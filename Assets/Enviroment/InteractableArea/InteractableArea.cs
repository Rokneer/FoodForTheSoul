using UnityEngine;

public abstract class InteractableArea : MonoBehaviour
{
    [SerializeField]
    private GameObject interactHint;

    protected virtual bool CanShowHint => true;

    protected virtual void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag(TagStrings.Player))
        {
            PlayerController player = collider.gameObject.GetComponentInParent<PlayerController>();

            player.isInsideInteractable = true;
            player.InteractableAction += Interact;

            interactHint.SetActive(CanShowHint && player.isInsideInteractable);
        }
    }

    protected virtual void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag(TagStrings.Player))
        {
            PlayerController player = collider.gameObject.GetComponentInParent<PlayerController>();

            player.isInsideInteractable = false;
            player.InteractableAction -= Interact;

            interactHint.SetActive(CanShowHint && player.isInsideInteractable);
        }
    }

    protected abstract void Interact();
}
