using UnityEngine;

public class Spawner : MonoBehaviour
{
    public enum WallSide
    {
        Top,
        Right,
        Bottom,
        Left,
    }

    public WallSide wallSide;

    private void Start()
    {
        if (transform.parent.name.Equals("Top Spawners"))
        {
            wallSide = WallSide.Top;
        }
        else if (transform.parent.name.Equals("Right Spawners"))
        {
            wallSide = WallSide.Right;
        }
        else if (transform.parent.name.Equals("Bottom Spawners"))
        {
            wallSide = WallSide.Bottom;
        }
        else if (transform.parent.name.Equals("Left Spawners"))
        {
            wallSide = WallSide.Left;
        }
    }

    public bool CanSpawnPlane()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 50);

        foreach (Collider col in hitColliders)
        {
            if (col.gameObject.CompareTag("Aircraft"))
            {
                return false;
            }
        }

        return true;
    }

    public Plane SpawnPlane(Transform aircraftTransform, int speedMultipler)
    {
        Plane newPlane = Instantiate(aircraftTransform, transform.position, transform.rotation).GetComponent<Plane>();

        newPlane.InitAircraft(speedMultipler, wallSide);

        return newPlane;
    }
}
