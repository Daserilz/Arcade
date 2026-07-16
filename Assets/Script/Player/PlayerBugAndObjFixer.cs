using UnityEngine;

public class PlayerBugAndObjFixer : MonoBehaviour
{
    [Header("Bug Manager Reference")]
    [SerializeField] private ObjectBug bugManager; // drag your ObjectBug script here
    [SerializeField] private ObjectGone objectManager;

    [Header("Interaction Settings")]
    [SerializeField] private float interactRange = 3f; // how close Player1 must be

    private void Update()
    {
        //// 🔹 Player1 presses Q
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    TryFixBuggedObject();
        //}
    }


    public void PreformAction(Type playerType)
    {
        switch (playerType)
        {
            case Type.Mechanism:
                if (!TryFixBuggedObject())
                {
                    // 2. ถ้าไม่มีบั๊กให้แก้ ลองซ่อนวัตถุ
                    objectManager.TryHideNearestObject(transform.position);
                }
                break;
            case Type.Creative:
                objectManager.TryRespawnHiddenObject(transform.position, interactRange);
                break;
        }
    }

    private bool TryFixBuggedObject()
    {
        if (bugManager == null) return false;

        // Find nearest target object within range
        GameObject nearest = null;
        float nearestDist = Mathf.Infinity;

        foreach (GameObject obj in bugManager.GetTargetObjects())
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

            // 🔹 Award team points only when bugs are actually cleared
            GameManager.Instance.AddScoreMechanism();
            return true;
            //GameManager.Instance.AddTeamScore();
        }
        return false;
    }
}
