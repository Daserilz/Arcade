using UnityEngine;
using UnityEngine.UI;

public class Player2Movement : MonoBehaviour
{
    public float speed = 5f;
    public Transform cameraTransform;

    [Header("Dash Settings")]
    public int maxDashes = 3;
    public float dashForce = 5f;
    public float dashCooldown = 0.5f;
    private int dashesLeft;
    private float lastDashTime;
    private Rigidbody rb;
    private Vector3 lastMoveDir;

    [Header("UI Dash Icons")]
    public Image[] dashIcons;
    public Sprite fullIcon;
    public Sprite emptyIcon;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dashesLeft = maxDashes;
        UpdateDashUI();
    }

    void Update()
    {
        float x = 0f;
        float z = 0f;

        if (Input.GetKey(KeyCode.I)) z += 1f;
        if (Input.GetKey(KeyCode.K)) z -= 1f;
        if (Input.GetKey(KeyCode.L)) x += 1f;
        if (Input.GetKey(KeyCode.J)) x -= 1f;

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

        if (Input.GetKeyDown(KeyCode.O)) // Example dash key for Player2
        {
            if (dashesLeft > 0 && Time.time > lastDashTime + dashCooldown)
            {
                Dash();
            }
        }
    }

    void Dash()
    {
        Vector3 dashDir = lastMoveDir;
        if (dashDir == Vector3.zero)
        {
            dashDir = transform.forward;
        }

        rb.linearVelocity = lastMoveDir * dashForce;
        dashesLeft--;
        lastDashTime = Time.time;
        UpdateDashUI();
    }

    public void ResetDashes()
    {
        dashesLeft = maxDashes;
        UpdateDashUI();
    }

    void UpdateDashUI()
    {
        for (int i = 0; i < dashIcons.Length; i++)
        {
            if (i < dashesLeft)
                dashIcons[i].sprite = fullIcon;
            else
                dashIcons[i].sprite = emptyIcon;
        }
    }
}
