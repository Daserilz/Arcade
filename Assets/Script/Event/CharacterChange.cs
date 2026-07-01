using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class CharacterChange : MonoBehaviour
{
    [Header("Test Settings")]
    [Tooltip("ติ๊กถูกหากต้องการเปิดใช้งานการกดปุ่มบนคีย์บอร์ดเพื่อทดสอบระบบ")]
    [SerializeField] private bool enableKeyboardTest = true;

    [Tooltip("กำหนดปุ่มบนคีย์บอร์ดที่ต้องการใช้กดสลับร่างทั้งหมดในฉาก")]
    [SerializeField] private string testKey = "g";
    private void Update()
    {
        // ส่วนนี้ทำไว้สำหรับทดสอบ: ถ้ากดปุ่ม G (หรือปุ่มที่ตั้งไว้) บนคีย์บอร์ด จะสั่งสลับร่างทันที
        if (enableKeyboardTest && Keyboard.current != null)
        {
            // ตรวจสอบว่าปุ่มที่ตั้งไว้ถูกกดในเฟรมนี้หรือไม่
            var targetKeyField = Keyboard.current[testKey] as KeyControl;
            if (targetKeyField != null && targetKeyField.wasPressedThisFrame)
            {

                SwitchAllPlayersForm();
            }
        }
    }
    public void SwitchAllPlayersForm()
    {
        // 1. ค้นหาคอมโพเนนต์ PlayerInteractor ทั้งหมดที่เปิดใช้งานอยู่ในฉากปัจจุบัน
        PlayerInteractor[] allPlayers = Object.FindObjectsByType<PlayerInteractor>(FindObjectsSortMode.None);

        // 2. ตรวจสอบว่ามี Player อยู่ในฉากไหม ถ้าไม่มีให้แจ้งเตือนเพื่อป้องกัน Error
        if (allPlayers.Length == 0)
        {
            Debug.LogWarning("GlobalFormSwitcher: ไม่พบวัตถุที่มีสคริปต์ PlayerInteractor อยู่ในฉากเลย!");
            return;
        }

        // 3. วนลูป (Foreach) เพื่อสั่งให้ Player ทุกๆ ตัวสลับร่างตามเงื่อนไขของตัวเอง
        foreach (PlayerInteractor player in allPlayers)
        {
            if (player != null)
            {
                player.SwitchForm();
            }
        }

        Debug.Log($"<color=green><b>[Global Switch]</b> สั่งสลับร่างตัวละครทั้งหมดในฉากสำเร็จ! (รวมทั้งหมด {allPlayers.Length} ตัว)</color>");
    }
}
