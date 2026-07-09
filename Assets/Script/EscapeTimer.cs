using UnityEngine;

public class EscapeTimer : MonoBehaviour
{
    [SerializeField] private float escapeTime = 300f; // 5 minutes = 300 seconds
    private float currentTime;
    private bool timerRunning = true;

    private UiManager ui;
    [SerializeField] WorldBorderMovement border;

    void Start()
    {
        currentTime = escapeTime;
        ui = FindAnyObjectByType<UiManager>();
        ui.UpdateEscapeTimer(currentTime);
    }

    void Update()
    {
        if (!timerRunning) return;

        currentTime -= Time.deltaTime;

        ui.UpdateEscapeTimer(currentTime);


        if (currentTime <= 0)
        {
            timerRunning = false;
            currentTime = 0;

            if (border != null)
                border.ActivateEscapeMode(); // 🔹 full map chase
        }
    }

    public void PlayerEscaped()
    {
        timerRunning = false;

        if (ui != null)
            ui.HideEscapeTimer();
    }
}
