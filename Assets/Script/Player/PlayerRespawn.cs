using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 lastCheckpointPosition;

    private PlayerMovement playerMovement;
    private Rigidbody rb;

    [Header("Invincibility Settings")]
    [SerializeField] private float invincibilityDuration = 3f;
    private Renderer[] activeRenderers;

    [HideInInspector] public bool isInvincible = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        lastCheckpointPosition = transform.position;
    }

    public void UpdateRenderers(GameObject targetModel)
    {
        if (targetModel != null)
        {
            // ดึง Renderer ทั้งหมดจากตัวเองและลูกๆ (Children) ลงใน Array
            activeRenderers = targetModel.GetComponentsInChildren<Renderer>();
        }
    }

    public void SetNewCheckpoint(Vector3 position)
    {
        lastCheckpointPosition = position;
    }

    public void Respawn()
    {
        if (isInvincible)
        {
            Debug.Log("Immune!");
            return;
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
            transform.position = lastCheckpointPosition;
            playerMovement.enabled = true;
        }
        // กรณีที่ 2: ถ้าเกมใช้ Rigidbody (ต้องล้างค่าแรงส่งและแรงเฉื่อยทั้งหมดให้เป็น 0)
        else if (rb != null)
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
        ActivateInvincibility();
        Debug.Log("[Respawn]");
    }

    public void ActivateInvincibility()
    {
        // สั่งเริ่ม Coroutine
        StartCoroutine(InvincibilityRoutine());
    }

    private IEnumerator InvincibilityRoutine()
    {
        // เปิดโหมดอมตะ
        isInvincible = true;

        float elapsedTime = 0f;
        float blinkInterval = 0.15f; // ความเร็วกระพริบ (ทุกๆ 0.15 วินาที)

        // ลูปทำงานจนกว่าจะหมดเวลา invincibilityDuration (3 วินาที)
        while (elapsedTime < invincibilityDuration)
        {
            // สลับการแสดงผล (เปิด/ปิด โมเดล) เพื่อทำเอฟเฟกต์กระพริบ
            if (activeRenderers != null)
            {
                foreach (Renderer rend in activeRenderers)
                {
                    if (rend != null)
                    {
                        rend.enabled = !rend.enabled;
                    }
                }
            }

            // สั่งให้ Coroutine รอเวลา 0.15 วิ แล้วค่อยกลับมาทำบรรทัดถัดไป 
            yield return new WaitForSeconds(blinkInterval);

            // บวกเวลาเพิ่มเข้าไป
            elapsedTime += blinkInterval;
        }

        // เมื่อครบ 3 วินาที
        // 1. บังคับเปิดการแสดงผลให้กลับมาเป็นปกติ (ป้องกันบั๊กลูปจบตอนโมเดลกำลังปิดอยู่)
        if (activeRenderers != null)
        {
            foreach (Renderer rend in activeRenderers)
            {
                if (rend != null)
                {
                    rend.enabled = true;
                }
            }
        }

        // 2. ปิดโหมดอมตะ
        isInvincible = false;
        // Debug.Log("หมดเวลาอมตะ กลับสู่สภาวะปกติ");
    }
}
