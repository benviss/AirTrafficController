using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour
{
    public ParticleSystem radioWaves;

    private Plane _targetedAirCraft;
    private float _minTargetTime = .25f;
    private float _lastTargetTime = 0;
    private Vector2 _mousePosition;
    private Transform _inputSignalEmitter;

    private readonly DynamicRef<LevelViewModel> _levelViewModel = DynamicRef.SearchByType<LevelViewModel>();

    void Start()
    {
        _targetedAirCraft = null;
        App.Instance.sceneController.OnSceneLoaded += HandleSceneLoaded;
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name.StartsWith(GameConstants.LevelIdentifier))
        {
            StartCoroutine(SetLevelRefs());
        }
    }

    private IEnumerator SetLevelRefs()
    {
        _inputSignalEmitter = null;

        if (!_levelViewModel.HasValue)
            yield return _levelViewModel.Wait;

        _inputSignalEmitter = _levelViewModel.Value.inputSignalEmitter;
    }

    void Update()
    {
        if (App.Instance.gameStarted)
            HandleAircraftTargeting();
    }

    private void HandleAircraftTargeting()
    {
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        var hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == GameConstants.Aircraft)
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

        if (_targetedAirCraft && 
            direction != -1 && 
            _targetedAirCraft.TurnAircraft(direction))
        {
            if (_inputSignalEmitter)
            {
                ParticleSystem wavesParticle = Instantiate(radioWaves);
                wavesParticle.transform.position = _inputSignalEmitter.position;
                Destroy(wavesParticle.gameObject, 1f);
            }

            _targetedAirCraft.UnHighlight();
            _targetedAirCraft = null;
        }
    }
}
