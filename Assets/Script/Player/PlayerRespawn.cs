using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 lastCheckpointPosition;

    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastCheckpointPosition = transform.position;
    }

    public void SetNewCheckpoint(Vector3 position)
    {
        lastCheckpointPosition = position;
    }

    public void Respawn()
    {
        // กรณีที่ 2: ถ้าเกมใช้ Rigidbody (ต้องล้างค่าแรงส่งและแรงเฉื่อยทั้งหมดให้เป็น 0)
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.position = lastCheckpointPosition; // แนะนำให้ใช้ rb.position แทน transform.position
        }
        else
        {
            // ถ้าเป็นออบเจกต์ธรรมดา ย้ายได้เลย
            transform.position = lastCheckpointPosition;
        }

        Debug.Log("[Respawn]");
    }
}
