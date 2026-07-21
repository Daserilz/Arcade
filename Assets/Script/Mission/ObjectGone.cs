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

    private GameObject currentBrokenObject = null;
    private Coroutine sequenceCoroutine;
    private bool isRespawning = false;

    [SerializeField] private ObjectBug bugManager; // still referenced, but no SetBusy

    private Dictionary<GameObject, Material> originalMaterials = new Dictionary<GameObject, Material>();
    [SerializeField] private Material brokenMaterial; // glowing red material

    void Start()
    {
        foreach (GameObject obj in targetObjects)
        {
            if (obj != null && obj.TryGetComponent<Renderer>(out Renderer rend))
            {
                originalMaterials[obj] = rend.material;
            }
        }

        sequenceCoroutine = StartCoroutine(HideOneObjectAtATime());
    }

    IEnumerator HideOneObjectAtATime()
    {
        while (true)
        {
            if (currentBrokenObject != null)
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

            MarkAsBroken(obj);
            currentBrokenObject = obj;
        }
    }

    void MarkAsBroken(GameObject obj)
    {
        if (obj.TryGetComponent<Renderer>(out Renderer rend))
        {
            rend.material = brokenMaterial;
        }
        Debug.Log("<color=red>Object marked as broken (signal only)!</color>");
    }

    void RestoreObject(GameObject obj)
    {
        if (obj.TryGetComponent<Renderer>(out Renderer rend) && originalMaterials.ContainsKey(obj))
        {
            rend.material = originalMaterials[obj];
        }
        Debug.Log("<color=blue>Object restored to normal!</color>");
    }

    public void TryRespawnHiddenObject(Vector3 playerPos, float interactRange)
    {
        if (currentBrokenObject != null && !isRespawning)
        {
            float distance = Vector3.Distance(playerPos, currentBrokenObject.transform.position);
            if (distance <= interactRange)
            {
                isRespawning = true;
                StartCoroutine(RespawnRoutine(currentBrokenObject));
            }
            else
            {
                Debug.Log("<color=red>Player2 too far to fix object!</color>");
            }
        }
    }

    IEnumerator RespawnRoutine(GameObject obj)
    {
        yield return new WaitForSeconds(respawnDelay);

        if (obj != null)
        {
            RestoreObject(obj);
            GameManager.Instance.AddScoreCreative();
        }

        currentBrokenObject = null;
        isRespawning = false;
    }

    // Player1 hides nearest object
    public void TryHideNearestObject(Vector3 playerPos)
    {
        if (currentBrokenObject != null) return;

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
                GameManager.Instance.AddScoreMechanism();
            }
            else
            {
                MarkAsBroken(nearest);
                currentBrokenObject = nearest;
                Debug.Log("<color=red>Player1 marked object as broken!</color>");
            }
        }
    }
}
