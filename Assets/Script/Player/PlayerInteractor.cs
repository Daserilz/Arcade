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

    void Update()
    {
        // ถ้ากด E และมีเป้าหมายอยู่ใกล้ๆ
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("Click E");
            if (currentTarget != null)
            {
                if (currentTarget.isOneUse && currentTarget.hasInteracted) return;
                currentTarget.Interact(myPlayerType);
            }
        }
    }


    // เมื่อเดินเข้าไปในรัศมีของ Object

    public void RegisterInteractable(ObjInteract target)
    {
        currentTarget = target;
    }

    // ออบเจกต์ในรัศมีจะเรียกใช้ฟังก์ชันนี้เพื่อยกเลิกเป้าหมาย
    public void UnregisterInteractable(ObjInteract target)
    {
        if (currentTarget == target)
        {
            currentTarget = null;
        }
    }
}
