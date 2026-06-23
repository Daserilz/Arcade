using UnityEngine;

public class CameraFollowPlayer2 : MonoBehaviour
{
    public Transform player2;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (player2 == null) return;

        Vector3 targetPos = player2.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
    }
}
