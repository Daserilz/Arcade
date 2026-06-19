using UnityEngine;

public interface IInteractable
{
    Type GetInteractType();
    void Interact(Type playerType);
}
