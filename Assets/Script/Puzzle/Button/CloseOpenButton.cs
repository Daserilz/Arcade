using UnityEngine;
using UnityEngine.Events;

public class CloseOpenButton : ObjInteract
{
    public GameObject targetObject;
    public UnityEvent interactEvent;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public override void Interact(Type playerType)
    {
        base.Interact(playerType);
        if (targetObject == null) return;
        targetObject.SetActive(!targetObject.activeSelf);
        interactEvent.Invoke();
    }
}
