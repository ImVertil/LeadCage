using System;
using System.Collections;
using System.Collections.Generic;
using Player.Weapons;
using UnityEngine;
using Random = UnityEngine.Random;

public class Recoil : MonoBehaviour
{

    private Vector3 _currentRotation;
    private Vector3 _targetRotation;

    [SerializeField] private float _returnSpeed;
    [SerializeField] private float _snappiness;

    [SerializeField] private PlayerController _pc;
    void Start()
    {
        GunPlayEvents.OnGunRecoil += recoilFire;
    } 

    // Update is called once per frame
    void FixedUpdate()
    {
        /*_currentRotation = Vector3.Lerp(_currentRotation, Vector3.zero, _returnSpeed * Time.deltaTime);
        _targetRotation = Vector3.Slerp(_targetRotation, _currentRotation, _snappiness * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(_currentRotation);*/
        _targetRotation = Vector3.Lerp(_targetRotation, Vector3.zero, _returnSpeed * Time.deltaTime);
        _currentRotation = Vector3.Slerp(_currentRotation, _targetRotation, _snappiness * Time.fixedDeltaTime);
   
        transform.localRotation = Quaternion.Euler(_currentRotation);
    }

    void recoilFire(float recoilX, float recoilY, float recoilZ)
    {
        _targetRotation += new Vector3(-recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
        //_currentRotation += new Vector3(-recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}
