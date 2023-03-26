using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RifleController : MonoBehaviour
{
    private Animator _animator;
    private PlayerController _controller;
    private RigBuilder _rigBuilder;
    private Rig _hipsRig;
    private Rig _handsRig;
    private Rig _aimRig;
    private Rig _weaponPullRig;
    private TwoBoneIKConstraint _rightHandConstraint;
    private TwoBoneIKConstraint _leftHandConstraint;

    private int _riflePulledOutHash;
    private int _aimingHash;

    private bool _riflePulledOut = false;
    private bool _aiming = false;
    private bool _waiting = false;

    private float _startSensitivity;

    private float _desiredHipsRigWeight = 0f;
    private float _desiredHandsRigWeight = 0f;
    private float _desiredAimRigWeight = 0f;
    private float _desiredPullRigWeight = 0f;

    private float _hipRigWeightVelocity = 0f;
    private float _handRigWeightVelocity = 0f;
    private float _aimRigWeightVelocity = 0f;
    private float _pullRigweightVelocity = 0f;

    private float _putSpeed = 0.5f;
    private float _glueSpeed = 0.2f;

    [SerializeField] private float ArmLayerWeightDelay = 0.5f;
    [SerializeField] private GameObject WeaponPivot;
    [SerializeField] private GameObject RightHandIK;
    [SerializeField] private GameObject LeftHandIK;

    private void Start()
    {
        
        _animator = GetComponent<Animator>();
        _controller = GetComponent<PlayerController>();
        _rigBuilder = GetComponent<RigBuilder>();

        _rightHandConstraint = RightHandIK.GetComponent<TwoBoneIKConstraint>();
        _leftHandConstraint = LeftHandIK.GetComponent<TwoBoneIKConstraint>();

        _startSensitivity = _controller.MouseSensitivity;

        _hipsRig = _rigBuilder.layers[0].rig;
        _handsRig = _rigBuilder.layers[3].rig;
        _aimRig = _rigBuilder.layers[1].rig;
        _weaponPullRig = _rigBuilder.layers[2].rig;

        _riflePulledOutHash = Animator.StringToHash("RiflePulledOut");
        _aimingHash = Animator.StringToHash("Aiming");
    }

    // Update is called once per frame
    void Update()
    {
       SheatheUnsheatheRifle();
       TakeAim();
    }

    private void TakeAim()
    {
        if (Input.GetButton(Controls.AIM) && _riflePulledOut)
        {
            _aiming = true;
            _desiredAimRigWeight = 1f;
        }
        else
        {
            _aiming = false;
            _desiredAimRigWeight = 0f;
        }

        _aimRig.weight = Mathf.SmoothDamp(_aimRig.weight, _desiredAimRigWeight, ref _aimRigWeightVelocity, 0.2f);
        _animator.SetBool(_aimingHash, _aiming);
    }

    private void SheatheUnsheatheRifle()
    {
        if (Input.GetButtonDown(Controls.WEAPON) && !_waiting)
        {
            _animator.SetBool(_riflePulledOutHash, !_riflePulledOut);
            _riflePulledOut = !_riflePulledOut;
            if (_riflePulledOut)
            {
                WeaponPivot.SetActive(true);
                _putSpeed = 0.5f;
                _glueSpeed = 0.5f;
                _animator.SetLayerWeight(1, 1);
                _desiredHipsRigWeight = 1;
                _desiredHandsRigWeight = 1;
                _weaponPullRig.weight = 1;
                _desiredPullRigWeight = 0;
            }
            else
            {
                _putSpeed = 0.5f;
                _glueSpeed = 0.2f;
                _desiredHipsRigWeight = 0;
                _desiredHandsRigWeight = 0;
                _desiredPullRigWeight = 1;
                StartCoroutine(WaitToChangeWeight());
            }
        }

        _handsRig.weight = Mathf.SmoothDamp(_handsRig.weight, _desiredHandsRigWeight, ref _handRigWeightVelocity, _putSpeed);
        _hipsRig.weight = Mathf.SmoothDamp(_hipsRig.weight, _desiredHipsRigWeight, ref _hipRigWeightVelocity, _putSpeed);
        _weaponPullRig.weight = Mathf.SmoothDamp(_weaponPullRig.weight, _desiredPullRigWeight, ref _pullRigweightVelocity, _glueSpeed);
    }

    IEnumerator WaitToChangeWeight()
    {
        _waiting = true;
        yield return new WaitForSeconds(ArmLayerWeightDelay);
        _waiting = false;
        WeaponPivot.SetActive(false);
        _animator.SetLayerWeight(1, 0);
    }
}
