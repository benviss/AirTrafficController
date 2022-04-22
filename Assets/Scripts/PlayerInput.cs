using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public ParticleSystem radioWaves;
    public Transform emitterPos;
    Plane targetedAirCraft;
    float minTargetTime = .25f;
    float lastTargetTime = 0;

    // Use this for initialization
    void Start()
    {
        targetedAirCraft = null;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Aircraft")
            {
                if (targetedAirCraft)
                {
                    targetedAirCraft.UnHighlight();
                }
                targetedAirCraft = hit.transform.gameObject.GetComponent<Plane>();
                lastTargetTime = Time.time;
                targetedAirCraft.Highlight();
            }
        }
        if (targetedAirCraft && (lastTargetTime + minTargetTime < Time.time))
        {
            targetedAirCraft.UnHighlight();
            targetedAirCraft = null;
        }

        if (targetedAirCraft)
        {
            CheckInput();
        }
    }

    void CheckInput()
    {
        float direction = -1;
        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            direction = 180;
        }
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            direction = 0;
        }
        if (Input.GetAxisRaw("Vertical") == 1)
        {
            direction = -90;
        }
        if (Input.GetAxisRaw("Vertical") == -1)
        {
            direction = 90;
        }
        if (direction != -1)
        {
            if (targetedAirCraft.TurnAircraft(direction))
            {
                ParticleSystem wavesParticle = Instantiate(radioWaves);
                wavesParticle.transform.position = emitterPos.position;
                Destroy(wavesParticle.gameObject, 1f);

                targetedAirCraft.UnHighlight();
                targetedAirCraft = null;
            }
        }
    }
}
