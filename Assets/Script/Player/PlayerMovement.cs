using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Transform cameraTransform;

    void Update()
    {
        float x = Input.GetAxis("Horizontal"); // WASD
        float z = Input.GetAxis("Vertical");   // WASD

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
        }
    }
}
