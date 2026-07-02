using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private Type myPlayerType;

    [Header("Transform Settings")]
    [SerializeField] private GameObject originalModel;
    [SerializeField] private GameObject mechanismPrefab;
    [SerializeField] private GameObject creativePrefab;



    private ObjInteract currentTarget;

    private Type originalPlayerType;      // ใช้จำว่าก่อนแปลงร่าง ตัวละครอยู่ Type ไหน
    private GameObject tempCharacterInstance; // เก็บอ้างอิงร่าง Prefab ที่ถูกสร้างขึ้นมาชั่วคราว
    private Coroutine transformCoroutine;

    private void Start()
    {
        originalPlayerType = myPlayerType;
    }

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

    // ========================================================
    // ---------------- Swap Character -------------------
    // ========================================================
    public void SwitchForm(float duration)
    {
        Type targetType = Type.None;

        // เช็คเงื่อนไข: ถ้าปัจจุบันเป็น Creative ให้เปลี่ยนเป็น Mechanism
        if (myPlayerType == Type.Creative)
        {
            targetType = Type.Mechanism;
        }
        // เช็คเงื่อนไข: ถ้าปัจจุบันเป็น Mechanism ให้เปลี่ยนเป็น Creative
        else if (myPlayerType == Type.Mechanism)
        {
            targetType = Type.Creative;
        }

        // หากค่าไม่ตรงเงื่อนไข (เช่น เป็น None) ให้ยกเลิกการทำงาน
        if (targetType == Type.None) return;

        // หากผู้เล่นกดสลับร่างซ้ำในขณะที่เวลากำลังนับถอยหลัง 30 วิของร่างก่อนหน้าอยู่
        // ให้หยุดเวลานั้นทิ้งไป แล้วทำการรีเซ็ตเริ่มต้นนับ 30 วิใหม่ทันที
        if (transformCoroutine != null)
        {
            StopCoroutine(transformCoroutine);
        }

        transformCoroutine = StartCoroutine(TransformationRoutine(targetType , duration));
    }

    private IEnumerator TransformationRoutine(Type targetType , float duration)
    {
        // 1. เคลียร์ร่างเก่าทิ้งก่อน (ถ้ามีค้างอยู่)
        if (tempCharacterInstance != null)
        {
            Destroy(tempCharacterInstance);
        }

        // 2. ซ่อนโมเดลร่างปกติ และอัปเดต Type ปัจจุบันไปเป็นร่างใหม่
        myPlayerType = targetType;
        if (originalModel != null) originalModel.SetActive(false);

        // 3. ตรวจสอบเงื่อนไขเพื่อดึง Prefab ร่างใหม่มา spawn
        GameObject prefabToSpawn = null;
        if (targetType == Type.Mechanism)
        {
            prefabToSpawn = mechanismPrefab;
        }
        else if (targetType == Type.Creative)
        {
            prefabToSpawn = creativePrefab;
        }

        // 4. สั่งสร้างโมเดลร่างแปลงขึ้นมา ณ ตำแหน่งตัวละคร
        if (prefabToSpawn != null)
        {
            tempCharacterInstance = Instantiate(prefabToSpawn, transform.position, transform.rotation);
            tempCharacterInstance.transform.SetParent(this.transform); // ติดตามตัวละครหลัก
        }

        Debug.Log($"<color=yellow>แปลงร่างสลับไปเป็น Type: {myPlayerType} ชั่วคราว!</color>");

        // 5. จับเวลาหน่วง 30 วินาที
        yield return new WaitForSeconds(duration);

        // 6. เมื่อครบเวลา เรียกฟังก์ชันคืนร่างเดิมตามที่บันทึกไว้ใน Start()
        RevertToNormal();
    }

    private void RevertToNormal()
    {
        // ทำลายโมเดลร่างแปลงชั่วคราวทิ้ง
        if (tempCharacterInstance != null)
        {
            Destroy(tempCharacterInstance);
        }

        // แสดงผลโมเดลร่างแรกเริ่มกลับมาตามเดิม
        if (originalModel != null) originalModel.SetActive(true);

        // คืนค่า Type ของตัวละครกลับไปเป็นค่าเริ่มต้นของ Scene นั้นๆ
        myPlayerType = originalPlayerType;

        transformCoroutine = null; // เคลียร์สถานะตัวจับเวลา
        Debug.Log($"<color=orange>ครบเวลา คืนร่างเดิมสำเร็จ! Type: {myPlayerType}</color>");
    }

}
