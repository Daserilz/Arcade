using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.BoolParameter;

public class UiManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_Text timeText;
    public TMP_Text mechanismScoreText;
    public TMP_Text creativeScoreText;
    void Start()
    {
        UpdateScoreText(GameManager.Instance.creativeScore , GameManager.Instance.mechanismScore);
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
}
