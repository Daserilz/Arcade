using UnityEngine;

public class Wave : MonoBehaviour
{
    [Header("Wave Settings")]
    public float amplitude = 50f;  // ความสูงของคลื่น
    public float frequency = 0.05f; // ความกว้างคลื่น (ปรับค่าน้อยๆ คลื่นจะกว้าง)
    public float speed = 5f;        // ความเร็วคลื่น

    [Header("Blocks")]
    public Transform[] blocks; // ลาก 2D Sprite หรือ UI (RectTransform) มาใส่ทั้งหมด

    // เก็บตำแหน่งเริ่มต้นของแต่ละบล็อคไว้ เพื่อไม่ให้มันเคลื่อนที่เพี้ยน
    private Vector3[] startPositions;

    void Start()
    {
        // บันทึกตำแหน่งเริ่มต้นตอนเริ่มเกม
        startPositions = new Vector3[blocks.Length];
        for (int i = 0; i < blocks.Length; i++)
        {
            startPositions[i] = blocks[i].localPosition;
        }
    }

    void Update()
    {
        // ให้ Manager สั่งการทุกบล็อคใน Update เดียว (เร็วกว่าให้แต่ละบล็อคมี Update ของตัวเอง)
        for (int i = 0; i < blocks.Length; i++)
        {
            // ใช้แกน X ของบล็อคนั้นๆ เป็นจุดอ้างอิงคลื่น
            float xPos = startPositions[i].x;

            // คำนวณความสูงใหม่ด้วยสูตร Sine Wave
            float waveY = Mathf.Sin((xPos * frequency) + (Time.unscaledTime * speed)) * amplitude;

            // อัปเดตตำแหน่งใหม่ (ขยับเฉพาะแกน Y)
            blocks[i].localPosition = startPositions[i] + new Vector3(0, waveY, 0);
        }
    }
}
