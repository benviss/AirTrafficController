﻿using UnityEngine;

public class HelicopterBlades : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 5, 0));
    }
}
