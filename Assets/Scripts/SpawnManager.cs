using System.Collections.Generic;
using System.Collections;

using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int speedMultiplier = 1;
    public List<Transform> spawnableAircrafts = new List<Transform>();
    public List<Plane> planes;
    public float spawnWaitTime = 5;

    private Spawner[] _spawners;

    private const float _defaultMaxSpawnDelay = 3;
    private const float _minMaxSpawnDelay = 1;
    private const float _minMinimumSpawnDelay = .1f;
    private const int spawnRetries = 5;

    private void Awake()
    {
        _spawners = GetComponentsInChildren<Spawner>();
    }

    public void StartSpawner()
    {
        // clean up and planes before starting to spawn new ones
        foreach (var plane in planes)
        {
            Destroy(plane.gameObject);
        }

        speedMultiplier = 1;
        spawnWaitTime = 5;

        planes = new List<Plane>();

        SpawnPlane();
        SpawnPlane();
        StartCoroutine(SpawnPlanes());
    }

    IEnumerator SpawnPlanes()
    {
        var upperTimeLimit = _defaultMaxSpawnDelay - .25f * App.Instance.maxCraftsOnScreen;
        upperTimeLimit = Mathf.Clamp(upperTimeLimit, _minMaxSpawnDelay, upperTimeLimit);
        var spawnDelay = Random.Range(_minMinimumSpawnDelay, upperTimeLimit);

        yield return new WaitForSeconds(spawnDelay);

        if (planes.Count < App.Instance.maxCraftsOnScreen)
        {
            try
            {
                SpawnPlane();
            }
            catch
            {
                Debug.LogWarning("exception on spawning plane");
            }
        }

        StartCoroutine(SpawnPlanes());
    }

    public void AddSpawnable(Transform newCraft)
    {
        spawnableAircrafts.Add(newCraft);
    }

    void SpawnPlane()
    {
        for (int i=0; i<spawnRetries; i++)
        {
            // TODO add chance to spawn an intentional collision course
            int randomSpawner = Random.Range(0, _spawners.Length);

            if (_spawners[randomSpawner].CanSpawnPlane())
            {
                int randomAircraft = Random.Range(0, spawnableAircrafts.Count);

                Plane newPlane = _spawners[randomSpawner].SpawnPlane(spawnableAircrafts[randomAircraft], speedMultiplier);
                newPlane.transform.parent = transform;
                newPlane.OnPlaneCrashed += HandlePlaneCrashed;
                planes.Add(newPlane);
                return;
            }
        }

        Debug.Log("Could not spawn plane");
    }

    public void HandlePlaneCrashed(Plane plane)
    {
        // we really ought to use a pool.
        planes.Remove(plane);
    }
}
