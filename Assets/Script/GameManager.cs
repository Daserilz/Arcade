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

    // ¡��ԡ�Ѻ Event ����� Object �١�Դ���ͷ���� (��ͧ�ѹ Memory Leak)
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� UiManager �ͧ Scene ����
        uiManager = FindAnyObjectByType<UiManager>();

        Debug.Log("��Ŵ Scene �������� �� UiManager ���������: " + (uiManager != null));
    }

    public void NextLevel() // �Ҩ���ա������ code �͹Ҥ�
    {
        Time.timeScale = 1f;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    // 🔹 Add score only when an object is actually fixed/restored
    public void AddTeamScore()
    {
        teamScore += pointsPerFix;
        Debug.Log($"<color=cyan>Team Score: {teamScore}</color>");
    }

    public int GetTeamScore() => teamScore;

    public void RestartPlay() // �Ҩ���ա������ code �͹Ҥ�
    {
        Time.timeScale = 1f;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        ResetScore();
    }

    public void GameWin() // �Ҩ���ա������ code �͹Ҥ�
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
