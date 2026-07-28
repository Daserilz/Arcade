using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelManager : MonoBehaviour
{
    public GameObject firstButtonPanel;

    // เรียกใช้ฟังก์ชันนี้เมื่อทำการเปิด Panel

    private void OnEnable()
    {
        StartCoroutine(OnOpenPanel());
    }
    private IEnumerator OnOpenPanel()
    {
        yield return null;

        if (firstButtonPanel != null)
        {
            // 2. เคลียร์ปุ่มที่เลือกอยู่ปัจจุบันทิ้งไปก่อน
            EventSystem.current.SetSelectedGameObject(null);

            // 3. บังคับให้ระบบไปเลือกปุ่มบน Panel ใหม่
            EventSystem.current.SetSelectedGameObject(firstButtonPanel);
        }
    }
}
