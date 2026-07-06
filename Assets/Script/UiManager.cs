using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_Text timeText;
    void Start()
    {
        //UpdateScoreText(10 , 10); // gamemanager score
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
        
    }
}
