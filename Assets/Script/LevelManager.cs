using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public float gameTime = 180f;
    public float exitTime = 60f;

    private float currentTimer;
    [SerializeField] private GameObject exitObject;

    private bool isExitOpen = false;
    [SerializeField] private UiManager uiManager;


    [Header("Player Status Settings")]
    public int playerCount = 2;
    private int currentPlayerCount;
    
    void Start()
    {
        if (exitObject != null)
        {
            exitObject.SetActive(false);
        }
        currentTimer = gameTime;
        currentPlayerCount = playerCount;
        
        if (uiManager == null)
        {
            uiManager = FindAnyObjectByType<UiManager>();
        }
    }

    void Update()
    {
        currentTimer -= Time.deltaTime;
        
        if (uiManager != null)
        {
            uiManager.UpdateTimeText(currentTimer, isExitOpen);
        }
        
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

    public void RemovePlayer()
    {
        currentPlayerCount -= 1;
        if (currentPlayerCount <= 0 )
        {
            GameManager.Instance.GameLose();
        }
    }
}

