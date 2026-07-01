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

    // Track which objects currently have bugs
    private HashSet<GameObject> busyObjects = new HashSet<GameObject>();

    void Start()
    {
        StartCoroutine(ActivateObjectsSequentially());
    }

    IEnumerator ActivateObjectsSequentially()
    {
        while (true)
        {
            // Pick one random object
            GameObject obj = targetObjects[Random.Range(0, targetObjects.Count)];

            // Skip if hidden or already busy
            if (!obj.activeSelf || busyObjects.Contains(obj))
            {
                yield return null;
                continue;
            }

            // Wait random delay before spawning
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            busyObjects.Add(obj);

            // Spawn bugs
            List<GameObject> spawnedBugs = new List<GameObject>();
            for (int i = 0; i < bugCount; i++)
            {
                Vector3 randomPos = obj.transform.position + Random.insideUnitSphere * spawnRadius;
                randomPos.y = obj.transform.position.y;
                GameObject bug = Instantiate(bugPrefab, randomPos, Quaternion.identity);
                spawnedBugs.Add(bug);
            }

            // Wait for bugs to live their lifetime
            float lifetime = Random.Range(minBugLifetime, maxBugLifetime);
            yield return new WaitForSeconds(lifetime);

            // Despawn bugs
            foreach (GameObject bug in spawnedBugs)
            {
                if (bug != null) Destroy(bug);
            }

            busyObjects.Remove(obj); // free object again
        }
    }

    // 👇 ObjectGone will call this
    public bool IsObjectBusy(GameObject obj)
    {
        return busyObjects.Contains(obj);
    }
}
