using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectBug : MonoBehaviour
{
    [Header("Bug Settings")]
    public GameObject bugPrefab;          // Assign your black bug prefab
    public int bugCount = 3;              // How many bugs per object
    public float minSpawnDelay = 1f;      // Min delay before bugs spawn
    public float maxSpawnDelay = 5f;      // Max delay before bugs spawn
    public float minBugLifetime = 2f;     // Min time before bug despawns
    public float maxBugLifetime = 3f;     // Max time before bug despawns
    public float spawnRadius = 2f;        // Radius around object to spawn bugs

    [Header("Target Objects")]
    public List<GameObject> targetObjects; // Drag all your map objects here

    void Start()
    {
        // Start the looping sequence
        StartCoroutine(ActivateObjectsSequentially());
    }

    IEnumerator ActivateObjectsSequentially()
    {
        while (true) // loop forever
        {
            foreach (GameObject obj in targetObjects)
            {
                // Wait random delay before spawning bugs for this object
                float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
                yield return new WaitForSeconds(delay);

                // Spawn bugs
                List<GameObject> spawnedBugs = new List<GameObject>();
                for (int i = 0; i < bugCount; i++)
                {
                    Vector3 randomPos = obj.transform.position + Random.insideUnitSphere * spawnRadius;
                    randomPos.y = obj.transform.position.y; // keep on same height
                    GameObject bug = Instantiate(bugPrefab, randomPos, Quaternion.identity);
                    spawnedBugs.Add(bug);
                }

                // Despawn bugs after random lifetime
                float lifetime = Random.Range(minBugLifetime, maxBugLifetime);
                yield return new WaitForSeconds(lifetime);

                foreach (GameObject bug in spawnedBugs)
                {
                    if (bug != null) Destroy(bug);
                }

                // After bugs are gone, move on to next object
            }
        }
    }
}
 