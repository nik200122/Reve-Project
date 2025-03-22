using UnityEngine;

public interface IInteractable
{
    void Interact(Transform interactorTransform);
    void TerminateInteract();
    string GetInteractText();
    Transform GetTransform();
}
