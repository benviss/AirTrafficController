using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner:MonoBehaviour {



  public enum WallSide {
    Top, Right, Bottom, Left
  }

  public WallSide wallSide;

  private void Start() {
    if(transform.parent.name.Equals("Top Spawners")) {
      wallSide = WallSide.Top;
    }
    else if(transform.parent.name.Equals("Right Spawners")) {
      wallSide = WallSide.Right;
    }
    else if(transform.parent.name.Equals("Bottom Spawners")) {
      wallSide = WallSide.Bottom;
    }
    else if(transform.parent.name.Equals("Left Spawners")) {
      wallSide = WallSide.Left;
    }
  }

  public bool CanSpawnPlane() {

    Collider[] hitColliders = Physics.OverlapSphere(transform.position, 50);

    foreach(Collider col in hitColliders) {
      if(col.gameObject.CompareTag("Aircraft")) {
        return false;
      }
    }

    return true;
  }

  public Plane SpawnPlane() {
    //choose an aircraft and instantiate the game object at this spawners position and rotation
    int randy = Mathf.Abs(Random.Range(0, SpawnManager.Instance.aircrafts.Count) - Random.Range(0, SpawnManager.Instance.aircrafts.Count));
    //int randy = Mathf.RoundToInt(Mathf.Floor(Mathf.Abs(Random.Range(0, 1) - Random.Range(0, 1)) * ((Random.Range(0, 1) + 2) - SpawnManager.Instance.aircrafts.Count) + SpawnManager.Instance.aircrafts.Count));

    Plane newPlane = Instantiate(SpawnManager.Instance.aircrafts[randy], transform.position, transform.rotation).GetComponent<Plane>();

    newPlane.InitAircraft(SpawnManager.Instance.speedMultiplier, wallSide);

    return newPlane;
  }
}
