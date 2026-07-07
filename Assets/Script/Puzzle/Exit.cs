using UnityEngine;

public class Exit : MonoBehaviour
{
    public int requiredPlayers = 2;

    private int currentPlayers = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentPlayers++;
            Debug.Log($"Player entered exit. Current players: {currentPlayers}/{requiredPlayers}");
            CheckExitStatus();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentPlayers--;

            currentPlayers = Mathf.Max(0, currentPlayers);
            Debug.Log($"Player exited exit. Current players: {currentPlayers}/{requiredPlayers}");
        }
    }

    private void OnDisable()
    {
        currentPlayers = 0;
    }

    private void CheckExitStatus()
    {
        if (currentPlayers >= requiredPlayers)
        {
            Debug.Log("All players have reached the exit! Level complete!");
            // Implement level completion logic here

            GameManager.Instance.GameWin();

        }
    }
}
