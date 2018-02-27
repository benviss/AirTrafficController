using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager:Singleton<SpawnManager> {

  public List<Transform> aircrafts = new List<Transform>();
  public List<int> tempCounter = new List<int>();
  public List<Plane> planes;
  public int speedMultiplier = 1;
  public float spawnWaitTime = 5;

  private Spawner[] spawners;
  private float timeSinceLastSpawn;
  private int planesOnScreen;


  private void Start() {
    spawners = FindObjectsOfType<Spawner>();
    planes = new List<Plane>();
  }

  void Update() {
    float percentagePlanes = ((float)planes.Count / (float)TheGameManager.Instance.maxCraftsOnScreen);
    spawnWaitTime = Mathf.Lerp(0, 2, percentagePlanes);

    timeSinceLastSpawn += Time.deltaTime;

    if(planes.Count < TheGameManager.Instance.maxCraftsOnScreen) {
      SpawnPlane();
    }
  }

  public void AddSpawnable(Transform newCraft) {
    aircrafts.Add(newCraft);
  }

  void SpawnPlane() {
    if(timeSinceLastSpawn < spawnWaitTime) return;

    int randy = Random.Range(0, spawners.Length);

    if(spawners[randy].CanSpawnPlane()) {
      timeSinceLastSpawn = 0;
      Plane newPlane = spawners[randy].SpawnPlane();
      newPlane.transform.parent = transform;
      planes.Add(newPlane);
    }
  }

  public void KillMe(Plane plane) {
    planes.Remove(plane);
  }
}
