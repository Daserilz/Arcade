using UnityEngine;

public class WorldBorderMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 2f;

    [Header("Distances")]
    [SerializeField] private float eventDistance = 50f;   // Distance for event mode
    [SerializeField] private float fullDistance = 200f;   // Distance for escape mode

    [Header("Direction")]
    [SerializeField] private Vector3 moveDirection = Vector3.forward;

    private Vector3 startPosition;
    private bool eventMode = false;
    private bool escapeMode = false;

    private void Awake()
    {
        startPosition = transform.position;
        moveDirection = moveDirection.normalized;

        // Make sure the border starts at its original position.
        transform.position = startPosition;
    }

    private void Update()
    {
        if (eventMode)
        {
            MoveBorder(eventDistance, true);
        }
        else if (escapeMode)
        {
            MoveBorder(fullDistance, false);
        }
    }

    private void MoveBorder(float targetDistance, bool returnToStart)
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        float distanceMoved = Vector3.Distance(startPosition, transform.position);

        if (distanceMoved >= targetDistance)
        {
            if (returnToStart)
            {
                // Event mode: return to start
                transform.position = startPosition;
                eventMode = false;
            }
            else
            {
                // Escape mode: stop at the end
                transform.position = startPosition + moveDirection * fullDistance;
                escapeMode = false;
            }
        }
    }

    // Called when a random event starts
    public void ActivateEventMode()
    {
        transform.position = startPosition;
        eventMode = true;
        escapeMode = false;
    }

    // Called when escape timer reaches 0
    public void ActivateEscapeMode()
    {
        transform.position = startPosition;
        escapeMode = true;
        eventMode = false;
    }

    // Optional: stop everything
    public void StopBorder()
    {
        eventMode = false;
        escapeMode = false;
    }

    // Optional: reset manually
    public void ResetBorder()
    {
        eventMode = false;
        escapeMode = false;
        transform.position = startPosition;
    }
}