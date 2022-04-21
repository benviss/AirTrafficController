using UnityEngine;

public class FlashingLight : MonoBehaviour
{

    public Light light;

    public float flashTime = 1;
    private float flashCounter;

    void Start()
    {
        light = GetComponent<Light>();
    }

    void Update()
    {
        if (flashCounter < flashTime)
        {
            flashCounter += Time.deltaTime;
        }
        else
        {
            light.enabled = !light.enabled;
            flashCounter = 0;
        }
    }
}
