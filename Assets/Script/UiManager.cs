using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [Header("In Game Settings")]
    public TMP_Text timeText;
    public TMP_Text mechanismScoreText;
    public TMP_Text creativeScoreText;

    [Header("Escape Timer Settings")]
    public TMP_Text escapeTimerText; // 🔹 NEW dedicated text for escape timer

    [Header("Game Event Settings")]
    public TMP_Text eventText;
    public TMP_Text notiEventactiveText;
    public GameObject eventUIMenu;

    [Header("Game End Settings")]
    public GameObject gameEndUI;
    public TMP_Text totalMScoreText;
    public TMP_Text totalCScoreText;
    public TMP_Text flnalScoreText;

    void Start()
    {
        // Initialize UI at start
        UpdateScoreText(GameManager.Instance.creativeScore, GameManager.Instance.mechanismScore);

        gameEndUI.SetActive(false);
        eventUIMenu.SetActive(false);
    }

    public void UpdateTimeText(float time, bool isExit)
    {
        int displayTime = Mathf.CeilToInt(time);

        if (!isExit)
        {
            timeText.text = $"Exit is open in {displayTime} seconds";
        }
        else
        {
            timeText.text = $"Exit is close in {displayTime} seconds";
        }
    }

    // 🔹 Called by GameManager whenever scores change
    public void UpdateScoreText(int creativeScore, int mechanismScore)
    {
        if (creativeScoreText != null) creativeScoreText.text = $"Creative : {creativeScore}";
        if (mechanismScoreText != null) mechanismScoreText.text = $"Mechanism : {mechanismScore}";
    }

    // 🔹 Escape Timer UI
    public void UpdateEscapeTimer(float time)
    {
        if (escapeTimerText == null) return;

        int displayTime = Mathf.CeilToInt(time);
        if (time > 0)
        {
            escapeTimerText.text = $"Timer : {displayTime} seconds";
        }
        else
        {
            escapeTimerText.text = "Time's up! Escape now!";
        }
    }

    public void HideEscapeTimer()
    {
        if (escapeTimerText != null)
            escapeTimerText.gameObject.SetActive(false);
    }

    public void UpdateEventUI(GameEventType eventType)
    {
        ActiveEventUIMenu();
        eventText.text = $"Current Event : {eventType}";
        notiEventactiveText.text = $"The event {eventType} has started.";
        Invoke("DisActiveEventUIMenu", 2f);
    }

    public void ActiveEventUIMenu()
    {
        eventUIMenu.SetActive(true);
    }

    public void DisActiveEventUIMenu()
    {
        eventUIMenu.SetActive(false);
    }

    public void ActiveGameEndUI(int cScore, int mScore)
    {
        gameEndUI.SetActive(true);
        totalMScoreText.text = $"Mechanism Score : {mScore}";
        totalCScoreText.text = $"Creative Score : {cScore}";
        int totalScore = cScore + mScore;
        flnalScoreText.text = $"Total : {totalScore}";
    }
}
