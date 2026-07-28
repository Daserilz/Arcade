using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionUI : MonoBehaviour
{
    public static SceneTransitionUI Instance;

    [Header("Default UI Elements")]
    public RectTransform defaultOldUIPanel; // หน้าจอเริ่มต้นที่จะบีบให้ดับ[cite: 1]
    public CanvasGroup defaultNewUI;        // หน้าจอใหม่ที่จะเฟดขึ้นมา[cite: 1]
    public CanvasGroup whiteFlash;          // แสงวาบสีขาวตอนจอดับ[cite: 1]

    private void Awake()
    {
        // ทำเป็น Singleton เพื่อให้เรียกจากสคริปต์อื่นได้ง่ายๆ
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ==========================================
    // 1. แบบแยกส่วน: เอฟเฟกต์ปิดทีวี (TV Off)
    // ==========================================
    public void TurnOffTV(RectTransform targetPanel = null, Action onComplete = null)
    {
        // ถ้าไม่ส่ง targetPanel มา ให้ใช้ default ตัวเก่า
        RectTransform panelToClose = targetPanel != null ? targetPanel : defaultOldUIPanel;

        if (panelToClose == null) return;

        Sequence seq = DOTween.Sequence().SetUpdate(true); // ทำงานได้แม้ Time.timeScale = 0

        // จังหวะที่ 1: บีบจอแนวตั้ง + ขยายแกน X นิดๆ[cite: 1]
        seq.Append(panelToClose.DOScaleY(0.01f, 0.2f).SetEase(Ease.InCubic).SetLink(panelToClose.gameObject)); //[cite: 1]
        seq.Join(panelToClose.DOScaleX(1.05f, 0.15f).SetLink(panelToClose.gameObject)); //[cite: 1]

        // จังหวะที่ 2: หดเส้นแนวนอนให้กลายเป็นจุด[cite: 1]
        seq.Append(panelToClose.DOScaleX(0f, 0.1f).SetEase(Ease.OutExpo).SetLink(panelToClose.gameObject)); //[cite: 1]

        // จังหวะที่ 3: แสงวาบ (Flash)[cite: 1]
        if (whiteFlash != null)
        {
            seq.AppendCallback(() =>
            {
                whiteFlash.gameObject.SetActive(true);
                whiteFlash.alpha = 0;
            });
            seq.Append(whiteFlash.DOFade(1f, 0.05f).SetLink(whiteFlash.gameObject)); //[cite: 1]
            seq.Append(whiteFlash.DOFade(0f, 0.2f).SetLink(whiteFlash.gameObject));  //[cite: 1]
            seq.AppendCallback(() => whiteFlash.gameObject.SetActive(false));
        }

        // จังหวะที่ 4: เคลียร์ค่าและเรียก Action[cite: 1]
        seq.OnComplete(() =>
        {
            panelToClose.gameObject.SetActive(false); //[cite: 1]
            panelToClose.localScale = Vector3.one; // รีเซ็ต Scale คืนเผื่อถูกเรียกใช้อีก[cite: 1]

            onComplete?.Invoke(); // รันคำสั่งที่ส่งเข้ามา
        });
    }

    // ==========================================
    // 2. แบบแยกส่วน: เอฟเฟกต์เฟดเปิดหน้าจอ (Fade In)
    // ==========================================
    public void FadeInUI(CanvasGroup targetUI = null, float duration = 1.5f, Action onComplete = null)
    {
        CanvasGroup panelToOpen = targetUI != null ? targetUI : defaultNewUI;

        if (panelToOpen == null) return;

        panelToOpen.alpha = 0; //[cite: 1]
        panelToOpen.gameObject.SetActive(true); //[cite: 1]

        panelToOpen.DOFade(1f, duration) //[cite: 1]
                   .SetUpdate(true)
                   .SetLink(panelToOpen.gameObject)
                   .OnComplete(() => onComplete?.Invoke());
    }

    // ==========================================
    // 3. แบบสำเร็จรูป: ปิดจอ -> ทำ Action บางอย่าง -> เปิดจอใหม่
    // ==========================================
    public void PlayTransition(Action midTransitionAction = null)
    {
        PlayCustomTransition(defaultOldUIPanel, defaultNewUI, midTransitionAction);
    }

    // ==========================================
    // 4. ขั้นสุด: โยน UI ตัวไหนมาเข้า Transition ก็ได้!
    // ==========================================
    public void PlayCustomTransition(RectTransform oldPanel, CanvasGroup newUI, Action midTransitionAction = null)
    {
        TurnOffTV(oldPanel, () =>
        {
            // ทำคำสั่งพิเศษตอนจอดำสนิท (เช่น โหลดเซฟ, เปลี่ยนสเตตัสตัวละคร)
            midTransitionAction?.Invoke();

            // รอให้ Flash จางลงนิดนึงก่อนค่อยเฟดจอใหม่ (ใช้ DOTween ดีเลย์)
            DOVirtual.DelayedCall(0.3f, () => FadeInUI(newUI, 1f), true);
        });
    }
}

