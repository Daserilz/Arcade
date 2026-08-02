using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // ดึงกล้องหลักของฉากมาใช้งานอัตโนมัติ
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera == null) return;

        // วิธีที่ 1: หันหน้าตามองศาการหมุนของกล้องเป๊ะๆ (เหมาะกับกล้อง Top-Down / 3D มากที่สุด)
        transform.rotation = mainCamera.transform.rotation;

        // --- หรือถ้าใช้ วิธีที่ 1 แล้ว Sprite กลับด้าน/หันหลัง ให้คอมเมนต์บรรทัดบน แล้วใช้บรรทัดล่างแทน ---
        // transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }
}
