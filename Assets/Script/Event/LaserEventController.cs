using System.Collections.Generic;
using UnityEngine;

public class LaserEventController : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform[] spawnPoints;


    private List<GameObject> activeLasers = new List<GameObject>();
    public void StartLaserEvent()
    {
        if (laserPrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("ยังไม่ได้ตั้งค่า Laser Prefab หรือ Spawn Points!");
            return;
        }

        // วนลูปสร้างเลเซอร์ตามจำนวนจุดเกิดที่มี
        foreach (Transform point in spawnPoints)
        {
            GameObject laser = Instantiate(laserPrefab, point.position, point.rotation);
            activeLasers.Add(laser); // เก็บเข้า List
        }
    }

    /// <summary>
    /// เรียกเมื่อหมดเวลา Event (30 วิ): ลบเลเซอร์ทั้งหมด
    /// </summary>
    public void StopLaserEvent()
    {
        foreach (GameObject laser in activeLasers)
        {
            if (laser != null)
            {
                Destroy(laser);
            }
        }

        // เคลียร์ข้อมูลใน List หลังจากลบของจริงทิ้งหมดแล้ว
        activeLasers.Clear();
    }
}
