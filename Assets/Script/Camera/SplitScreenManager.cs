using UnityEngine;

public class SplitScreenCameraManager : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public Camera camera1;
    public Camera camera2;

    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0f, 5f, -10f);

    void Start()
    {
        // Ensure both cameras exist and set split-screen rects
        camera1.rect = new Rect(0f, 0f, 0.5f, 1f);
        camera2.rect = new Rect(0.5f, 0f, 0.5f, 1f);

        camera1.enabled = true;
        camera2.enabled = true;

        // Audio listener setup: only one active
        if (camera1.GetComponent<AudioListener>() == null)
            camera1.gameObject.AddComponent<AudioListener>();
        if (camera2.GetComponent<AudioListener>() != null)
            camera2.GetComponent<AudioListener>().enabled = false;
    }

    void LateUpdate()
    {
        if (player1 == null || player2 == null) return;

        // Always follow each player
        FollowPlayer(camera1, player1);
        FollowPlayer(camera2, player2);
    }

    void FollowPlayer(Camera cam, Transform player)
    {
        Vector3 targetPos = player.position + offset;
        cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, smoothSpeed * Time.deltaTime);
    }
}
