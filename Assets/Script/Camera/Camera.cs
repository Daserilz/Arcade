using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public float smoothSpeed = 5f;
    public float minZoom = 5f;
    public float maxZoom = 20f;
    public float zoomLimiter = 10f;
    public Vector3 offset;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (player1 == null || player2 == null) return;

        // Center point between players
        Vector3 centerPoint = (player1.position + player2.position) / 2f;
        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed * Time.deltaTime);

        // Zoom based on distance
        float distance = Vector3.Distance(player1.position, player2.position);
        float newZoom = Mathf.Lerp(minZoom, maxZoom, distance / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }
}
