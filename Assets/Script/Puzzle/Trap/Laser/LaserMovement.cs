using UnityEngine;

public class LaserMovement : MonoBehaviour
{
    public float speed = 5f;
    [SerializeField] private Vector3 moveDirection = Vector3.forward;
    [SerializeField] private float maxDistance = 15f;

    private Vector3 startPosition;
    private float spawnTime;

    // Update is called once per frame
    private void Start()
    {
        // บันทึกตำแหน่งและเวลาเริ่มต้นทันทีที่เลเซอร์ถูกสร้าง (เมื่อเริ่ม Event)
        startPosition = transform.position;
        spawnTime = Time.time;

        // ทำให้แน่ใจว่าทิศทางมีค่าความยาวเป็น 1 (ป้องกันความเร็วเพี้ยน)
        moveDirection = moveDirection.normalized;
    }
    void Update()
    {
        float timeAlive = Time.time - spawnTime;
        float currentDistance = Mathf.PingPong(timeAlive * speed, maxDistance);
        transform.position = startPosition + (moveDirection * currentDistance);
    }
}
