using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBulb:MonoBehaviour {

  Renderer rend;
  private void Start() {
    rend = GetComponent<Renderer>();
  }

  public void SetColor(Color color) {
    rend.material.color = color;
  }
}
