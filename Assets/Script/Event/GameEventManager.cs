using System.Collections;
using UnityEngine;


public enum GameEventType
{
    CharacterSwap, // สลับตัวละคร
    WorldBorder,   // บีบพื้นที่
    Laser          // เลเซอร์
}
public class EventManager : MonoBehaviour
{
    [SerializeField] private float eventDuration = 30f;
    [SerializeField] private float cooldownDuration = 30f;


    [SerializeField] private CharacterChange formSwitcher;
    [SerializeField] private LaserEventController laserController;
    [SerializeField] private WorldBorderEventController worldBorderController;

    private UiManager uiManager;

    private GameEventType currentEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(EventLoopRoutine());
        uiManager = FindAnyObjectByType<UiManager>();
    }

    private IEnumerator EventLoopRoutine()
    {
        // ใช้ while(true) เพื่อให้ลูปทำงานวนไปเรื่อยๆ ตลอดทั้งเกม
        while (true)
        {
            // 1. ช่วง Cooldown (รอ 30 วินาที)
            Debug.Log($"<color=white>เริ่มนับ Cooldown {cooldownDuration} วินาที...</color>");
            yield return new WaitForSeconds(cooldownDuration);

            // 2. สุ่ม Event และสั่งให้ Event ทำงาน
            StartRandomEvent();

            // 3. ปล่อยให้ Event ทำงานเป็นเวลา 30 วินาที
            yield return new WaitForSeconds(eventDuration);

            // 4. สั่งหยุด Event ปัจจุบัน
            StopCurrentEvent();
        }
    }

    private void StartRandomEvent()
    {
        // สุ่มตัวเลขตั้งแต่ 0 ถึง 2 (3 ไม่นับ) เพื่อเลือก Event
        int randomValue = Random.Range(0, 3);
        currentEvent = (GameEventType)randomValue;

        Debug.Log($"<color=red>🔥 เกิด Event: {currentEvent}!</color>");
        uiManager.UpdateEventUI(currentEvent);

        // ใช้ Switch Case เพื่อแยกว่าต้องเรียกคำสั่งอะไร ตาม Event ที่สุ่มได้
        switch (currentEvent)
        {
            case GameEventType.CharacterSwap:
                if (formSwitcher != null)
                {
                    // เรียกใช้ฟังก์ชันสลับตัวละครทั้งหมดในฉากที่เราเคยทำไว้
                    formSwitcher.SwitchAllPlayersForm(eventDuration);
                }
                break;

            case GameEventType.WorldBorder:
                // TODO: ใส่คำสั่งเปิดใช้งาน Worldborder
                if (worldBorderController != null) worldBorderController.StartBorderEvent();
                Debug.Log("Worldborder กำลังบีบเข้ามา!");
                break;

            case GameEventType.Laser:
                // TODO: ใส่คำสั่งเปิดใช้งาน เลเซอร์ ตรงนี้
                if (laserController != null) laserController.StartLaserEvent();
                Debug.Log("ระวัง! เลเซอร์ทำงานแล้ว!");
                break;
        }
    }

    private void StopCurrentEvent()
    {
        Debug.Log($"<color=yellow>จบ Event: {currentEvent} - กลับสู่สภาวะปกติ</color>");
        uiManager.eventText.text = $"Current Event : None ";

        switch (currentEvent)
        {
            case GameEventType.CharacterSwap:
                // have 30 s for script
                // 
                break;

            case GameEventType.WorldBorder:
                // TODO: ใส่คำสั่งปิด Worldborder / ขยายพื้นที่กลับ
                if (worldBorderController != null) worldBorderController.StopBorderEvent();
                Debug.Log("Worldborder ขยายกลับเป็นปกติ");
                break;

            case GameEventType.Laser:
                // TODO: ใส่คำสั่งปิด เลเซอร์
                if (laserController != null) laserController.StopLaserEvent();
                Debug.Log("เลเซอร์ถูกปิดลงแล้ว");
                break;

        }
    }
}
