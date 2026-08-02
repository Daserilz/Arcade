using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Players")]
    public Transform player1;
    public Transform player2;

    [Header("Camera Settings")]
    public float smoothSpeed = 5f;
    public float minZoom = 5f;
    public float maxZoom = 20f;
    public float zoomLimiter = 10f;

    public Vector3 offset = new Vector3(0f, 10f, 0f);


    [Header("Room Boundary Limits ")]
    public bool useBounds = true;
    public float minX = -10f;
    public float maxX = 10f;
    public float minZ = -10f; // ถ้าเป็นเกม 2D ให้เปลี่ยนจาก Z เป็น Y
    public float maxZ = 10f;

    [Header("Edge Rotation Settings ")]
    public bool enableEdgeRotation = true;
    public float defaultAngleX = 60f;     // มุมก้มปกติของกล้อง (เช่น 60 องศา)
    public float minZEdgeAngleX = 45f;    // มุมเมื่อชิดขอบล่าง (เงยกล้องขึ้น มองเข้าห้อง)
    public float maxZEdgeAngleX = 75f;    // มุมเมื่อชิดขอบบน (ก้มกล้องลง ไม่ให้ทะลุเพดาน)
    public float edgeThreshold = 3f;


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
        float targetZoom;

        if (isPlayer1Alive && isPlayer2Alive)
        {
            // กรณีอยู่ครบทั้งสองคน: หาจุดกึ่งกลางและคำนวณระยะซูมตามห่าง
            Vector3 centerPoint = (player1.position + player2.position) / 2f;
            targetPosition = centerPoint + offset;

            float distance = Vector3.Distance(player1.position, player2.position);
            targetZoom = Mathf.Lerp(minZoom, maxZoom, distance / zoomLimiter);
        }
        else if (isPlayer1Alive)
        {
            // กรณีเหลือแค่ Player 1
            targetPosition = player1.position + offset;
            targetZoom = minZoom;
        }
        else
        {
            // กรณีเหลือแค่ Player 2
            targetPosition = player2.position + offset;
            targetZoom = minZoom;
        }

        if (useBounds)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ);
            // หมายเหตุ: ถ้าเป็นเกม Top-Down แบบ 2D ให้เปลี่ยนบรรทัดบนเป็น targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
        }
        // 3. คำนวณการปรับองศากล้องเมื่อเข้าใกล้ขอบ (Edge Rotation)
        float targetAngleX = defaultAngleX;

        if (enableEdgeRotation && useBounds)
        {
            // เช็คระยะใกล้ขอบล่าง (minZ)
            if (targetPosition.z - minZ < edgeThreshold)
            {
                // แปลงค่าความใกล้ให้เป็นเปอร์เซ็นต์ 0 ถึง 1 (1 คือชิดขอบสุด)
                float t = 1f - Mathf.Clamp01((targetPosition.z - minZ) / edgeThreshold);
                targetAngleX = Mathf.Lerp(defaultAngleX, minZEdgeAngleX, t);
            }
            // เช็คระยะใกล้ขอบบน (maxZ)
            else if (maxZ - targetPosition.z < edgeThreshold)
            {
                float t = 1f - Mathf.Clamp01((maxZ - targetPosition.z) / edgeThreshold);
                targetAngleX = Mathf.Lerp(defaultAngleX, maxZEdgeAngleX, t);
            }
        }

        // เคลื่อนที่กล้องไปยังตำแหน่งเป้าหมายอย่างนุ่มนวล
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, smoothSpeed * Time.deltaTime);      // ปรับซูมกล้อง

        Quaternion targetRotation = Quaternion.Euler(targetAngleX, transform.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
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
