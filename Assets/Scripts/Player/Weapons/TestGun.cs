using System;
using System.Collections;
using System.Collections.Generic;
using Player.Weapons;
using UnityEngine;

public class TestGun : MonoBehaviour
{
    [SerializeField] private float _range = 100f;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private float _impactForce = 30f;
    [SerializeField] private float _recoilX = 2f;
    [SerializeField] private float _recoilY = 2f;
    [SerializeField] private float _recoilZ = 0.35f;
    
    public GameObject bulletOrigin;
    
    private Camera _mainCamera;
    private float _cooldownCounter = 0f;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetButtonDown(Controls.FIRE) && Time.time >= _cooldownCounter)
        {
            _cooldownCounter = Time.time + 1f / _fireRate;
            Shoot();
            
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(bulletOrigin.transform.position, bulletOrigin.transform.forward, out hit, _range))
        {
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * _impactForce);
            }
        }
        
        GunPlayEvents.Instance.GunRecoil(_recoilX,_recoilY,_recoilZ);

    }
    
    
}
