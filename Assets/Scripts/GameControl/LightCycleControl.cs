using System;
using UnityEngine;

public class LightCycleControl : MonoBehaviour
{
    public float dayTime = 8000f;
    public float gameSpeed = 1;
    public float tiltAngle = 10f;
    [Range(0f, 1f)]
    public float percentDay = .75f;

    public Light daySource;
    public Light nightSource;

    private float dayLength = 86400f;

    private void Start()
    {
        App.Instance.OnNewGame += HandleNewGameStarted;
    }

    private void HandleNewGameStarted()
    {
        dayTime = 8000f;
    }

    void Update()
    {
        UpdateTime();
        CalculateAngles();
        SmoothLights();
    }

    void UpdateTime()
    {
        dayTime += Time.deltaTime * gameSpeed;
        if (dayTime > dayLength)
        {
            dayTime -= dayLength;
            App.Instance.IncrementDay();
        }
        if (((dayTime - Time.deltaTime) * 2 < dayLength) &&
            (dayTime * 2 > dayLength))
        {
            App.Instance.IncrementHalfDay();
        }
    }

    void CalculateAngles()
    {
        float dayPercentage = dayTime / dayLength;
        float x = Mathf.Cos(dayPercentage * Mathf.PI * 2);
        float y = Mathf.Sin(dayPercentage * Mathf.PI * 2) + (percentDay - .5f) * 2;

        daySource.transform.position = new Vector3(x, y, -y * (tiltAngle / 90));
        nightSource.transform.position = new Vector3(-x, -y, y * (tiltAngle / 90));

        daySource.transform.LookAt(Vector3.zero);
        nightSource.transform.LookAt(Vector3.zero);
    }

    void SmoothLights()
    {
        if (daySource.transform.position.y < 0)
        {
            daySource.intensity = 1 + daySource.transform.position.y;
        }
        else
        {
            nightSource.intensity = (1 + nightSource.transform.position.y) * .5f;
        }
    }
}
