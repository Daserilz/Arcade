using UnityEngine;

public class ObjInteract : MonoBehaviour , IInteractable
{
    [Header("Object Settings")]
    public bool isOneUse;
    public bool hasInteracted { get; private set; } = false;
    [SerializeField] private Type objectType = Type.None;

    // คืนค่า Type ของ Object นี้
    public Type GetInteractType()
    {
        return objectType;
    }

    protected void OnTriggerEnter(Collider other)
    {
        // เช็คว่าสิ่งจับชนมีสคริปต์ PlayerInteractor ไหม
        if (other.CompareTag("Player"))
        {
            //Debug.Log("In Trigger");
            PlayerInteractor player = other.GetComponent<PlayerInteractor>();
            if (player != null)
            {
                if (isOneUse && hasInteracted) return;
                // CORE LOGIC: เช็คเงื่อนไข Type ตรงกัน หรือออบเจกต์เป็น None
                if (objectType == Type.None || objectType == player.GetPlayerType())
                {
                    // UI open

                    //  Interact แล้วนะ"
                    player.RegisterInteractable(this);
                }
            }
        }
        
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Out Trigger");
            PlayerInteractor player = other.GetComponent<PlayerInteractor>();

            if (player != null )
            {
                // UI close

                // บPlayer ให้ลบออบเจกต์นี้ออกจากเป้าหมาย ป้องกันบั๊กกด Interact นอกระยะ
                player.UnregisterInteractable(this);
            }
        }
    }


    public virtual void Interact(Type playerType)
    {
        Debug.Log($"[Success] Player ({playerType}) interacted with {gameObject.name} (Type: {objectType})!");
        if (playerType == Type.Mechanism) GameManager.Instance.addScoreMechanism();
        else if (playerType == Type.Creative) GameManager.Instance.addScoreCreative();

        if (isOneUse)
        {
            hasInteracted = true;
        }

        // TODO: ใส่ Logic Object เช่น เปิดประตู, เก็บไอเทม, เปิดกลไก ฯลฯของ 
    }
}
