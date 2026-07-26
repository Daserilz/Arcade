using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionUI : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform oldUIPanel; // หน้าจอเดิมที่จะบีบให้ดับ
    public CanvasGroup newUI;        // หน้าจอใหม่ที่จะเฟดขึ้นมา
    public CanvasGroup whiteFlash;   // (ทางเลือก) แสงวาบสีขาวตอนจอดับ

    public void PlayTVOffTransition()
    {
        // 1. เตรียมสถานะเริ่มต้น
        newUI.alpha = 0;
        newUI.gameObject.SetActive(false);
        if (whiteFlash != null) whiteFlash.alpha = 0;

        // 2. สร้างคิวแอนิเมชัน
        Sequence seq = DOTween.Sequence();

        // จังหวะที่ 1: บีบจอแนวตั้งให้แบนเป็นเส้น (ใช้เวลา 0.15 วินาที)
        // ใช้ Ease.InCubic เพื่อให้เริ่มช้าแล้วพุ่งลงมาเร็วๆ
        seq.Append(oldUIPanel.DOScaleY(0.01f, 0.2f).SetEase(Ease.InCubic));

        // (ทางเลือก) ขยายแกน X ออกไปนิดนึงตอนบีบแนวตั้ง เพื่อให้ดูเป็นจอแก้วโป่งๆ
        seq.Join(oldUIPanel.DOScaleX(1.05f, 0.15f));

        // จังหวะที่ 2: หดเส้นแนวนอนให้กลายเป็นจุดแล้วหายไป (ใช้เวลา 0.1 วินาที)
        // ใช้ Ease.OutExpo ให้มันกระตุกหายไปอย่างรวดเร็ว
        seq.Append(oldUIPanel.DOScaleX(0f, 0.1f).SetEase(Ease.OutExpo));

        // จังหวะที่ 3: แสงวาบ (Flash) 
        if (whiteFlash != null)
        {
            seq.AppendCallback(() => whiteFlash.gameObject.SetActive(true));
            seq.Append(whiteFlash.DOFade(1f, 0.05f));
            seq.Append(whiteFlash.DOFade(0f, 0.2f));
        }

        // จังหวะที่ 4: สลับสถานะ GameObject ตอนที่จอดับสนิท
        seq.AppendCallback(() =>
        {
            oldUIPanel.gameObject.SetActive(false);

            // รีเซ็ต Scale ของหน้าจอเก่าเผื่อนำกลับมาใช้ใหม่
            oldUIPanel.localScale = Vector3.one;

            newUI.gameObject.SetActive(true);
        });

        // จังหวะที่ 5: เฟด UI ใหม่ขึ้นมา (รอให้ Flash จางลงนิดนึงก่อนค่อยเฟด)
        seq.AppendInterval(0.5f);
        seq.Append(newUI.DOFade(1f, 1f));

        // จบงาน
        if (whiteFlash != null)
        {
            seq.OnComplete(() => whiteFlash.gameObject.SetActive(false));
        }
    }
}

