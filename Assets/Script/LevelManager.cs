using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [Header("Level Timer Settings")]
    public float gameTime = 180f;
    public float exitTime = 60f;
    private float currentTimer;
    private bool isExitOpen = false;

    [Header("Escape Timer Settings")]
    [SerializeField] private float escapeTime = 300f; // 5 minutes = 300 seconds
    private float escapeCurrentTime;
    private bool isEscapeTimerRunning = false;
    [SerializeField] private WorldBorderMovement border;


    [Header("Scene References")]
    [SerializeField] private GameObject exitObject;
    [SerializeField] private GameObject emergencyObject;
    [SerializeField] private UiManager uiManager;


    [Header("Player Status Settings")]
    public int playerCount = 2;
    private int currentPlayerCount;

    [Header("Sound")]
    public AudioClip exitSound;
    public AudioClip escapeSound;


    public UnityEvent esacpeEvent;
    void Start()
    {

        if (exitObject != null && emergencyObject !=null)
        {
            exitObject.SetActive(false);
            emergencyObject.SetActive(false);
        }

        currentTimer = gameTime;
        currentPlayerCount = playerCount;
        escapeCurrentTime = escapeTime;

        if (uiManager == null)
        {
            uiManager = FindAnyObjectByType<UiManager>();
        }
    }

    void Update()
    {
        if (!isEscapeTimerRunning)
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
        else
        {
            escapeCurrentTime -= Time.deltaTime;

            if (uiManager != null)
            {
                uiManager.UpdateEscapeTimer(escapeCurrentTime);
            }

            if (escapeCurrentTime <= 0)
            {
                isEscapeTimerRunning = false;
                escapeCurrentTime = 0;

                if (border != null)
                {
                    border.ActivateEscapeMode(); // Full map chase
                }
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
        SoundManager.Instance.PlaySFXWithFadeOut(exitSound, 3f, 0.2f);
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


        if (emergencyObject != null) emergencyObject.SetActive(true);
        StartEscapeTimer();
    }


    public void StartEscapeTimer()
    {
        isEscapeTimerRunning = true;
        escapeCurrentTime = escapeTime;
        GameManager.Instance.pointsPerFix = 2;
        esacpeEvent.Invoke();
        SoundManager.Instance.PlaySFXWithFadeOut(escapeSound, 3f, 0.2f);
        Debug.Log("Escape Timer Started!");
       
    }

    public void PlayerEscaped()
    {
        isEscapeTimerRunning = false;

        if (uiManager != null)
        {
            uiManager.HideEscapeTimer();
        }
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

