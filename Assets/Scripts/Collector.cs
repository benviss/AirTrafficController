using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour {

  public enum WallSide {
    Top,Right,Bottom,Left
  }

  public WallSide side;

	// Use this for initialization
	void Start () {
	  if(gameObject.name == "Top") {
      side = WallSide.Top;
    }
    if(gameObject.name == "Right") {
      side = WallSide.Right;
    }
    if(gameObject.name == "Bottom") {
      side = WallSide.Bottom;
    }
    if(gameObject.name == "Left") {
      side = WallSide.Left;
    }
  }
	
	// Update is called once per frame
	void Update () {
		
	}
}
