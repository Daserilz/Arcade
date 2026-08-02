using UnityEngine;

public class SceneBGM : MonoBehaviour
{
    [SerializeField] private AudioClip sceneMusic;
    [SerializeField] private bool stopMusicOnExit = false;

    private void Start()
    {
        // สั่งให้ SoundManager เล่นเพลงที่กำหนดทันทีที่ Scene โหลดขึ้นมา
        if (SoundManager.Instance != null && sceneMusic != null)
        {
            SoundManager.Instance.PlayMusic(sceneMusic);
        }
    }

    private void OnDestroy()
    {
        // (ตัวเลือกเสริม) ถ้าต้องการให้หยุดเพลงทันทีที่ออกจาก Scene นี้
        if (stopMusicOnExit && SoundManager.Instance != null)
        {
            SoundManager.Instance.StopMusic();
        }
    }
}
