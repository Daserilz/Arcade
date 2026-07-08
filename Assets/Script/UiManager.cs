using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [Header("In Game Settings")]
    public TMP_Text timeText;
    public TMP_Text mechanismScoreText;
    public TMP_Text creativeScoreText;
    public TMP_Text teamScoreText; // 🔹 NEW: shows team score

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
        UpdateTeamScore(GameManager.Instance.GetTeamScore());

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
        UpdateTeamScore(GameManager.Instance.GetTeamScore());
    }

    public void UpdateTeamScore(int teamScore)
    {
        if (teamScoreText != null) teamScoreText.text = $"Team : {teamScore}";
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
