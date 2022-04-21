using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool useOffsetValues;
    public float rotateSpeed;

    public Transform target;
    public Transform pivot;
    public Vector3 offset;
    public float maxViewAngle;
    public float minViewAngle;
    public bool invertX;

    private bool leftGate = false;
    // Use this for initialization
    void Start()
    {
        if (!useOffsetValues)
        {
            offset = target.position - transform.position;
        }

        pivot.transform.position = target.transform.position;
        pivot.transform.parent = target.transform;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float rotationDevider = rotateSpeed;

        // get the x position of the mouse and rotate the target
        //if((pivot.rotation.eulerAngles.x > maxViewAngle - 20) && pivot.rotation.eulerAngles.x < 180) {
        //  rotationDevider = maxViewAngle - pivot.rotation.eulerAngles.x;
        //}
        //float horizontal = Input.GetAxis("RightHorizontal") * rotateSpeed;
        //target.Rotate(0, horizontal, 0);


        //float vertical = Input.GetAxis("RightVertical") * rotateSpeed;
        //if(invertX)
        //  pivot.Rotate(-vertical, 0, 0);
        //else
        //  pivot.Rotate(vertical, 0, 0);

        //// Limit the up down camera rotation
        //if(pivot.rotation.eulerAngles.x > maxViewAngle && pivot.rotation.eulerAngles.x < 180)
        //  pivot.rotation = Quaternion.Euler(maxViewAngle, 0, 0);

        //if(pivot.rotation.eulerAngles.x < (360f + minViewAngle) && pivot.rotation.eulerAngles.x >= 180)
        //  pivot.rotation = Quaternion.Euler(360f + minViewAngle, 0, 0);

        //Move camera based on current rotation of the target and the original offset
        float desiredYAngle = target.eulerAngles.y;
        //float desiredXAngle = pivot.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(0, desiredYAngle, 0);

        transform.position = new Vector3(target.position.x, target.position.y + 5, target.position.z - 15);
        if (transform.position.x < 10)
        {
            transform.position = new Vector3(10, transform.position.y, transform.position.z);
        }
        //if(transform.position.y < target.position.y) {
        //  transform.position = new Vector3(transform.position.x, target.position.y - 0.5f, target.position.z - 15);
        //}
        //transform.position = target.position - offset;
        //transform.LookAt(target);
    }
}
