using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    Plane targetedAirCraft;
    float minTargetTime = .25f;
    float lastTargetTime = 0;

    // Use this for initialization
    void Start () {
        targetedAirCraft = null;
    }
	
	// Update is called once per frame
	void Update () {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit = new RaycastHit();
    if (Physics.Raycast(ray, out hit)) {
        if (hit.transform.tag == "Aircraft") {
            if (targetedAirCraft) {
                targetedAirCraft.UnHighlight();
            }
            targetedAirCraft = hit.transform.gameObject.GetComponent<Plane>();
            lastTargetTime = Time.time;
            targetedAirCraft.Highlight();
        }
    }
    if (targetedAirCraft && (lastTargetTime + minTargetTime < Time.time)) {
        targetedAirCraft.UnHighlight();
        targetedAirCraft = null;
    }

    if (targetedAirCraft) {
      CheckInput();
    }
  }

  void CheckInput()
  {
    float direction = -1;
    if (Input.GetKeyDown("left")) {
      direction = 180;
    }
    if (Input.GetKeyDown("right")) {
      direction = 0;
    }
    if (Input.GetKeyDown("up")) {
      direction = 270;
    }
    if (Input.GetKeyDown("down")) {
      direction = 90;
    }
    if (direction != -1) {
        if (targetedAirCraft.TurnAircraft(direction)) {
        targetedAirCraft.UnHighlight();
        targetedAirCraft = null;
        }
    }
  }
}
