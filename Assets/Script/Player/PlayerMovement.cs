using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Settings")]
    public float speed = 5f;
    public Transform cameraTransform;
    private Vector2 moveInput;

    [Header("Dash Settings")]
    public int maxDashes = 3;             // How many dashes allowed
    public float dashForce = 5f;         // Dash strength
    public float dashCooldown = 0.5f;
    private int dashesLeft;
    private float lastDashTime;
    private Rigidbody rb;
    private Vector3 lastMoveDir;


    [Header("Animator")]
    public Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        dashesLeft = maxDashes;
    }

    public void OnMoveEvent(InputAction.CallbackContext context)
    {
        // อ่านค่าทิศทางออกมาเป็น Vector2 (X แกนซ้าย-ขวา, Y แกนบน-ล่าง)
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnDash(CallbackContext context)
    {
        if (context.performed)
        {
            if (dashesLeft > 0 && Time.time > lastDashTime + dashCooldown)
            {
                Dash();
                Debug.Log("Dash performed! Dashes left: " + dashesLeft);
            }
        }
    }


    void Update()
    {
       float x = moveInput.x;
       float z = moveInput.y;

        ////float x = Input.GetAxis("Horizontal"); // WASD
        ////float z = Input.GetAxis("Vertical");   // WASD

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 move = (forward * z + right * x);

        if (move.magnitude > 0.1f)
        {
            transform.position += move.normalized * speed * Time.deltaTime;
            lastMoveDir = move.normalized;
        }

        if (animator != null)
        {
            animator.SetFloat("InputX", x);
            animator.SetFloat("InputY", z);
        }
    }

    void Dash()
    {
        Vector3 dashDir = lastMoveDir;
        if (dashDir == Vector3.zero)
        {
            dashDir = transform.forward;
        }

        rb.linearVelocity = lastMoveDir * dashForce; // Snappy dash
        dashesLeft--;
        lastDashTime = Time.time;
    }

    public void ResetDashes()
    {
        dashesLeft = maxDashes;
    }


    public void UpdateAnimator(Animator newAnimator)
    {
        animator = newAnimator;
    }
}
