using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;

    [Header("Tutorial Pages")]
    public List<GameObject> pages = new List<GameObject>();
    public Button backButton;
    public Button nextButton;

    private int currentPage = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //playButton.onClick.AddListener(GameManager.Instance.NextLevel);
        exitButton.onClick.AddListener(GameManager.Instance.QuitGame);

        backButton.onClick.AddListener(PreviousPage);
        nextButton.onClick.AddListener(NextPage);

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void NextPage()
    {
        if (currentPage < pages.Count - 1)
        {
            // ยังไม่ถึงหน้าสุดท้าย
            currentPage++;
            UpdateUI();
        }
        else
        {
            // อยู่หน้าสุดท้ายแล้ว เปลี่ยน Scene
            BlockTransition transition = FindAnyObjectByType<BlockTransition>();
            transition.PlayTransition("Lv1"); // เปลี่ยนเป็นชื่อ Scene ของ Level 1
            //GameManager.Instance.NextLevel();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        // เปิดหน้าปัจจุบัน และปิดหน้าอื่นๆ
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == currentPage);
        }

        // ซ่อนปุ่ม Back ถ้าอยู่หน้าแรก (เปิดใช้งานถ้าไม่ได้อยู่หน้าแรก)
        backButton.gameObject.SetActive(currentPage > 0);

    }
}
