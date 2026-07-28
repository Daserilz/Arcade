using UnityEngine;
using UnityEngine.InputSystem;

public class ArcadeControllerManager : MonoBehaviour
{
    [Header("ลาก Player Input ของตัวละคร 1 และ 2 มาใส่ตรงนี้")]
    public PlayerInput player1Input;
    public PlayerInput player2Input;

    void Start()
    {
        AssignControllers();
    }

    void AssignControllers()
    {
        // check controller count
        int gamepadCount = Gamepad.all.Count;

        if (gamepadCount >= 1 && player1Input != null)
        {
            // บังคับให้ Player 1 use first controller (Index 0)
            // "Gamepad" คือชื่อ Control Scheme ที่คุณตั้งไว้
            player1Input.SwitchCurrentControlScheme("Arcade", Gamepad.all[0]);
            Debug.Log("จับคู่จอย 1 ให้ Player 1 สำเร็จ");
        }

        if (gamepadCount >= 2 && player2Input != null)
        {
            // บังคับให้ Player 2 ใช้จอยตัวที่สอง (Index 1)
            player2Input.SwitchCurrentControlScheme("Arcade", Gamepad.all[1]);
            Debug.Log("จับคู่จอย 2 ให้ Player 2 สำเร็จ");
        }
        else if (gamepadCount < 2)
        {
            Debug.LogWarning("เสียบจอยไม่ครบ 2 ตัว!");
        }
    }
}
