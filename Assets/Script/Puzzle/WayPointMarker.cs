using UnityEngine;
using UnityEngine.UI;

public class WayPointMarker : MonoBehaviour
{
    [Header("References")]
    public Transform target;
    public Image markerUI;
    public Camera playerCamera;
    [Header("Settings")]
    public Vector3 offset = new Vector3(0, 2f, 0);
    public float edgePadding = 30f; // ระยะห่างไม่ให้ไอคอนชิดขอบจอเกินไป
    public bool hideWhenClose = false; // ซ่อนเมื่ออยู่ใกล้เกินไปหรือไม่?
    public float minDistance = 5f;

    [Header("State")]
    public bool isMarkerActive = true;

    private Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (markerUI != null) markerUI.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMarkerActive)
        {
            if (markerUI != null && markerUI.enabled) markerUI.enabled = false;
            return;
        }

        if (target != null && target.gameObject.activeInHierarchy)
        {
            // แปลงพิกัดทางออกจาก 3D เป็นพิกัด 2D บนหน้าจอ
            Vector3 targetPos = target.position + offset;

            // ออปชันเสริม: ซ่อนมาร์คเกอร์ถ้าผู้เล่นเดินมาใกล้ทางออกแล้ว
            if (hideWhenClose && Vector3.Distance(playerCamera.transform.position, targetPos) < minDistance)
            {
                markerUI.enabled = false;
                return;
            }

            // แปลงพิกัดทางออกเทียบกับมุมมองของกล้องตัวนี้
            Vector3 screenPos = playerCamera.WorldToScreenPoint(targetPos);


            // เช็คว่าเป้าหมายอยู่หลังกล้องหรือไม่ (ถ้า Z ติดลบ แปลว่าหันหลังให้)
            bool isBehind = screenPos.z < 0;

            // ดึงกรอบพื้นที่หน้าจอของกล้องตัวนี้ (รองรับ Split-screen อัตโนมัติ)
            Rect camRect = playerCamera.pixelRect;
            Vector2 screenCenter = new Vector2(camRect.center.x, camRect.center.y);

            // เช็คว่าพิกัดปัจจุบันหลุดออกนอกกรอบจอของกล้องนี้ไปหรือยัง
            bool isOffScreen = isBehind ||
                               screenPos.x <= camRect.xMin || screenPos.x >= camRect.xMax ||
                               screenPos.y <= camRect.yMin || screenPos.y >= camRect.yMax;

            if (isOffScreen)
            {
                // หาเวกเตอร์ทิศทางจากกึ่งกลางจอไปยังเป้าหมาย
                Vector2 direction = new Vector2(screenPos.x, screenPos.y) - screenCenter;

                // ถ้าหันหลังให้เป้าหมาย ทิศทางบนหน้าจอจะกลับด้าน ต้องคูณ -1
                if (isBehind)
                {
                    direction = -direction;
                }
                direction.Normalize();

                // คำนวณขอบจอที่ยอมให้ไอคอนอยู่ได้ (หักลบ Padding)
                float boundsX = (camRect.width / 2f) - edgePadding;
                float boundsY = (camRect.height / 2f) - edgePadding;

                // หาจุดตัดขอบจอ (ใช้เรขาคณิตความชัน)
                float angle = Mathf.Atan2(direction.y, direction.x);
                float m = Mathf.Tan(angle);

                float clampedX, clampedY;

                // เช็คว่าจะชนขอบแนวตั้ง (บน/ล่าง) หรือแนวนอน (ซ้าย/ขวา) ก่อน
                if (Mathf.Abs(m) > boundsY / boundsX)
                {
                    // ชนขอบบนหรือล่าง
                    clampedY = Mathf.Sign(direction.y) * boundsY;
                    clampedX = clampedY / m;
                }
                else
                {
                    // ชนขอบซ้ายหรือขวา
                    clampedX = Mathf.Sign(direction.x) * boundsX;
                    clampedY = clampedX * m;
                }

                // อัปเดตตำแหน่งกลับไปที่หน้าจอ โดยอิงจากจุดศูนย์กลางของกล้องตัวนี้
                screenPos = new Vector3(screenCenter.x + clampedX, screenCenter.y + clampedY, 0);

                // หากต้องการให้ไอคอนหมุนชี้ตามทิศทาง (เช่น เป็นรูปลูกศร) ให้ปลดคอมเมนต์โค้ดด้านล่าง
                /*
                float arrowAngle = angle * Mathf.Rad2Deg;
                markerUI.transform.rotation = Quaternion.Euler(0, 0, arrowAngle - 90f); // ลบ 90 ขึ้นอยู่กับภาพต้นฉบับ
                */
            }
            else
            {
                // ถ้าอยู่ในจอปกติ ให้รีเซ็ตการหมุน (ถ้าใช้ระบบหมุนลูกศร)
                // markerUI.transform.rotation = Quaternion.identity;
            }

            markerUI.enabled = true;
            markerUI.transform.position = screenPos;
        }
        else
        {
            if (markerUI != null) markerUI.enabled = false;
        }
    }


    public void ToggleMarker()
    {
        isMarkerActive = !isMarkerActive;
        if (!isMarkerActive && markerUI != null) markerUI.enabled = false;
    }
}
