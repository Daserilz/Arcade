using UnityEngine;

public class SplitScreenCameraManager : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public Camera camera1;
    public Camera camera2;

    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0f, 5f, -10f);
    public float splitDistance = 15f; // distance threshold

    void Start()
    {
        // Make sure only one AudioListener is active at start
        if (camera1.GetComponent<AudioListener>() == null)
            camera1.gameObject.AddComponent<AudioListener>();
        if (camera2.GetComponent<AudioListener>() != null)
            camera2.GetComponent<AudioListener>().enabled = false;
    }

    void LateUpdate()
    {
        if (player1 == null || player2 == null) return;

        float distance = Vector3.Distance(player1.position, player2.position);

        if (distance > splitDistance)
        {
            // Enable split-screen
            camera1.rect = new Rect(0f, 0f, 0.5f, 1f);
            camera2.rect = new Rect(0.5f, 0f, 0.5f, 1f);
            camera2.enabled = true;

            // Follow each player
            FollowPlayer(camera1, player1);
            FollowPlayer(camera2, player2);

            // Audio listener toggle
            camera1.GetComponent<AudioListener>().enabled = false;
            camera2.GetComponent<AudioListener>().enabled = true;
        }
        else
        {
            // Merge into single view (Player1 camera only)
            camera1.rect = new Rect(0f, 0f, 1f, 1f);
            camera2.enabled = false;

            // Follow Player1 only
            FollowPlayer(camera1, player1);

            // Audio listener toggle
            camera1.GetComponent<AudioListener>().enabled = true;
            camera2.GetComponent<AudioListener>().enabled = false;
        }
    }

    void FollowPlayer(Camera cam, Transform player)
    {
        Vector3 targetPos = player.position + offset;
        cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, smoothSpeed * Time.deltaTime);
    }
}
