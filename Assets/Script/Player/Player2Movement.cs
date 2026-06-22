using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    public float speed = 5f;
    public Transform cameraTransform;

    void Update()
    {
        float x = 0f;
        float z = 0f;

        // IJKL input
        if (Input.GetKey(KeyCode.I)) z += 1f;   // forward
        if (Input.GetKey(KeyCode.K)) z -= 1f;   // backward
        if (Input.GetKey(KeyCode.L)) x += 1f;   // right
        if (Input.GetKey(KeyCode.J)) x -= 1f;   // left

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
