using UnityEngine;

public class Player1Dash : MonoBehaviour
{
    public KeyCode dashKey = KeyCode.E;   // Dash button for Player1
    public int maxDashes = 3;             // How many dashes allowed
    public float dashForce = 15f;         // Dash strength
    public float dashCooldown = 0.5f;     // Cooldown between dashes

    private int dashesLeft;
    private float lastDashTime;
    private Rigidbody rb;
    private Vector3 lastMoveDir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dashesLeft = maxDashes;
    }

    void Update()
    {
        // Movement input for Player1 (WASD by default)
        float h = Input.GetAxisRaw("Horizontal_P1");
        float v = Input.GetAxisRaw("Vertical_P1");
        Vector3 moveInput = new Vector3(h, 0, v);

        if (moveInput.sqrMagnitude > 0.01f)
            lastMoveDir = moveInput.normalized;

        if (Input.GetKeyDown(dashKey) && dashesLeft > 0 && Time.time > lastDashTime + dashCooldown)
            Dash();
    }

    void Dash()
    {
        if (lastMoveDir == Vector3.zero)
            lastMoveDir = transform.forward;

        rb.velocity = lastMoveDir * dashForce; // Snappy dash
        dashesLeft--;
        lastDashTime = Time.time;
    }

    public void ResetDashes()
    {
        dashesLeft = maxDashes;
    }
}
