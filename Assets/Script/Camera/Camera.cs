using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Players")]
    public Transform player1;
    public Transform player2;

    [Header("Camera Settings")]
    public float smoothSpeed = 5f;
    public float minFOV = 40f;        // FOV ตอนยืนชิดกัน
    public float maxFOV = 60f;        // FOV ตอนยืนห่างกัน
    public float zoomLimiter = 10f;

    public Vector3 offset = new Vector3(0f, 10f, 0f);


    [Header("Room Boundary Limits ")]
    public bool useBounds = true;
    public float minX = -10f;
    public float maxX = 10f;
    public float minZ = -10f; // ถ้าเป็นเกม 2D ให้เปลี่ยนจาก Z เป็น Y
    public float maxZ = 10f;

    [Header("Camera Proximity FOV ")]
    public bool enableProximityFOV = true;
    public float proximityThreshold = 8f; // ระยะห่างจากกล้องที่จะเริ่มเปิดใช้งานระบบนี้ (หน่วยเมตร)
    public float closeToCameraFOV = 75f; //FOV สูงสุดเมื่อผู้เล่นเดินเข้ามาประชิดกล้องมากที่สุด


    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        bool isPlayer1Alive = IsPlayerAlive(player1);
        bool isPlayer2Alive = IsPlayerAlive(player2);

        if (!isPlayer1Alive && !isPlayer2Alive) return;

        Vector3 targetPosition;
        float targetFOV;

        // 1. คำนวณตำแหน่งและค่า FOV ตามระยะห่างของผู้เล่น
        if (isPlayer1Alive && isPlayer2Alive)
        {
            Vector3 centerPoint = (player1.position + player2.position) / 2f;
            targetPosition = centerPoint + offset;

            float distance = Vector3.Distance(player1.position, player2.position);
            targetFOV = Mathf.Lerp(minFOV, maxFOV, distance / zoomLimiter);
        }
        else if (isPlayer1Alive)
        {
            targetPosition = player1.position + offset;
            targetFOV = minFOV;
        }
        else
        {
            targetPosition = player2.position + offset;
            targetFOV = minFOV;
        }

        // 2. จำกัดตำแหน่งกล้องให้อยู่ในขอบเขตห้อง (Clamp Boundary)
        if (useBounds)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ);
        }

        // 3. คำนวณการปรับ FOV เมื่อเข้าใกล้ขอบห้อง (Edge FOV)
        if (enableProximityFOV)
        {
            float closestDistanceToCam = GetClosestPlayerDistanceToCamera(isPlayer1Alive, isPlayer2Alive);

            // ถ้าระยะห่างน้อยกว่าค่าที่กำหนด (ยิ่งใกล้ยิ่งค่าน้อย)
            if (closestDistanceToCam < proximityThreshold)
            {
                // แปลงระยะห่างเป็นเปอร์เซ็นต์ (t = 1 คือชิดกล้องสุดๆ, t = 0 คืออยู่ไกลเกินระยะ Threshold)
                float t = 1f - Mathf.Clamp01(closestDistanceToCam / proximityThreshold);

                // ค่อยๆ ปรับเพิ่มค่า FOV จากค่าเป้าหมายเดิม ไปหา closeToCameraFOV
                targetFOV = Mathf.Lerp(targetFOV, closeToCameraFOV, t);
            }
        }

        // 4. เคลื่อนที่กล้องและปรับ FOV อย่างนุ่มนวล
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, smoothSpeed * Time.deltaTime);
    }

    private float GetClosestPlayerDistanceToCamera(bool p1Alive, bool p2Alive)
    {
        float dist1 = p1Alive ? Vector3.Distance(transform.position, player1.position) : float.MaxValue;
        float dist2 = p2Alive ? Vector3.Distance(transform.position, player2.position) : float.MaxValue;

        return Mathf.Min(dist1, dist2);
    }

    private bool IsPlayerAlive(Transform player)
    {
        return player != null && player.gameObject.activeInHierarchy;
    }

    private void OnDrawGizmosSelected()
    {
        if (!useBounds) return;

        Gizmos.color = Color.yellow;
        // วาดกรอบสี่เหลี่ยมตามค่า minX, maxX, minZ, maxZ
        Vector3 center = new Vector3((minX + maxX) / 2f, 0f, (minZ + maxZ) / 2f);
        Vector3 size = new Vector3(maxX - minX, 1f, maxZ - minZ);
        Gizmos.DrawWireCube(center, size);
    }
}
