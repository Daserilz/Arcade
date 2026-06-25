using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private Type myPlayerType;

    [SerializeField] private ObjInteract currentTarget;

    public Type GetPlayerType()
    {
        return myPlayerType;
    }

    public void OnInteractEvent(InputAction.CallbackContext context)
    {
        // context.performed จะเช็คว่าเป็นการ "กดลงไป" (ป้องกันการทำงานซ้ำตอนปล่อยปุ่ม)
        if (context.performed)
        {
            Debug.Log($"<color=cyan> input keyboard {context} success</color>");
            if (currentTarget != null)
            {
                if (currentTarget.isOneUse && currentTarget.hasInteracted) return;
                currentTarget.Interact(myPlayerType);
            }
        }
    }

    //void Update()
    //{
    //    // ¶éÒ¡´ E áÅÐÁÕà»éÒËÁÒÂÍÂÙèã¡Åéæ
    //    if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
    //    {
    //        Debug.Log("Click E");
    //        if (currentTarget != null)
    //        {
    //            if (currentTarget.isOneUse && currentTarget.hasInteracted) return;
    //            currentTarget.Interact(myPlayerType);
    //        }
    //    }
    //}


    // àÁ×èÍà´Ô¹à¢éÒä»ã¹ÃÑÈÁÕ¢Í§ Object

    public void RegisterInteractable(ObjInteract target)
    {
        currentTarget = target;
    }

    // ÍÍºà¨¡µìã¹ÃÑÈÁÕ¨ÐàÃÕÂ¡ãªé¿Ñ§¡ìªÑ¹¹Õéà¾×èÍÂ¡àÅÔ¡à»éÒËÁÒÂ
    public void UnregisterInteractable(ObjInteract target)
    {
        if (currentTarget == target)
        {
            currentTarget = null;
        }
    }
}
