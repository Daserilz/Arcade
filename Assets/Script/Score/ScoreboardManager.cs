using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class PlayerScore
{
    public string teamName;
    public int scorePartC;
    public int scorePartM;

    // คำนวณคะแนนรวมอัตโนมัติ
    public int TotalScore => scorePartC + scorePartM;
}

[System.Serializable]
public class ScoreboardSaveData
{
    public List<PlayerScore> savedScores = new List<PlayerScore>();
}


public class ScoreboardManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField nameInputField;
    public GameObject inputPanel;
    public GameObject scoreboardPanel;
    public GameObject mainManuPanel;

    [Header("Scoreboard UI")]
    public Transform contentTransform; // Content object ใน Scroll View
    public GameObject rowPrefab;       // Prefab ของแต่ละแถว
    public ScrollRect scrollRect;

    [Header("Current Player UI (Bottom Bar)")]
    public TextMeshProUGUI bottomRankText;
    public TextMeshProUGUI bottomNameText;
    public TextMeshProUGUI bottomScoreText;

    private List<PlayerScore> allScores = new List<PlayerScore>();
    private PlayerScore currentPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadScoreboard();

        if (ScoreTransfer.hasNewScore)
        {
            // เปิดหน้ากรอกชื่อ และปิดหน้า Scoreboard ไว้ก่อน
            inputPanel.SetActive(true);
            mainManuPanel.SetActive(false);
            scoreboardPanel.SetActive(false);
        }
        else
        {
            // ถ้าเข้า Main Menu มาเฉยๆ (ไม่ได้เพิ่งเล่นจบ) 
            // ให้ซ่อนหน้ากรอกชื่อ และอาจจะแสดงหน้า Scoreboard ปกติ
            inputPanel.SetActive(false);
            UpdateScoreboard();
            // หมายเหตุ: ตรงนี้คุณสามารถตั้งให้เปิด scoreboardPanel ทันที หรือปิดไว้ทั้งหมดก็ได้ขึ้นอยู่กับดีไซน์
            // scoreboardPanel.SetActive(true); 
            // UpdateScoreboard(); // เรียกใช้ถ้าอยากให้โชว์ตารางเลย
        }
    }

    public void OnConfirmName()
    {
        if (string.IsNullOrEmpty(nameInputField.text)) return;

        // ดึงคะแนนจาก ScoreTransfer มาใช้ แทนการระบุตัวเลขตรงๆ
        currentPlayer = new PlayerScore
        {
            teamName = nameInputField.text,
            scorePartM = ScoreTransfer.scorePartM,
            scorePartC = ScoreTransfer.scorePartC
        };

        allScores.Add(currentPlayer);
        SaveScoreboard();

        // เปลี่ยนหน้า UI
        inputPanel.SetActive(false);
        scoreboardPanel.SetActive(true);

        // อัปเดตตารางและเล่นแอนิเมชัน
        UpdateScoreboard();

        // รีเซ็ตค่าเพื่อไม่ให้หน้ากรอกชื่อเด้งอีกหากผู้เล่นออกแล้วกลับเข้า Main Menu ใหม่
        ScoreTransfer.hasNewScore = false;
    }

    private void UpdateScoreboard()
    {
        // จัดเรียงคะแนนจากมากไปน้อย
        allScores = allScores.OrderByDescending(p => p.TotalScore).ToList();

        // ล้างข้อมูลเก่าใน Content
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        int currentPlayerRank = 1;
        RectTransform targetRowRect = null;

        // สร้าง UI แถวใหม่
        for (int i = 0; i < allScores.Count; i++)
        {
            GameObject newRow = Instantiate(rowPrefab, contentTransform);
            newRow.transform.localScale = Vector3.one;

            // หา Component Text ภายใน Prefab (ต้องจัดลำดับให้ตรงกัน หรือใช้ Script คุม Prefab)
            TextMeshProUGUI[] texts = newRow.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = (i + 1).ToString(); // Rank
            texts[1].text = allScores[i].teamName; // Name
            texts[2].text = $"{allScores[i].scorePartM}+{allScores[i].scorePartC}={allScores[i].TotalScore}"; // Score format แบบในรูป

            // หากเป็นผู้เล่นปัจจุบัน ให้เก็บค่าเพื่อใช้ทำแอนิเมชันและอัปเดตแถบล่าง
            if (allScores[i] == currentPlayer)
            {
                currentPlayerRank = i + 1;
                targetRowRect = newRow.GetComponent<RectTransform>();

                // Highlight สีตัวหนังสือผู้เล่นปัจจุบัน (แบบแถบล่างในภาพ)
                texts[1].color = Color.yellow;
                texts[2].color = Color.yellow;

                // อัปเดตแถบ Sticky ด้านล่าง
                bottomRankText.text = currentPlayerRank.ToString();
                bottomNameText.text = currentPlayer.teamName;
                bottomScoreText.text = texts[2].text;
            }
        }

        // เริ่มแอนิเมชันเลื่อนหน้าจอไปยังอันดับของผู้เล่น
        if (targetRowRect != null)
        {
            StartCoroutine(ScrollToPlayer(targetRowRect));
        }
    }

    private void GenerateMockupData()
    {
        // ใส่ข้อมูลสมมติเพื่อให้มีให้ Scroll
        string[] names = { "DSase", "ghghgh", "yuiiuiui", "errerer", "ioioioi", "ddfdssdfdfs" };
        foreach (string n in names)
        {
            allScores.Add(new PlayerScore { teamName = n, scorePartC = Random.Range(0, 10), scorePartM = Random.Range(0, 10) });
        }
    }

    private IEnumerator ScrollToPlayer(RectTransform target)
    {
        // รอให้ UI จัด Layout เสร็จสิ้น 1 เฟรม
        yield return new WaitForEndOfFrame();

        // เลื่อน Scroll View แบบ Smooth
        float time = 0f;
        float duration = 1.5f; // เวลาในการเลื่อน (วินาที)

        // คำนวณตำแหน่ง Y ที่ต้องเลื่อนไป
        Canvas.ForceUpdateCanvases();
        Vector2 targetPos = (Vector2)scrollRect.transform.InverseTransformPoint(contentTransform.position)
            - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);

        float startNormalizedY = scrollRect.verticalNormalizedPosition;

        // คำนวณ Normalized Position (0 ถึง 1)
        float targetNormalizedY = 1f - ((float)target.GetSiblingIndex() / (float)(contentTransform.childCount - 1));
        targetNormalizedY = Mathf.Clamp01(targetNormalizedY);

        while (time < duration)
        {
            time += Time.deltaTime;
            // ใช้ Mathf.SmoothStep เพื่อให้การเคลื่อนไหวดูนุ่มนวลขึ้น
            scrollRect.verticalNormalizedPosition = Mathf.SmoothStep(startNormalizedY, targetNormalizedY, time / duration);
            yield return null;
        }
    }

    private void SaveScoreboard()
    {
        // 1. นำข้อมูลใส่ Class Wrapper
        ScoreboardSaveData data = new ScoreboardSaveData();
        data.savedScores = allScores;

        // 2. แปลงเป็นข้อความ JSON
        string json = JsonUtility.ToJson(data);

        // 3. เซฟลง PlayerPrefs
        PlayerPrefs.SetString("ScoreboardSave", json);
        PlayerPrefs.Save();

        Debug.Log("Scoreboard Saved!");
    }

    private void LoadScoreboard()
    {
        // เช็คว่าเคยมีการเซฟข้อมูลไว้หรือไม่
        if (PlayerPrefs.HasKey("ScoreboardSave"))
        {
            string json = PlayerPrefs.GetString("ScoreboardSave");
            ScoreboardSaveData data = JsonUtility.FromJson<ScoreboardSaveData>(json);

            allScores = data.savedScores;
            Debug.Log("Scoreboard Loaded!");
        }
        else
        {
            // ถ้ายังไม่เคยเซฟเลย (เล่นครั้งแรก) ค่อยสร้างข้อมูลจำลอง
           //GenerateMockupData();
        }
    }

    public void ClearScoreboardData()
    {
        PlayerPrefs.DeleteKey("ScoreboardSave");
        PlayerPrefs.Save();
        allScores.Clear();
        UpdateScoreboard();
    }
}
