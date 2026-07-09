using UnityEngine;

public class EscapeTimer : MonoBehaviour
{
    [SerializeField] private float escapeTime = 300f; // 5 minutes = 300 seconds
    private float currentTime;
    private bool timerRunning = true;

    void Start()
    {
        currentTime = escapeTime;
        UiManager ui = FindObjectOfType<UiManager>();
        ui.UpdateEscapeTimer(currentTime);
    }

    void Update()
    {
        if (!timerRunning) return;

        currentTime -= Time.deltaTime;

        UiManager ui = FindObjectOfType<UiManager>();
        ui.UpdateEscapeTimer(currentTime);


        if (currentTime <= 0)
        {
            timerRunning = false;
            currentTime = 0;

            WorldBorderMovement border = FindObjectOfType<WorldBorderMovement>();
            if (border != null)
                border.ActivateEscapeMode(); // 🔹 full map chase
        }
    }

    public void PlayerEscaped()
    {
        timerRunning = false;

        UiManager ui = FindObjectOfType<UiManager>();
        if (ui != null)
            ui.HideEscapeTimer();
    }
}
