using System.Collections.Generic;
using UnityEngine;

public class LaserEventController : MonoBehaviour
{
    [SerializeField] private GameObject[] sceneLasers;



    private void Start()
    {
        // ป้องกันบั๊ก: บังคับปิดเลเซอร์ทั้งหมดทันทีตอนเริ่มเกม
        foreach (GameObject laser in sceneLasers)
        {
            if (laser != null)
            {
                laser.SetActive(false);
            }
        }
    }
    public void StartLaserEvent()
    {
        if (sceneLasers.Length == 0)
        {
            Debug.LogWarning("not have laser in Scene Lasers!");
            return;
        }

        foreach (GameObject laser in sceneLasers)
        {
            if (laser != null)
            {
                laser.SetActive(true); // เปิดการแสดงผลและการทำงาน
            }
        }
    }

    /// <summary>
    /// เรียกเมื่อหมดเวลา Event (30 วิ): ลบเลเซอร์ทั้งหมด
    /// </summary>
    public void StopLaserEvent()
    {
        foreach (GameObject laser in sceneLasers)
        {
            if (laser != null)
            {
                laser.SetActive(false); // ซ่อนและหยุดการทำงาน
            }
        }
    }
}
