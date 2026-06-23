using UnityEngine;

public class CameraFollowPlayer1 : MonoBehaviour
{
    public Transform player1;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (player1 == null) return;

        Vector3 targetPos = player1.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
    }
}
