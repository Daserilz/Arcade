using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockTransition : MonoBehaviour
{
    [Header("UI Blocks")]
    public RectTransform[] blocks;

    [Header("Transition Settings")]
    public float moveDuration = 0.5f;
    public float delayBetweenBlocks = 0.1f;

    [Header("Positions (Y Axis)")]
    public float hiddenY = 1200f;
    public float visibleY = 0f;
    void Start()
    {
        foreach (var block in blocks)
        {
            block.anchoredPosition = new Vector2(block.anchoredPosition.x, hiddenY);
        }
    }

    public void PlayTransition(string nextSceneName)
    {
        StartCoroutine(AnimateTransition(nextSceneName));
    }

    private IEnumerator AnimateTransition(string nextSceneName)
    {
        // 1. Transition In (บล็อคเลื่อนลงมา)
        for (int i = 0; i < blocks.Length; i++)
        {
            // DOTween Magic: สั่งให้เลื่อนแกน Y ไปที่ visibleY ในเวลา moveDuration
            // SetEase(Ease.OutBack) จะทำให้มีจังหวะเด้งดึ๋งนิดๆ ตอนลงมาถึง
            blocks[i].DOAnchorPosY(visibleY, moveDuration).SetEase(Ease.OutCubic);

            yield return new WaitForSeconds(delayBetweenBlocks);
        }

        // รอให้บล็อคตัวสุดท้ายขยับเสร็จ
        yield return new WaitForSeconds(moveDuration);

        // --- โหลด Scene ---
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
            while (!asyncLoad.isDone) yield return null;
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }

        // 2. Transition Out (เลื่อนบล็อคกลับขึ้นไป)
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].DOAnchorPosY(hiddenY, moveDuration).SetEase(Ease.InCubic);
            yield return new WaitForSeconds(delayBetweenBlocks);
        }
    }
}
