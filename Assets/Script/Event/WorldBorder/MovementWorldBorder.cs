using UnityEngine;

public class WorldBorderMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float rePosTime = 10f;
    private float currentTime;
    [SerializeField] private Vector3 moveDirection = Vector3.forward;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector3 startPosition;
    private void Awake()
    {
        // บันทึกตำแหน่งเริ่มต้นของกำแพงไว้ตั้งแต่ตอนโหลดฉาก
        startPosition = transform.position;
        currentTime = rePosTime;
    }

    private void OnEnable()
    {
        // ทุกครั้งที่ Event เริ่มทำงาน (SetActive เป็น true) ให้ดึงบล็อกกลับมาที่จุดเริ่มต้นก่อนเสมอ
        transform.position = startPosition;
        moveDirection = moveDirection.normalized;
        currentTime = rePosTime;
    }

    private void Update()
    {
        // สั่งให้บล็อกเคลื่อนที่ไปตามทิศทางเรื่อยๆ จนกว่า Event จะถูกปิด
        transform.Translate(moveDirection * speed * Time.deltaTime);
        currentTime -= Time.deltaTime;
        if (currentTime < 0 )
        {
            transform.position = startPosition;
            moveDirection = moveDirection.normalized;
            currentTime = rePosTime;
        }
    }
}
