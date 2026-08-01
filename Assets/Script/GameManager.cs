using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Score Settings")]
    public int pointsPerFix = 1; // same points for both players

    private int teamScore = 0;
    private bool escapeModeActive = false;

    public int mechanismScore;
    public int creativeScore;

    private UiManager uiManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        uiManager = FindAnyObjectByType<UiManager>();
        Debug.Log("Scene loaded, UiManager found: " + (uiManager != null));
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void AddTeamScore()
    {
        teamScore += pointsPerFix;
        Debug.Log($"<color=cyan>Team Score: {teamScore}</color>");

        if (uiManager != null)
            uiManager.UpdateScoreText(creativeScore, mechanismScore);
    }

    public int GetTeamScore() => teamScore;

    public void RestartPlay()
    {
        Time.timeScale = 1f;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        ResetScore();
    }

    public void backToManu()
    {
        Time.timeScale = 1f;
      
        ScoreTransfer.scorePartM = mechanismScore;
        ScoreTransfer.scorePartC = creativeScore;
        ScoreTransfer.hasNewScore = true;

        BlockTransition.Instance.LoadScene("MainMenu");
        ResetScore();
    }

    public void GameWin()
    {
        Time.timeScale = 0f;
        
        SceneTransitionUI.Instance.PlayCustomTransition(uiManager.mainGamePanel, uiManager.gameEndCanvasGroup, () =>
        {
            uiManager.darkPanel.SetActive(true);
            uiManager.ActiveGameEndUI(creativeScore, mechanismScore, true);
        });

        Debug.Log("Game End");
    }

    public void GameLose()
    {
        Time.timeScale = 0f;
        if (escapeModeActive)
        {
            creativeScore = Mathf.CeilToInt(creativeScore / 2f);
            mechanismScore = Mathf.CeilToInt(mechanismScore / 2f);
        }
     
        BlockTransition.Instance.PlayTransition(() =>
        {
            uiManager.ActiveGameEndUI(creativeScore, mechanismScore, false);
        });
        Debug.Log("Game Over");
    }

    public void QuitGame()
    {
        Application.Quit();

    }


    public void AddScoreMechanism()
    {
        mechanismScore += pointsPerFix;
        Debug.Log($"<color=green>Mechanism Score: {mechanismScore}</color>");

        if (uiManager != null)
            uiManager.UpdateScoreText(creativeScore, mechanismScore);
    }

    public void AddScoreCreative()
    {
        creativeScore += pointsPerFix;
        Debug.Log($"<color=magenta>Creative Score: {creativeScore}</color>");

        if (uiManager != null)
            uiManager.UpdateScoreText(creativeScore, mechanismScore);
    }

    public void ResetScore()
    {
        mechanismScore = 0;
        creativeScore = 0;
        pointsPerFix = 1;
        escapeModeActive = false;

        if (uiManager != null)
            uiManager.UpdateScoreText(creativeScore, mechanismScore);
    }

    public void EsacpeSetUp()
    {
        pointsPerFix = 2;
        escapeModeActive = true;
    }

}
