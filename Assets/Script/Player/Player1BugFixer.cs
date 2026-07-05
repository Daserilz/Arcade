using UnityEngine;

public class Player1BugFixer : MonoBehaviour
{
    [Header("Bug Manager Reference")]
    [SerializeField] private ObjectBug bugManager; // drag your ObjectBug script here

    [Header("Interaction Settings")]
    [SerializeField] private float interactRange = 3f; // how close Player1 must be

    private void Update()
    {
        // 🔹 Player1 presses Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryFixBuggedObject();
        }
    }

    private void TryFixBuggedObject()
    {
        if (bugManager == null) return;

        // Find nearest target object within range
        GameObject nearest = null;
        float nearestDist = Mathf.Infinity;

        foreach (GameObject obj in bugManager.targetObjects)
        {
            if (obj == null || !obj.activeSelf) continue;

            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist < interactRange && dist < nearestDist)
            {
                nearest = obj;
                nearestDist = dist;
            }
        }

        // If nearest object is busy (has bugs), clear them
        if (nearest != null && bugManager.IsObjectBusy(nearest))
        {
            bugManager.ClearBugs(nearest);
            Debug.Log("<color=green>Player1 fixed bugs on object!</color>");
        }
    }
}
