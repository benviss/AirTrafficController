using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane:MonoBehaviour {

  public float speed = 5;
  public float baseSpeed = 5;
  public float speedRange = 5;
  public GameObject explosion;

  // signal lights
  public GameObject leftLight;
  public GameObject rightLight;
  public GameObject tailLight;

  private HighlightsFX outlineHighlight;
  private Renderer rend;
  private Color outlineColor;

  public Collector.WallSide finalDestination;

  enum Headings {
    Up, Right, Down, Left
  }

  Headings currentHeading;
  Headings nextHeading;

  void Start() {
    outlineHighlight = FindObjectOfType<Camera>().GetComponent<HighlightsFX>();
    rend = this.GetComponentInChildren<Renderer>();
    outlineColor = new Color(1f, 1f, 1f, 1f);
    nextHeading = currentHeading;
  }

  // Update is called once per frame
  void Update() {
    Vector3 forwardMovement = GetForwardMovement();
    transform.position += forwardMovement * speed * Time.deltaTime;

    float targetRotation = GetTargetRotation(nextHeading);
    float currentHeadingRotation = GetTargetRotation(currentHeading);
    if (nextHeading != currentHeading) {
      float angle = Mathf.LerpAngle(transform.rotation.eulerAngles.y, targetRotation, Time.deltaTime * speed);
      if (angle >= 356) {
        angle = 0;
      }
      if (Mathf.Abs(targetRotation - angle) < 4f) {
        angle = targetRotation;
      }

      transform.eulerAngles = new Vector3(0, angle, 0);
      if (targetRotation == transform.rotation.eulerAngles.y) {
        SetCurrentHeading(nextHeading);
      }
    }
  }

  Vector3 GetForwardMovement() {
    switch (nextHeading) {
      case Headings.Up:
        return Vector3.forward;
      case Headings.Right:
        return Vector3.right;
      case Headings.Down:
        return Vector3.back;
      case Headings.Left:
        return Vector3.left;        
      default:
        return Vector3.right;
    }
  }

  float GetTargetRotation(Headings heading){
    switch (heading) {
      case Headings.Up:
        return 270;
      case Headings.Right:
        return 0;
      case Headings.Down:
        return 90;
      case Headings.Left:
        return 180;
      default:
        return 0;
    }
  }

  public bool TurnAircraft(float direction) {
    if (currentHeading != nextHeading) return false;
    
    float currentRotation = GetTargetRotation(nextHeading);

    if((Mathf.DeltaAngle(currentRotation, direction) > 90) || (Mathf.DeltaAngle(currentRotation, direction) == 0)) return false;
    
    AudioManager.Instance.PlayTurnSound();

    if (direction == 0) {
      nextHeading = Headings.Right;
    } else if (direction == 90) {
      nextHeading = Headings.Down;
    } else if (direction == 180){
      nextHeading = Headings.Left;
    } else {
      nextHeading = Headings.Up;
    }
    // update curretntHeading

    return true;
  }


  private void OnTriggerEnter(Collider other) {
    if(other.gameObject.layer.Equals(11)) {
      GameObject newObject = (GameObject)Instantiate(explosion, this.transform.position, this.transform.rotation);
      Destroy(newObject, 3f);
      Destroy(this.gameObject);
      //GameManger.GameOver()
    }
    //else if(other.gameObject.layer.Equals(9)) {
      // if it is correct collector tell game manager of success and destroy aircraft
      //if(other.gameObject.GetComponent<Collector>().side == finalDestination) {
      //  TheGameManager.Instance.planesCollected++;
      //  Destroy(gameObject);
      //}
      else {
        switch (currentHeading) {
          case Headings.Up:
            nextHeading = Headings.Down;
            break;
          case Headings.Right:
            nextHeading = Headings.Left;
            break;
          case Headings.Down:
            nextHeading = Headings.Up;
            break;
          case Headings.Left:
            nextHeading = Headings.Right;
            break;
        }

      }
    }

  public void InitAircraft(int speedMultiplier, Spawner.WallSide startingWall) {

    speed = Random.Range((baseSpeed + speedMultiplier), ((baseSpeed + speedMultiplier) + speedRange));

    // Set the planes final destiantion so we know when to score it and which signal light to set active
    switch(startingWall) {

      case Spawner.WallSide.Top:
        finalDestination = Collector.WallSide.Bottom;
        SetCurrentHeading(Headings.Down);
        break;

      case Spawner.WallSide.Right:
        finalDestination = Collector.WallSide.Left;
        SetCurrentHeading(Headings.Left);
        break;

      case Spawner.WallSide.Bottom:
        finalDestination = Collector.WallSide.Top;
        SetCurrentHeading(Headings.Up);
        break;

      case Spawner.WallSide.Left:
        finalDestination = Collector.WallSide.Right;
        SetCurrentHeading(Headings.Right);
        break;
    }
  }

  public void Highlight() {
    outlineHighlight.ClearOutlineData();
    outlineHighlight.AddRenderers(
        new List<Renderer>() { rend },
        outlineColor,
        HighlightsFX.SortingType.DepthFiltered);
  }

  public void UnHighlight() {
    outlineHighlight.ClearOutlineData();
  }

  private void SetCurrentHeading(Headings newHeading) {
    currentHeading = newHeading;
  }

  private void SetActiveSignalLight() {

  }
}
