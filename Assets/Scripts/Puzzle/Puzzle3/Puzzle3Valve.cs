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
        _sizeOverLifetimeModule = _smokeParticles.sizeOverLifetime;
        _sizeOverLifetimeModule.size = GetCurveFromState(_initialState);
        _currentState = _initialState;
    }

    public void RotateValve(int direction, bool reverse)
    {
        if (_isMoving)
            return;

        int nextState = (int)_currentState + (direction * (reverse ? -1 : 1));
        if (nextState < 0 || nextState >= Enum.GetNames(typeof(ValveState)).Length)
        {
            InteractionManager.Instance.SetInfoText("It's not going any further than that...");
            
            return;
        }

        // we want to invoke the event whether the state goes into FULLY_CLOSED or goes out of it
        if((ValveState)nextState == ValveState.FULLY_CLOSED || _currentState == ValveState.FULLY_CLOSED)
        {
            PuzzleEvents.OnGeneratorPipeStatusChanged(this);
        }

        _currentState = (ValveState)nextState;
        int rotation = (int)transform.rotation.eulerAngles.z + Puzzle.ROTATION_VALUE * direction;
        StartCoroutine(Rotate(rotation));
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
            SoundManager.Instance.PlaySound(Sound.Valve_Squeak, transform, false, null, randPitch);

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
