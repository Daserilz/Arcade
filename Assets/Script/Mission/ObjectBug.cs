using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectBug : MonoBehaviour
{
    [Header("Bug Settings")]
    public GameObject bugPrefab;
    public int bugCount = 3;
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 5f;
    public float minBugLifetime = 2f;
    public float maxBugLifetime = 3f;
    public float spawnRadius = 2f;

    [Header("Target Objects")]
    public List<GameObject> targetObjects;

    private HashSet<GameObject> busyObjects = new HashSet<GameObject>();

    void Start()
    {
        StartCoroutine(ActivateObjectsSequentially());
    }

    IEnumerator ActivateObjectsSequentially()
    {
        while (true)
        {
            GameObject obj = targetObjects[Random.Range(0, targetObjects.Count)];

            if (!obj.activeSelf || busyObjects.Contains(obj))
            {
                yield return null;
                continue;
            }

            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            busyObjects.Add(obj);

            List<GameObject> spawnedBugs = new List<GameObject>();
            for (int i = 0; i < bugCount; i++)
            {
                Vector3 randomPos = obj.transform.position + Random.insideUnitSphere * spawnRadius;
                randomPos.y = obj.transform.position.y;
                GameObject bug = Instantiate(bugPrefab, randomPos, Quaternion.identity);
                bug.tag = "Bug"; // ensure prefab has Bug tag
                spawnedBugs.Add(bug);
            }

            float lifetime = Random.Range(minBugLifetime, maxBugLifetime);
            yield return new WaitForSeconds(lifetime);

            foreach (GameObject bug in spawnedBugs)
            {
                if (bug != null) Destroy(bug);
            }

            busyObjects.Remove(obj);
        }
    }

    public bool IsObjectBusy(GameObject obj)
    {
        return busyObjects.Contains(obj);
    }

    // 👇 Player1 calls this
    public void ClearBugs(GameObject obj)
    {
        foreach (GameObject bug in GameObject.FindGameObjectsWithTag("Bug"))
        {
            if (bug != null && Vector3.Distance(obj.transform.position, bug.transform.position) < spawnRadius * 2f)
            {
                Destroy(bug);
            }
        }

        busyObjects.Remove(obj);
        Debug.Log("<color=red>Bugs cleared on object!</color>");
    }
}
