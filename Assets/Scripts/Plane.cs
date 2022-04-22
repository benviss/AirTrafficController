using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class Plane : MonoBehaviour
{

    public float speed;
    public float baseSpeed;
    public float speedRange;
    public GameObject explosion;
    public GameObject leftLight;
    public GameObject rightLight;
    public GameObject tailLight;
    public Collector.WallSide finalDestination;

    private bool justSpawned = true;
    private HighlightsFX outlineHighlight;
    private Renderer rend;
    private Color outlineColor;
    private bool inWallCollider;
    private bool alarming;
    private float alarmTime = .3f;
    private float alarmDistance = 8;
    private Color alarmColor;
    private Light alarmLight;

    enum Headings
    {
        Up, Right, Down, Left
    }

    Headings currentHeading;
    Headings nextHeading;

    void Start()
    {
        outlineHighlight = FindObjectOfType<Camera>().GetComponent<HighlightsFX>();
        rend = this.GetComponentInChildren<Renderer>();
        outlineColor = new Color(1f, 1f, 1f, 1f);
        alarmColor = Color.red;
        nextHeading = currentHeading;

        alarmLight = gameObject.AddComponent<Light>();
        alarmLight.intensity = 1000;
        alarmLight.range = 18;
        alarmLight.type = LightType.Point;
        alarmLight.color = alarmColor;
        alarmLight.cullingMask = 0;
        SerializedObject serializedlight = new SerializedObject(alarmLight);
        serializedlight.FindProperty("m_DrawHalo").boolValue = true;
        serializedlight.ApplyModifiedProperties();
        alarmLight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forwardMovement = GetForwardMovement();
        transform.position += forwardMovement * speed * Time.deltaTime;

        float targetRotation = GetTargetRotation(nextHeading);
        float currentHeadingRotation = GetTargetRotation(currentHeading);
        if (nextHeading != currentHeading)
        {
            float angle = Mathf.LerpAngle(transform.rotation.eulerAngles.y, targetRotation, Time.deltaTime * speed);
            if (angle >= 356)
            {
                angle = 0;
            }
            if (Mathf.Abs(targetRotation - angle) < 4f)
            {
                angle = targetRotation;
            }

            transform.eulerAngles = new Vector3(0, angle, 0);
            if (targetRotation == transform.rotation.eulerAngles.y)
            {
                SetCurrentHeading(nextHeading);
            }
        }

        // scan for nearby obstacles (geese wont alarm)
        if (!alarming && !this.transform.tag.Equals("Airstacles") && ScanForAirstacles())
        {
            StartCoroutine(Alarming());
        }
    }

    IEnumerator Alarming()
    {
        alarming = true;
        alarmLight.enabled = true;
        yield return new WaitForSeconds(alarmTime);
        alarmLight.enabled = false;
        yield return new WaitForSeconds(alarmTime);
        alarming = false;
    }


    bool ScanForAirstacles()
    {
        // 11 is the collidable layer
        int layerMask = 11;
        var box = new Vector3(1, 1, 1);

        RaycastHit[] hits = Physics.BoxCastAll(transform.position + transform.forward*10, box*alarmDistance, Vector3.up);

        var nearyAircraft = hits
            .Where(h => (h.transform != this.transform) && h.transform.gameObject.layer == layerMask);

        return nearyAircraft.Any();
    }

    Vector3 GetForwardMovement()
    {
        switch (nextHeading)
        {
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

    float GetTargetRotation(Headings heading)
    {
        switch (heading)
        {
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

    public bool TurnAircraft(float direction)
    {
        if (currentHeading != nextHeading)
        {
            return false;
        }

        if (inWallCollider)
        {
            return false;
        }

        float currentRotation = GetTargetRotation(nextHeading);

        if ((Mathf.DeltaAngle(currentRotation, direction) > 90) || (Mathf.DeltaAngle(currentRotation, direction) == 0)) return false;

        AudioManager.Instance.PlayTurnSound();

        if (direction == 0)
        {
            nextHeading = Headings.Right;
        }
        else if (direction == 90)
        {
            nextHeading = Headings.Down;
        }
        else if (direction == 180)
        {
            nextHeading = Headings.Left;
        }
        else
        {
            nextHeading = Headings.Up;
        }
        // update curretntHeading

        return true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
        {
            Debug.Log("collider null?");
            return;
        }

        if (other.gameObject.layer.Equals(11))
        {
            if (!transform.tag.Equals("Airstacles"))
            {
                gameObject.transform.parent.gameObject.GetComponent<SpawnManager>().KillMe(this);
                GameObject newObject = (GameObject) Instantiate(explosion, this.transform.position, this.transform.rotation);
                Destroy(newObject, 3f);
                FindObjectOfType<TheGameManager>().GameOver();
            }

            Destroy(this.gameObject);
        }
        else if (other.gameObject.layer.Equals(9) && other.gameObject.GetComponent<Collector>().side != finalDestination)
        {
            inWallCollider = true;
            if (justSpawned) return;

            switch (currentHeading)
            {
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

    private void OnTriggerExit(Collider other)
    {
        if (other == null)
        {
            Debug.Log("collider null?");
            return;
        }

        justSpawned = false;
        if (other.gameObject.layer.Equals(9))
        {
            inWallCollider = false;
            //if it is correct collector tell game manager of success and destroy aircraft
            if (other.gameObject.GetComponent<Collector>().side == finalDestination)
            {
                SpawnManager.Instance.KillMe(this);
                TheGameManager.Instance.planesCollected++;
                Destroy(gameObject);
            }
        }
    }

    public void InitAircraft(int speedMultiplier, Spawner.WallSide startingWall)
    {

        speed = Random.Range((baseSpeed + speedMultiplier), ((baseSpeed + speedMultiplier) + speedRange));

        // Set the planes final destiantion so we know when to score it and which signal light to set active
        switch (startingWall)
        {

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

    public void Highlight()
    {
        outlineHighlight.ClearOutlineData();
        outlineHighlight.AddRenderers(
            new List<Renderer>() { rend },
            outlineColor,
            HighlightsFX.SortingType.DepthFiltered);
    }

    public void UnHighlight()
    {
        outlineHighlight.ClearOutlineData();
    }

    private void SetCurrentHeading(Headings newHeading)
    {
        currentHeading = newHeading;
        if (!transform.tag.Equals("Airstacles"))
        {
            SetActiveSignalLight();
        }
    }

    private void SetActiveSignalLight()
    {
        leftLight?.SetActive(false);
        rightLight?.SetActive(false);
        tailLight?.SetActive(false);

        if (currentHeading == Headings.Up)
        {
            if (finalDestination == Collector.WallSide.Top) return;

            if (finalDestination == Collector.WallSide.Left)
            {
                leftLight?.SetActive(true);
            }
            else if (finalDestination == Collector.WallSide.Right)
            {
                rightLight?.SetActive(true);
            }
            else if (finalDestination == Collector.WallSide.Bottom)
            {
                tailLight?.SetActive(true);
            }
        }

        else if (currentHeading == Headings.Right)
        {
            if (finalDestination == Collector.WallSide.Right) return;

            if (finalDestination == Collector.WallSide.Top)
            {
                leftLight?.SetActive(true);
            }
            else if (finalDestination == Collector.WallSide.Bottom)
            {
                rightLight?.SetActive(true);
            }
            else if (finalDestination == Collector.WallSide.Left)
            {
                tailLight?.SetActive(true);
            }
        }

        else if (currentHeading == Headings.Down)
        {
            if (finalDestination == Collector.WallSide.Bottom) return;

            if (finalDestination == Collector.WallSide.Top)
            {
                tailLight?.SetActive(true);
            }
            else if (finalDestination == Collector.WallSide.Right)
            {
                leftLight?.SetActive(true);
            }
            else if (finalDestination == Collector.WallSide.Left)
            {
                rightLight?.SetActive(true);
            }
        }

        else if (currentHeading == Headings.Left)
        {
            if (finalDestination == Collector.WallSide.Left) return;

            if (finalDestination == Collector.WallSide.Top)
            {
                rightLight?.SetActive(true);
            }
            else if (finalDestination == Collector.WallSide.Bottom)
            {
                leftLight?.SetActive(true);
            }
            else if (finalDestination == Collector.WallSide.Right)
            {
                tailLight?.SetActive(true);
            }
        }
    }
}
