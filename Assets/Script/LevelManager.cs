using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public float gameTime = 180f;
    public float exitTime = 60f;

    private float currentTimer;
    public GameObject exitObject;

    private bool isExitOpen = false;
    private UiManager uiManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (exitObject != null)
        {
            exitObject.SetActive(false);
        }
        currentTimer = gameTime;
        uiManager = FindAnyObjectByType<UiManager>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer -= Time.deltaTime;
        uiManager.UpdateTimeText(currentTimer,isExitOpen);
        if (currentTimer < 0)
        {
            if (!isExitOpen)
            {
                OpenExit();
            }
            else
            {
                CloseExitAndReset();
            }
        }
    }

    private void OpenExit()
    {
        isExitOpen = true;
        if (exitObject != null)
        {
            exitObject.SetActive(true);
        }
        Debug.Log("Exit open");
        currentTimer = exitTime;
    }


    private void CloseExitAndReset()
    {
        isExitOpen = false;

        if (exitObject != null)
        {
            exitObject.SetActive(false);
        }
        Debug.Log("Exit close");
        currentTimer = gameTime;
    }
}

