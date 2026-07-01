using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectGone : MonoBehaviour
{
    [Header("Target Objects")]
    public List<GameObject> targetObjects;

    [Header("Timing Settings")]
    public float minDelay = 1f;
    public float maxDelay = 5f;
    public float respawnDelay = 2f;
    public float respawnRange = 3f; // distance Player2 must be within

    private GameObject currentHiddenObject = null;
    private Coroutine sequenceCoroutine;

    public ObjectBug bugManager;
    public Transform player2; // assign Player2 transform here

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

    public void TryRespawnHiddenObject()
    {
        if (currentHiddenObject != null && player2 != null)
        {
            float distance = Vector3.Distance(player2.position, currentHiddenObject.transform.position);
            if (distance <= respawnRange)
            {
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

        if (obj != null) obj.SetActive(true);

        currentHiddenObject = null;
    }
}
