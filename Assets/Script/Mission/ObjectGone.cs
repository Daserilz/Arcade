using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectGone : MonoBehaviour
{
    [Header("Target Objects")]
    [SerializeField] private List<GameObject> targetObjects;

    [Header("Timing Settings")]
    public float minDelay = 1f;
    public float maxDelay = 5f;
    public float respawnDelay = 2f;
    public float respawnRange = 3f;

    private GameObject currentHiddenObject = null;
    private Coroutine sequenceCoroutine;
    private bool isRespawning = false; // 🔹 NEW: prevents double scoring

    [SerializeField] private ObjectBug bugManager;
    [SerializeField] private Transform player2; // assign Player2 transform here

    void Start()
    {
        sequenceCoroutine = StartCoroutine(HideOneObjectAtATime());
    }

    IEnumerator HideOneObjectAtATime()
    {
        while (true)
        {
            if (currentHiddenObject != null)
            {
                yield return null;
                continue;
            }

            GameObject obj = targetObjects[Random.Range(0, targetObjects.Count)];

            if (bugManager != null && bugManager.IsObjectBusy(obj))
            {
                yield return null;
                continue;
            }

            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            obj.SetActive(false);
            currentHiddenObject = obj;
        }
    }

    // 🔹 Player2 respawns hidden object
    public void TryRespawnHiddenObject()
    {
        if (currentHiddenObject != null && player2 != null && !isRespawning)
        {
            float distance = Vector3.Distance(player2.position, currentHiddenObject.transform.position);
            if (distance <= respawnRange)
            {
                isRespawning = true; // lock until finished
                StartCoroutine(RespawnRoutine(currentHiddenObject));
            }
            else
            {
                Debug.Log("<color=red>Player2 too far to respawn object!</color>");
            }
        }
    }

    IEnumerator RespawnRoutine(GameObject obj)
    {
        yield return new WaitForSeconds(respawnDelay);

        if (obj != null)
        {
            obj.SetActive(true);
            Debug.Log("<color=blue>Player2 respawned object!</color>");

            // 🔹 Award team points only once per respawn
            //GameManager.Instance.AddTeamScore();
            GameManager.Instance.AddScoreCreative();
        }

        currentHiddenObject = null;
        isRespawning = false; // unlock for next time
    }

    // 🔹 Player1 hides nearest object (similar to Player2 respawn)
    public void TryHideNearestObject(Vector3 playerPos)
    {
        if (currentHiddenObject != null) return;

        GameObject nearest = null;
        float nearestDist = Mathf.Infinity;

        foreach (GameObject obj in targetObjects)
        {
            if (obj == null || !obj.activeSelf) continue;

            float dist = Vector3.Distance(playerPos, obj.transform.position);
            if (dist < nearestDist)
            {
                nearest = obj;
                nearestDist = dist;
            }
        }

        if (nearest != null)
        {
            if (bugManager != null && bugManager.IsObjectBusy(nearest))
            {
                bugManager.ClearBugs(nearest);
                Debug.Log("<color=red>Player1 cleared bugs!</color>");

                // 🔹 Award team points when bugs are cleared
                GameManager.Instance.AddScoreMechanism();
                //GameManager.Instance.AddTeamScore();
               
            }
            else
            {
                nearest.SetActive(false);
                currentHiddenObject = nearest;
                Debug.Log("<color=red>Player1 hid object!</color>");
            }
        }
    }
}
