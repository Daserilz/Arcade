using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static SoundManager Instance { get; private set; }
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        // 2. ตรวจสอบว่ามี Instance อื่นอยู่แล้วหรือไม่
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // ทำให้ SoundManager อยู่ข้าม Scene ได้
    }
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;

        if (musicSource.clip == clip && musicSource.isPlaying) return;
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // --- ฟังก์ชันสำหรับเล่นเสียง Sound Effects (SFX) ---
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        // ใช้ PlayOneShot เพื่อให้เล่นเสียง SFX ทับซ้อนกันได้โดยไม่ตัดเสียงที่กำลังเล่นอยู่
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlaySFXWithFadeOut(AudioClip clip, float playDuration, float fadeTime = 0.2f)
    {
        StartCoroutine(FadeOutSFXCoroutine(clip, playDuration, fadeTime));
    }

    private IEnumerator FadeOutSFXCoroutine(AudioClip clip, float playDuration, float fadeTime)
    {
        // เล่นเสียง SFX
        sfxSource.PlayOneShot(clip);

        // รอจนกว่าจะถึงช่วงที่ต้องเริ่ม Fade
        yield return new WaitForSeconds(playDuration - fadeTime);

        float startVolume = sfxSource.volume;
        float timer = 0f;

        // ค่อยๆ ลดระดับเสียงลงจนเหลือ 0
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            sfxSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeTime);
            yield return null;
        }

        sfxSource.Stop();
        sfxSource.volume = startVolume; // ดึงระดับเสียงกลับมาคืนค่าเดิมสำหรับเสียงถัดไป
    }

    // --- ฟังก์ชันปรับระดับเสียง ---
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp01(volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }
}
