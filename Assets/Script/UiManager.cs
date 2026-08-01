using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class UiManager : MonoBehaviour
{
  
    [Header("In Game Settings")]
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private List<TMP_Text> mechanismScoreText = new List<TMP_Text>();
    [SerializeField] private List<TMP_Text> creativeScoreText = new List<TMP_Text>();

    [Header("Escape Timer Settings")]
    [SerializeField] private List<TMP_Text> escapeTimerText = new List<TMP_Text>();

    [Header("Game Event Settings")]
    [SerializeField] private List<TMP_Text> eventText = new List<TMP_Text>();
    [SerializeField] private TMP_Text notiEventactiveText;
    [SerializeField] private GameObject eventUIMenu;

    [Header("Game End Settings")]
    [SerializeField] private GameObject gameEndUI;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject LoseUI;
    [SerializeField] private TMP_Text totalMScoreText;
    [SerializeField] private TMP_Text totalCScoreText;
    [SerializeField] private TMP_Text flnalScoreText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button backToMenuButton;

    [Header("Game Transition Settings")]
    public GameObject darkPanel;
    public RectTransform mainGamePanel;
    [HideInInspector] public CanvasGroup gameEndCanvasGroup;

    void Start()
    {
        // Initialize UI at start
        UpdateScoreText(GameManager.Instance.creativeScore, GameManager.Instance.mechanismScore);

        darkPanel.SetActive(false);
        gameEndCanvasGroup = gameEndUI.GetComponent<CanvasGroup>();

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
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        string timesText = $"{minutes:00}:{seconds:00}";



        if (!isExit)
        {
            timeText.text = $"Exit open in {timesText} ";
        }
        else
        {
            timeText.text = $"Exit close in {timesText}";
        }
    }

    // 🔹 Called by GameManager whenever scores change
    public void UpdateScoreText(int creativeScore, int mechanismScore)
    {
        if (creativeScoreText != null)
        {
            foreach (var scoreText in creativeScoreText)
            {
                if (scoreText != null)
                {
                    scoreText.text = $"Designer Obj Restore : {creativeScore}";
                }
            }
        }
        if (mechanismScoreText != null) 
        { 
            foreach (var scoreText in mechanismScoreText)
            {
                if (scoreText != null)
                {
                    scoreText.text = $"Developer Error Fix : {mechanismScore}";
                }
            }
        } 
    }

    // 🔹 Escape Timer UI
    public void UpdateEscapeTimer(float time)
    {
        if (escapeTimerText == null) return;

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        string displayTime = $"{minutes:00}:{seconds:00}";

        if (time > 0)
        {
            foreach (var escapeTimer in escapeTimerText)
            {
                if (escapeTimer != null)
                {
                    escapeTimer.text = $"BORDER is coming in {displayTime} ";
                }
            }
        }
        else
        {
            foreach (var escapeTimer in escapeTimerText)
            {
                if (escapeTimer != null)
                {
                    escapeTimer.text = "BRODER is COME! Escape now!";
                }
            }
        }
    }

    public void HideEscapeTimer()
    {
        if (escapeTimerText != null)
        {
            foreach (var escapeTimer in escapeTimerText)
            {
                if (escapeTimer != null)
                {
                    escapeTimer.gameObject.SetActive(false);
                }
            }
        }
           
    }

    public void UpdateEventUI(GameEventType eventType , bool isEventStart)
    {
        if (isEventStart)
        {
            ActiveEventUIMenu();
            foreach (var eventText in eventText)
            {
                if (eventText != null)
                {
                    eventText.text = $"Current Event : {eventType}";
                }
            }
            notiEventactiveText.text = $"The event {eventType} has started.";
            Invoke("DisActiveEventUIMenu", 2f);
        }
        else
        {
            foreach (var eventText in eventText)
            {
                if (eventText != null)
                {
                    eventText.text = $"Current Event : None";
                }
            }
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

    public void ActiveGameEndUI(int cScore, int mScore , bool isWin)
    {
      
        if (isWin)
        {
            winUI.SetActive(true);
            LoseUI.SetActive(false);
        }
        else
        {
            gameEndUI.SetActive(true);
            LoseUI.SetActive(true);
            winUI.SetActive(false);
        }

        totalMScoreText.text = $"Developer Error Fix  : {mScore}";
        totalCScoreText.text = $"Designer Obj Restore : {cScore}";
        int totalScore = cScore + mScore;
        flnalScoreText.text = $"Score Total : {totalScore}";

      

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backToMenuButton.gameObject);
    }
}
