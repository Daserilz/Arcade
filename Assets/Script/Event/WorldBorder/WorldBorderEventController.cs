using UnityEngine;

public class WorldBorderEventController : MonoBehaviour
{
    [SerializeField] private GameObject[] borderBlocks;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        foreach (GameObject border in borderBlocks)
        {
            if (border != null)
            {
                border.SetActive(false);
            }
        }
    }

    public void StartBorderEvent()
    {
        if (borderBlocks.Length == 0) return;

        foreach (GameObject border in borderBlocks)
        {
            if (border != null) border.SetActive(true);
            WorldBorderMovement worldBorderMove = border.GetComponent<WorldBorderMovement>();
            worldBorderMove.ActivateEventMode();
        }
    }

    public void StopBorderEvent()
    {
        foreach (GameObject border in borderBlocks)
        {
            if (border != null) border.SetActive(false);
            WorldBorderMovement worldBorderMove = border.GetComponent<WorldBorderMovement>();
            worldBorderMove.StopBorder();
            worldBorderMove.ResetBorder();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
