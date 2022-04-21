using System.Collections.Generic;
using System.Collections;

using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    public List<int> tempCounter = new List<int>();
    public int speedMultiplier = 1;
    public List<Transform> aircrafts = new List<Transform>();
    public List<Plane> planes;
    public float spawnWaitTime = 5;

    private Spawner[] spawners;
    private float defaultMaxSpawnDelay = 3;
    private float minMaxSpawnDelay = 1;
    private float minMinimumSpawnDelay = .1f;
    private int spawnRetries = 3;

    private void Start()
    {
        spawners = FindObjectsOfType<Spawner>();
        planes = new List<Plane>();

        SpawnPlane();
        SpawnPlane();
        StartCoroutine(SpawnPlanes());
    }

    IEnumerator SpawnPlanes()
    {
        var upperTimeLimit = defaultMaxSpawnDelay - .25f * TheGameManager.Instance.maxCraftsOnScreen;
        upperTimeLimit = Mathf.Clamp(upperTimeLimit, minMaxSpawnDelay, upperTimeLimit);
        var spawnDelay = Random.Range(minMinimumSpawnDelay, upperTimeLimit);

        Debug.Log($"upperlimit {upperTimeLimit} seconds");
        Debug.Log($"waiting {spawnDelay} seconds");
        yield return new WaitForSeconds(spawnDelay);

        Debug.Log($"attempting spawn {this.gameObject.GetInstanceID()}");
        if (planes.Count < TheGameManager.Instance.maxCraftsOnScreen)
        {
            SpawnPlane();
        }

        StartCoroutine(SpawnPlanes());
    }

    public void AddSpawnable(Transform newCraft)
    {
        aircrafts.Add(newCraft);
    }

    void SpawnPlane()
    {
        for (int i=0; i<spawnRetries; i++)
        {
            // TODO add chance to spawn an intentional collision course
            int randomSpawner = Random.Range(0, spawners.Length);

            if (spawners[randomSpawner].CanSpawnPlane())
            {
                int randomAircraft = Random.Range(0, aircrafts.Count);

                Plane newPlane = spawners[randomSpawner].SpawnPlane(aircrafts[randomAircraft], speedMultiplier);
                newPlane.transform.parent = transform;
                planes.Add(newPlane);
                return;
            }
        }

        Debug.Log("Could not spawn plane");
    }

    public void KillMe(Plane plane)
    {
        // we really ought to use a pool.
        planes.Remove(plane);
    }
}
