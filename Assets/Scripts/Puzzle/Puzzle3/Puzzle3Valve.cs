using System;
using System.Collections;
using UnityEngine;

public class Puzzle3Valve : MonoBehaviour
{
    [SerializeField] private ValveState _initialState;
    [SerializeField] private ParticleSystem _smokeParticles;
    [SerializeField] private ParticleSystem.MinMaxCurve _curve1;
    [SerializeField] private ParticleSystem.MinMaxCurve _curve2;
    [SerializeField] private ParticleSystem.MinMaxCurve _curve3;

    private ValveState _currentState;
    private bool _isMoving = false;
    private float _time = 0.75f;
    private ParticleSystem.SizeOverLifetimeModule _sizeOverLifetimeModule;

    void Start()
    {
        float initialX = transform.rotation.eulerAngles.x;
        float initialY = transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(initialX, initialY, GetRotation(_initialState));
        _sizeOverLifetimeModule = _smokeParticles.sizeOverLifetime;
        _sizeOverLifetimeModule.size = GetCurveFromState(_initialState);
        _currentState = _initialState;
    }

    public bool RotateValve(int direction, bool reverse)
    {
        int nextState = (int)_currentState + (direction * (reverse ? -1 : 1));
        if (_isMoving) // temporary :^)
            return true;

        if (nextState < 0 || nextState >= Enum.GetNames(typeof(ValveState)).Length)
            return false;

        _currentState = (ValveState)nextState;
        int rotation = GetRotation(_currentState) * (reverse ? -1 : 1);
        StartCoroutine(Rotate(rotation));
        return true;
    }

    private int GetRotation(ValveState state)
    {
        switch (state)
        {
            case ValveState.FULLY_OPEN:
                return Puzzle.FULLY_OPEN_ROTATION;

            case ValveState.PARTIALLY_OPEN:
                return Puzzle.PARTIALLY_OPEN_ROTATION;

            case ValveState.PARTIALLY_CLOSED:
                return Puzzle.PARTIALLY_CLOSED_ROTATION;

            case ValveState.FULLY_CLOSED:
                return Puzzle.FULLY_CLOSED_ROTATION;

            default:
                Debug.LogError("Valve rotation undefined");
                return 0;
        }
    }

    private ParticleSystem.MinMaxCurve GetCurveFromState(ValveState state)
    {
        switch(state)
        {
            case ValveState.FULLY_OPEN:
                return _curve1;

            case ValveState.PARTIALLY_OPEN:
                return _curve2;

            case ValveState.PARTIALLY_CLOSED:
                return _curve3;

            default:
                return _curve3;
        }
    }

    private IEnumerator Rotate(int pos)
    {
        if (!_isMoving)
        {
            _isMoving = true;
            LeanTween.rotateZ(gameObject, pos, _time)
                .setEase(LeanTweenType.easeInOutQuad);

            gameObject.tag = Tags.UNTAGGED;
            
            var randPitch = UnityEngine.Random.Range(0.8f, 1f);
            SoundManager.Instance.PlaySound(Sound.Countdown, transform, false, null, randPitch);

            _smokeParticles.Stop();
            _sizeOverLifetimeModule.size = GetCurveFromState(_currentState);
            if (_currentState != ValveState.FULLY_CLOSED)
                _smokeParticles.Play();

            yield return new WaitForSeconds(_time);

            gameObject.tag = Tags.INTERACTABLE;
            _isMoving = false;
        }
    }
}
