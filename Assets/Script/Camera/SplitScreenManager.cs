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
        // Ensure only one AudioListener active at start
        if (camera1.GetComponent<AudioListener>() == null)
            camera1.gameObject.AddComponent<AudioListener>();
        if (camera2.GetComponent<AudioListener>() == null)
            camera2.gameObject.AddComponent<AudioListener>();

        camera1.rect = new Rect(0f, 0f, 0.5f, 1f);
        camera2.rect = new Rect(0.5f, 0f, 0.5f, 1f);
        camera2.enabled = true;
    }

    void LateUpdate()
    {
        if (player1 != null)
        {
            FollowPlayer(camera1, player1);
        }
        else
        {
            TurnCameraBlack(camera1);
        }

        if (player2 != null)
        {
            FollowPlayer(camera2, player2);
        }
        else
        {
            TurnCameraBlack(camera2);
        }

        // Keep only one AudioListener active (choose Player1’s camera)

        if (camera1 != null && camera2 != null)
        {
            camera1.GetComponent<AudioListener>().enabled = true;
            camera2.GetComponent<AudioListener>().enabled = false;
        }
    }


    void TurnCameraBlack(Camera cam)
    {
        if (cam != null)
        {
            cam.cullingMask = 0; // สั่งไม่ให้กล้องมองเห็น Layer ใดๆ ในเกมเลย
            cam.clearFlags = CameraClearFlags.SolidColor; // เปลี่ยนรูปแบบการเคลียร์ภาพเป็นสีทึบ
            cam.backgroundColor = Color.black; // ตั้งค่าสีพื้นหลังเป็นสีดำ
        }
    }


    void FollowPlayer(Camera cam, Transform player)
    {
        Vector3 targetPos = player.position + offset;
        cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, smoothSpeed * Time.deltaTime);
    }
}
