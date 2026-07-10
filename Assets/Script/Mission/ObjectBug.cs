using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectBug : MonoBehaviour
{
    [Header("Bug Settings")]
    [SerializeField] private GameObject bugPrefab;
    public int bugCount = 3;
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 5f;
    public float spawnRadius = 2f;

    [Header("Target Objects")]
    [SerializeField] private List<GameObject> targetObjects;

    private HashSet<GameObject> busyObjects = new HashSet<GameObject>();
    private GameObject currentBuggedObject = null;
    private List<GameObject> spawnedBugs = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnSequentially());
    }

    IEnumerator SpawnSequentially()
    {
        while (true)
        {
            // Wait until Player1 fixes current bugs
            if (currentBuggedObject != null)
            {
                yield return null;
                continue;
            }

            // Pick next object
            GameObject obj = targetObjects[Random.Range(0, targetObjects.Count)];
            if (!obj.activeSelf || busyObjects.Contains(obj))
            {
                yield return null;
                continue;
            }

            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            busyObjects.Add(obj);
            currentBuggedObject = obj;

            // Spawn bugs permanently until cleared
            spawnedBugs.Clear();
            for (int i = 0; i < bugCount; i++)
            {
                Vector3 randomPos = obj.transform.position + Random.insideUnitSphere * spawnRadius;
                randomPos.y = obj.transform.position.y;
                GameObject bug = Instantiate(bugPrefab, randomPos, Quaternion.identity);
                bug.tag = "Bug";
                spawnedBugs.Add(bug);
            }

            Debug.Log("<color=yellow>Bugs spawned on object!</color>");
        }
    }

    public bool IsObjectBusy(GameObject obj)
    {
        return busyObjects.Contains(obj);
    }

    public List<GameObject> GetTargetObjects()
    {
        return targetObjects;
    }

    // Player1 fixes bugs
    public void ClearBugs(GameObject obj)
    {
        foreach (GameObject bug in spawnedBugs)
        {
            if (bug != null) Destroy(bug);
        }

        spawnedBugs.Clear();
        busyObjects.Remove(obj);
        if (currentBuggedObject == obj) currentBuggedObject = null;

        Debug.Log("<color=green>Player1 fixed object, bugs cleared!</color>");
    }
}
