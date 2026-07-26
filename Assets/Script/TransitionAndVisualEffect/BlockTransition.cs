using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class BlockTransition : MonoBehaviour
{
    public static BlockTransition Instance;

    [Header("UI Settings")]
    public GameObject blockPrefab;
    public Transform blockContainer;

    [Header("Grid Settings")]
    public int amountOfBlocks = 100; // จำนวนบล็อก (ปรับให้พอดีเต็มจอ)

    [Header("Animation Settings")]
    public float animDuration = 0.4f;      // เวลาที่บล็อกแต่ละอันใช้ขยายตัว
    public float delayBetweenBlocks = 0.02f; // ระยะเวลาหน่วงระหว่างแต่ละบล็อก

    private List<RectTransform> blocks = new List<RectTransform>();

    private void Awake()
    {
        // ทำให้ Object นี้ไม่ถูกทำลายเมื่อเปลี่ยน Scene
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            GenerateBlocks();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // สร้างบล็อกเตรียมไว้ตอนเริ่มเกม
    private void GenerateBlocks()
    {
        for (int i = 0; i < amountOfBlocks; i++)
        {
            GameObject newBlock = Instantiate(blockPrefab, blockContainer);
            RectTransform rect = newBlock.GetComponent<RectTransform>();
            rect.localScale = Vector3.zero; // เริ่มต้นให้มองไม่เห็น
            blocks.Add(rect);
        }
    }

    // ==========================================
    // 1. แบบแยกส่วน: สั่งให้บล็อกขึ้นมาปิดจอ
    // ==========================================
    public void AnimateIn(Action onComplete = null)
    {
        StartCoroutine(AnimateInRoutine(onComplete));
    }

    private IEnumerator AnimateInRoutine(Action onComplete)
    {
        List<RectTransform> shuffledBlocks = blocks.OrderBy(x => UnityEngine.Random.value).ToList();
        Sequence seqIn = DOTween.Sequence().SetUpdate(true); // SetUpdate(true) ทำให้ทำงานได้แม้ Time.timeScale = 0 (ตอน Pause เกม)

        for (int i = 0; i < shuffledBlocks.Count; i++)
        {
            float randomScale = UnityEngine.Random.Range(0.8f, 3f);
            seqIn.Insert(i * delayBetweenBlocks,
                         shuffledBlocks[i].DOScale(randomScale, animDuration)
                         .SetEase(Ease.OutBack)
                         .SetLink(shuffledBlocks[i].gameObject));
        }

        yield return seqIn.WaitForCompletion();
        yield return new WaitForSecondsRealtime(0.2f); // หน่วงเวลานิดหน่อยให้เนียนตา

        onComplete?.Invoke(); // เรียกใช้คำสั่งที่ส่งเข้ามา (ถ้ามี)
    }

    // ==========================================
    // 2. แบบแยกส่วน: สั่งให้บล็อกหดลงไป
    // ==========================================
    public void AnimateOut(Action onComplete = null)
    {
        StartCoroutine(AnimateOutRoutine(onComplete));
    }

    private IEnumerator AnimateOutRoutine(Action onComplete)
    {
        List<RectTransform> shuffledBlocks = blocks.OrderBy(x => UnityEngine.Random.value).ToList();
        Sequence seqOut = DOTween.Sequence().SetUpdate(true);

        for (int i = 0; i < shuffledBlocks.Count; i++)
        {
            seqOut.Insert(i * delayBetweenBlocks,
                          shuffledBlocks[i].DOScale(0, animDuration)
                          .SetEase(Ease.InBack)
                          .SetLink(shuffledBlocks[i].gameObject));
        }

        yield return seqOut.WaitForCompletion();

        onComplete?.Invoke();
    }

    // ==========================================
    // 3. แบบสำเร็จรูป: ปิดจอ -> ทำอะไรบางอย่าง -> เปิดจอ
    // ==========================================
    public void PlayTransition(Action midTransitionAction)
    {
        AnimateIn(() =>
        {
            midTransitionAction?.Invoke(); // ทำคำสั่งที่ต้องการตอนจอดำ
            AnimateOut(); // เปิดจอกลับ
        });
    }

    // ==========================================
    // 4. โหลด Scene (ของเดิมที่ปรับให้ใช้ระบบใหม่)
    // ==========================================
    public void LoadScene(string sceneName)
    {
        AnimateIn(() =>
        {
            StartCoroutine(LoadSceneRoutine(sceneName));
        });
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;
        yield return new WaitForSeconds(0.2f); // รอซีนเซ็ตตัว

        AnimateOut();
    }
}
