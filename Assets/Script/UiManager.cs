using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [Header("In Game Settings")]
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text mechanismScoreText;
    [SerializeField] private TMP_Text creativeScoreText;

    [Header("Escape Timer Settings")]
    [SerializeField] private TMP_Text escapeTimerText;

    [Header("Game Event Settings")]
    [SerializeField] private TMP_Text eventText;
    [SerializeField] private TMP_Text notiEventactiveText;
    [SerializeField] private GameObject eventUIMenu;

    [Header("Game End Settings")]
    [SerializeField] private GameObject gameEndUI;
    [SerializeField] private TMP_Text totalMScoreText;
    [SerializeField] private TMP_Text totalCScoreText;
    [SerializeField] private TMP_Text flnalScoreText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button backToMenuButton;

    void Start()
    {
        // Initialize UI at start
        UpdateScoreText(GameManager.Instance.creativeScore, GameManager.Instance.mechanismScore);

        gameEndUI.SetActive(false);
        eventUIMenu.SetActive(false);


        if (retryButton != null && backToMenuButton != null)
        {
            retryButton.onClick.AddListener(GameManager.Instance.RestartPlay);
            backToMenuButton.onClick.AddListener(GameManager.Instance.backToManu);
        }

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

    public void UpdateEventUI(GameEventType eventType , bool isEventStart)
    {
        if (isEventStart)
        {
            ActiveEventUIMenu();
            eventText.text = $"Current Event : {eventType}";
            notiEventactiveText.text = $"The event {eventType} has started.";
            Invoke("DisActiveEventUIMenu", 2f);
        }
        else
        {
            eventText.text = $"Current Event : None";
        }
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
