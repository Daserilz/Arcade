using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    [SerializeField] private AudioClip clickSound;  // เสียงตอนกดยืนยัน (Enter / Space / คลิก / ปุ่มจอย A)
    [SerializeField] private AudioClip selectSound; // เสียงตอนเลื่อนลูกศรมาโดนปุ่ม (หรือเมาส์ชี้)

    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();

        // ใส่เสียงตอน "กดยืนยันปุ่ม" อัตโนมัติ (ครอบคลุมทั้ง เมาส์คลิก, Enter, Spacebar, จอยกด Submit)
        button.onClick.AddListener(PlayClickSound);
    }
    private void PlayClickSound()
    {
        if (SoundManager.Instance != null && clickSound != null)
        {
            SoundManager.Instance.PlaySFX(clickSound);
        }
    }

    // 1. ทำงานเมื่อ "เลื่อนปุ่มลูกศร (Keyboard/Controller)" มาโฟกัสที่ปุ่มนี้
    public void OnSelect(BaseEventData eventData)
    {
        PlaySelectSound();
    }

    // 2. ทำงานเมื่อ "เอาเมาส์มาชี้" บนปุ่มนี้
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlaySelectSound();
    }
    private void PlaySelectSound()
    {
        // ตรวจสอบว่าปุ่มกดได้ปกติ (ไม่ถูกตั้งเป็น Disable) ก่อนเล่นเสียง
        if (button.interactable && SoundManager.Instance != null && selectSound != null)
        {
            SoundManager.Instance.PlaySFX(selectSound);
        }
    }
}
