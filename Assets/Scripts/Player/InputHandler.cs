using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public ParticleSystem radioWaves;
    public Transform emitterPos;

    private Plane _targetedAirCraft;
    private float _minTargetTime = .25f;
    private float _lastTargetTime = 0;
    private Vector2 _mousePosition;

    void Start()
    {
        _targetedAirCraft = null;
    }

    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        var hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Aircraft")
            {
                if (_targetedAirCraft)
                {
                    _targetedAirCraft.UnHighlight();
                }
                _targetedAirCraft = hit.transform.gameObject.GetComponent<Plane>();
                _lastTargetTime = Time.time;
                _targetedAirCraft.Highlight();
            }
        }

        if (_targetedAirCraft && (_lastTargetTime + _minTargetTime < Time.time))
        {
            _targetedAirCraft.UnHighlight();
            _targetedAirCraft = null;
        }
    }

    private void OnChangeDirection(InputValue value)
    {
        var input = value.Get<Vector2>();
        var direction = -1;

        if (input.x == -1)
        {
            direction = 180;
        }
        else if (input.x == 1)
        {
            direction = 0;
        }
        else if (input.y == 1)
        {
            direction = -90;
        }
        else if (input.y == -1)
        {
            direction = 90;
        }

        if (_targetedAirCraft && direction != -1)
        {
            if (_targetedAirCraft.TurnAircraft(direction))
            {
                ParticleSystem wavesParticle = Instantiate(radioWaves);
                wavesParticle.transform.position = emitterPos.position;
                Destroy(wavesParticle.gameObject, 1f);

                _targetedAirCraft.UnHighlight();
                _targetedAirCraft = null;
            }
        }
    }

    private void OnClick(InputValue value)
    {
        TheGameManager.Instance.MouseClicked();
    }
}
