using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int mechanismScore;
    public int creativeScore;

    private UiManager uiManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    // ยกเลิกรับ Event เมื่อ Object ถูกปิดหรือทำลาย (ป้องกัน Memory Leak)
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ค้นหา UiManager ของ Scene ใหม่
        uiManager = FindAnyObjectByType<UiManager>();

        Debug.Log("โหลด Scene ใหม่แล้ว หา UiManager เจอหรือไม่: " + (uiManager != null));
    }

    public void NextLevel() // อาจจะมีการย้าย code ในอนาคต
    {
        Time.timeScale = 1f;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void RestartPlay() // อาจจะมีการย้าย code ในอนาคต
    {
        Time.timeScale = 1f;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        ResetScore();
    }

    public void GameWin() // อาจจะมีการย้าย code ในอนาคต
    {
        Time.timeScale = 0f;
        uiManager.ActiveGameEndUI(creativeScore,mechanismScore);
        Debug.Log("Game End");
    }

    public void addScoreMechanism()
    {
        mechanismScore++;
        uiManager.UpdateScoreText(creativeScore,mechanismScore);
    }

    public void addScoreCreative()
    {
        creativeScore++;
        uiManager.UpdateScoreText(creativeScore, mechanismScore);
    }

    public void ResetScore()
    {
        mechanismScore = 0;
        creativeScore = 0;
    }
}
