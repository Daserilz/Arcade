using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.BoolParameter;


public class UiManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("In Game Settings")]
    public TMP_Text timeText;
    public TMP_Text mechanismScoreText;
    public TMP_Text creativeScoreText;
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
        UpdateScoreText(GameManager.Instance.creativeScore , GameManager.Instance.mechanismScore);
        gameEndUI.SetActive(false);
        eventUIMenu.SetActive(false);
    }

    public void UpdateTimeText(float time , bool isExit)
    {
        int displayTime = Mathf.CeilToInt(time);

        if (!isExit)
        {
            timeText.text = $"Exit is open in {displayTime} seconds ";
        }
        else
        {
            timeText.text = $"Exit is close in {displayTime} seconds ";
        }
    }



    public void UpdateScoreText(int creativeScore , int mechanismScore)
    {
        creativeScoreText.text = $"Creative : {creativeScore} ";
        mechanismScoreText.text = $"Mechanism : {mechanismScore} ";
    }

    public void UpdateEventUI(GameEventType eventType)
    {
        ActiveEventUIMenu();
        eventText.text = $"Current Event : {eventType} ";
        notiEventactiveText.text = $"The event {eventType} has start.";
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

    public void ActiveGameEndUI(int cScore , int mScore)
    {
        gameEndUI.SetActive(true);
        totalMScoreText.text = $"Mechanism Score : {mScore} ";
        totalCScoreText.text = $"Creative Score : {cScore} ";
        int totalScore = cScore + mScore;
        flnalScoreText.text = $"Total : {totalScore} ";
    }

}
