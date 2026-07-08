using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Score Settings")]
    [SerializeField] private int pointsPerFix = 10; // same points for both players

    private int teamScore = 0;

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

    public void NextLevel()
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
}
