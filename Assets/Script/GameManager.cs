using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Score Settings")]
    [SerializeField] private int pointsPerFix = 10; // same points for both players

    private int teamScore = 0;

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
        SceneManager.LoadScene("MainMenu");
        ResetScore();
    }

    public void GameWin()
    {
        Time.timeScale = 0f;
        uiManager.ActiveGameEndUI(creativeScore, mechanismScore);
        Debug.Log("Game End");
    }

    public void GameLose()
    {
        Time.timeScale = 0f;
        uiManager.ActiveGameEndUI(creativeScore, mechanismScore);
        Debug.Log("Game Over");
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void AddScoreMechanism()
    {
        mechanismScore++;
        Debug.Log($"<color=green>Mechanism Score: {mechanismScore}</color>");

        if (uiManager != null)
            uiManager.UpdateScoreText(creativeScore, mechanismScore);
    }

    public void AddScoreCreative()
    {
        creativeScore++;
        Debug.Log($"<color=magenta>Creative Score: {creativeScore}</color>");

        if (uiManager != null)
            uiManager.UpdateScoreText(creativeScore, mechanismScore);
    }

    public void ResetScore()
    {
        mechanismScore = 0;
        creativeScore = 0;

        if (uiManager != null)
            uiManager.UpdateScoreText(creativeScore, mechanismScore);
    }
}
