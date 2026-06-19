using UnityEngine;

public class ObjInteract : MonoBehaviour , IInteractable
{
    [Header("Object Settings")]
    [SerializeField] private Type objectType = Type.None;

    // คืนค่า Type ของ Object นี้
    public Type GetInteractType()
    {
        return objectType;
    }

    private void OnTriggerEnter(Collider other)
    {
        // เช็คว่าสิ่งจับชนมีสคริปต์ PlayerInteractor ไหม
        if (other.CompareTag("Player"))
        {
            Debug.Log("In Trigger");
            PlayerInteractor player = other.GetComponent<PlayerInteractor>();
            if (player != null)
            {
                // CORE LOGIC: เช็คเงื่อนไข Type ตรงกัน หรือออบเจกต์เป็น None
                if (objectType == Type.None || objectType == player.GetPlayerType())
                {
                    // เปิด UI


                    // ส่งสัญญาณบอก Player ว่า "ฉันพร้อมให้เธอกด Interact แล้วนะ"
                    player.RegisterInteractable(this);
                }
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Out Trigger");
            PlayerInteractor player = other.GetComponent<PlayerInteractor>();

            if (player != null)
            {
                // ปิด UI เมื่อผู้เล่นเดินออกจากรัศมี

                // บอก Player ให้ลบออบเจกต์นี้ออกจากเป้าหมาย ป้องกันบั๊กกด Interact นอกระยะ
                player.UnregisterInteractable(this);
            }
        }
    }


    public void Interact(Type playerType)
    {
        Debug.Log($"[Success] Player ({playerType}) interacted with {gameObject.name} (Type: {objectType})!");

        Destroy(gameObject);

        // TODO: ใส่ Logic Object เช่น เปิดประตู, เก็บไอเทม, เปิดกลไก ฯลฯของ 
    }
}
